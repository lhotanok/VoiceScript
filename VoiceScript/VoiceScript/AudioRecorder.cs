using System;
using System.Windows.Forms;

using NAudio.Wave;

namespace VoiceScript
{
    class AudioRecorder : IAudioRecorder
    {
        readonly BufferedWaveProvider waveProvider;
        readonly WaveInEvent waveIn;
        WaveFileWriter writer;

        VoiceDetection voiceDetection;
        readonly Timer timer;
        readonly string outputFilename;

        public AudioRecorder(string outputAudioFilename, Timer recordingTimer)
        {
            voiceDetection = VoiceDetection.Waiting;
            timer = recordingTimer;

            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1),
                DeviceNumber = 0
            };
            waveIn.DataAvailable += DataAvailableHandler;
            waveIn.RecordingStopped += RecordingStoppedHandler;

            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat)
            {
                DiscardOnBufferOverflow = true
            };

            outputFilename = outputAudioFilename;
        }

        public BufferedWaveProvider WaveProvider => waveProvider;
        public bool Recording => voiceDetection.Equals(VoiceDetection.Recording);

        public void StartRecording()
        {
            voiceDetection = VoiceDetection.Recording;
            writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
            waveIn.StartRecording();
        }

        public void StopRecording()
        {
            voiceDetection = VoiceDetection.Stopped;
            waveIn.StopRecording();
        }

        /// <summary>
        /// Safe release of the recording device.
        /// </summary>
        public void Dispose()
        {
            StopRecording();
            writer?.Dispose();
            waveIn?.Dispose();
        }

        void DataAvailableHandler(object sender, WaveInEventArgs e)
        {
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            WriteAudioStreamIntoFileWriter(e.Buffer, e.BytesRecorded);
        }

        void RecordingStoppedHandler(object sender, StoppedEventArgs e)
        {
            writer?.Dispose();
            writer = null;

            timer.Enabled = false;
            voiceDetection = VoiceDetection.Waiting;
        }

        /// <summary>
        /// Append new bytes of audio stream into <see cref="WaveFileWriter"/>.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytesToWrite"></param>
        void WriteAudioStreamIntoFileWriter(byte[] buffer, int bytesToWrite)
        {
            // if (File.Exists(audioFilename)) File.Delete(audioFilename);
            const int tenMinutes = 60 * 10;
            int maxFileSize = tenMinutes * waveIn.WaveFormat.AverageBytesPerSecond;
            int remainingFileSize = (int)Math.Min(bytesToWrite, maxFileSize - writer.Length);

            if (remainingFileSize > 0)
            {
                writer.Write(buffer, 0, bytesToWrite);
            }
            else
            {
                writer.Dispose();
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
            }
        }
    }
}
