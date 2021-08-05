using System;
using NAudio.Wave;

namespace VoiceScript.VoiceTranscription
{
    interface IAudioRecorder : IDisposable
    {
        BufferedWaveProvider WaveProvider { get; }
        WaveFileWriter AudioStream { get; }
        bool Recording { get; }
        void StartRecording(Action RecordingStoppedCallback = null);
        void StopRecording();
        double GetFileSecondsLength(string audioFilename);
    }
}
