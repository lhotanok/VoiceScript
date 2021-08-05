﻿using System;
using NAudio.Wave;

namespace VoiceScript
{
    interface IAudioRecorder : IDisposable
    {
        public BufferedWaveProvider WaveProvider { get; }
        public bool Recording { get; }
        public void StartRecording();
        public void StopRecording();
    }
}