using System;
using NAudio.Wave;

namespace VoiceScript.VoiceTranscription
{
    public class AudioPlayer : IAudioPlayer
    {
        WaveFileReader reader;
        WaveOutEvent waveOut;
        Action PlayStoppedCallback;

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
