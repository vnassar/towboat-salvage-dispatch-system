let audio;

export function playNewWorkOrderSound() {
    if (!audio) {
        audio = new Audio('sounds/notification-sound.mp3');
        audio.preload = 'auto';
        audio.volume = 0.9;
    }

    audio.currentTime = 0;
    audio.play().catch(() => {
        // Browser may block autoplay until user interaction.
    });
}