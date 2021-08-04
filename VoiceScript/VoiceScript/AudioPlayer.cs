using NAudio.Wave;
using System.IO;

namespace VoiceScript
{
    class AudioPlayer
    {
        WaveFileReader reader;
        WaveOutEvent waveOut;

        /// <summary>
        /// Start audio playing from the given file.
        /// </summary>
        /// <param name="audioFilename"></param>
        public void Play(string audioFilename)
        {
            reader = new WaveFileReader(audioFilename);

            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.PlaybackStopped += PlaybackStoppedHandler;
            waveOut.Play();
        }

        public void Stop()
        {
            waveOut.Stop();
            waveOut.Dispose();
            waveOut = null;

            reader.Dispose();
            reader = null;
        }

        void PlaybackStoppedHandler(object sender, StoppedEventArgs e) => Stop();
    }
}
