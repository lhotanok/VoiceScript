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
        internal void Play(string audioFilename)
        {
            reader = new WaveFileReader(audioFilename);
            waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.Play();
            waveOut.PlaybackStopped += (sender, e) =>
            {
                waveOut.Stop();
                reader.Dispose();
            };
        }
    }
}
