using System;
using NAudio.Wave;

namespace VoiceScript
{
    class AudioPlayer
    {
        WaveFileReader reader;
        WaveOutEvent waveOut;
        Action PlayStoppedCallback;

        /// <summary>
        /// Start audio playing from the given file.
        /// </summary>
        /// <param name="audioFilename"></param>
        /// <param name="PlaybackStoppedCallback"></param>
        public void Play(string audioFilename, Action PlaybackStoppedCallback = null)
        {
            PlayStoppedCallback = PlaybackStoppedCallback;
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

        void PlaybackStoppedHandler(object sender, StoppedEventArgs e) 
        {
            Stop();
            PlayStoppedCallback?.Invoke();
        }
    }
}
