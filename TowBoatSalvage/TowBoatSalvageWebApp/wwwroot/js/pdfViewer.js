window.pdfViewer = {
    openPdfInNewTab: function (base64) {
        try {
            // decode base64 to binary
            const byteCharacters = atob(base64);
            const byteNumbers = new Array(byteCharacters.length);
            for (let i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            const byteArray = new Uint8Array(byteNumbers);

            // create a blob and open in new tab
            const blob = new Blob([byteArray], { type: "application/pdf" });
            const url = URL.createObjectURL(blob);
            const newWindow = window.open(url, "_blank");

            if (!newWindow) {
                // popup blocked — fallback to data URL navigation
                window.location.href = "data:application/pdf;base64," + base64;
            } else {
                // revoke object URL after a short delay to avoid memory leak
                setTimeout(() => URL.revokeObjectURL(url), 10000);
            }
        } catch (err) {
            console.error("pdfViewer.openPdfInNewTab error:", err);
        }
    },
    downloadPdf: function (base64, fileName) {
        try {
            const byteCharacters = atob(base64);
            const byteNumbers = new Array(byteCharacters.length);
            for (let i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            const byteArray = new Uint8Array(byteNumbers);
            const blob = new Blob([byteArray], { type: "application/pdf" });
            const url = URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            setTimeout(() => URL.revokeObjectURL(url), 5000);
        } catch (err) {
            console.error("pdfViewer.downloadPdf error:", err);
        }
    }
};