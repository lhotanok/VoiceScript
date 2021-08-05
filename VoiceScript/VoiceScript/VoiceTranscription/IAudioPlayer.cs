using System;

namespace VoiceScript.VoiceTranscription
{
    interface IAudioPlayer
    {
        void Play(string audioFilename, Action PlaybackStoppedCallback = null);
        void Stop();
    }
}
