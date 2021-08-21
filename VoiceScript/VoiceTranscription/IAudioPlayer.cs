using System;

namespace VoiceScript.VoiceTranscription
{
    public interface IAudioPlayer
    {
        void Play(string audioFilename, Action PlaybackStoppedCallback = null);
        void Stop();
    }
}
