using System;
using System.IO;
using NAudio.Wave;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Handles audio recording, buffer storage and file storage.
    /// </summary>
    public interface IAudioRecorder : IDisposable
    {
        /// <summary>
        /// Stores buffered audio waves.
        /// </summary>
        BufferedWaveProvider WaveProvider { get; }

        /// <summary>
        /// Writes audio waves into file using byte stream.
        /// </summary>
        WaveFileWriter AudioStream { get; }
        
        /// <summary>
        /// Current state of the recording process.
        /// </summary>
        bool Recording { get; }

        /// <summary>
        /// Path to the file where raw audio is stored.
        /// </summary>
        string OutputFilename { get; }

        /// <summary>
        /// Starts recording process.
        /// Sets callback for stopped recording event.
        /// </summary>
        /// <param name="RecordingStoppedCallback">Specifies action that should be 
        /// performed once the recording stops.</param>
        void StartRecording(Action RecordingStoppedCallback = null);
        
        /// <summary>
        /// Exits the recording process.
        /// </summary>
        void StopRecording();

        /// <summary>
        /// Calculates audio duration in seconds.
        /// </summary>
        /// <param name="audioFilename">Path to the audio file</param>
        /// <returns></returns>
        double GetFileSecondsLength(string audioFilename);

        /// <summary>
        /// Converts seconds to bytes with respect to raw audio format.
        /// </summary>
        /// <param name="seconds">Number of seconds to convert into bytes</param>
        /// <returns></returns>
        int ConvertSecondsToBytes(int seconds);

        /// <summary>
        /// Finds out if there is any recording device that might be used.
        /// </summary>
        /// <returns></returns>
        bool RecordingDeviceAvailable();
    }
}
