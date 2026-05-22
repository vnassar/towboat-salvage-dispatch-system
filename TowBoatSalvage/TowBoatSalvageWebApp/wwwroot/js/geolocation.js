window.appGeolocation = {
    getCurrentPosition: function (options) {
        return new Promise(function (resolve, reject) {
            if (!navigator.geolocation) {
                reject({ message: 'Geolocation not supported' });
                return;
            }
            navigator.geolocation.getCurrentPosition(
                function (pos) {
                    resolve({
                        latitude: pos.coords.latitude,
                        longitude: pos.coords.longitude,
                        accuracy: pos.coords.accuracy,
                        timestamp: pos.timestamp
                    });
                },
                function (err) {
                    reject({ code: err.code, message: err.message });
                },
                options || {}
            );
        });
    },

    // start watching; dotNetRef is a DotNetObjectReference passed from Blazor
    // dotNetRef.invokeMethodAsync('ReceivePosition', position) will be called repeatedly
    watchPosition: function (dotNetRef, options) {
        if (!navigator.geolocation) {
            return -1;
        }
        const id = navigator.geolocation.watchPosition(
            function (pos) {
                const payload = {
                    latitude: pos.coords.latitude,
                    longitude: pos.coords.longitude,
                    accuracy: pos.coords.accuracy,
                    timestamp: pos.timestamp
                };
                try {
                    dotNetRef.invokeMethodAsync('ReceivePosition', payload);
                } catch (err) {
                    // ignore if DotNet object disposed
                    console.error('dotNetRef.invokeMethodAsync error', err);
                }
            },
            function (err) {
                try {
                    dotNetRef.invokeMethodAsync('ReceivePositionError', { code: err.code, message: err.message });
                } catch (err2) {
                    console.error('dotNetRef.invokeMethodAsync error', err2);
                }
            },
            options || {}
        );
        return id;
    },

    clearWatch: function (watchId) {
        if (watchId != null && watchId !== -1) {
            try {
                navigator.geolocation.clearWatch(watchId);
            } catch (err) {
                console.error('clearWatch error', err);
            }
        }
    }
};