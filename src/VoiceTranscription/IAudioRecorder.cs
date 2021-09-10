using System;
using System.IO;
using NAudio.Wave;

namespace VoiceScript.VoiceTranscription
{
    public interface IAudioRecorder : IDisposable
    {
        BufferedWaveProvider WaveProvider { get; }
        WaveFileWriter AudioStream { get; }
        bool Recording { get; }
        string OutputFilename { get; }
        void StartRecording(Action RecordingStoppedCallback = null);
        void StopRecording();
        double GetFileSecondsLength(string audioFilename);
        int ConvertSecondsToBytes(int seconds);
        bool RecordingDeviceAvailable();
    }
}
