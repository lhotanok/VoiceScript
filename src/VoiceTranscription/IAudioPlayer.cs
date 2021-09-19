using System;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Manages audio playback from the file.
    /// </summary>
    public interface IAudioPlayer
    {
        /// <summary>
        /// Plays audio stored under the given file path.
        /// Handles potential callback once the audio playback stopped.
        /// </summary>
        /// <param name="audioFilename">Path to the audio file</param>
        /// <param name="PlaybackStoppedCallback">Callback to trigger after audio stopped playing</param>
        void Play(string audioFilename, Action PlaybackStoppedCallback = null);

        /// <summary>
        /// Stops current audio playback immediately.
        /// </summary>
        void Stop();
    }
}
