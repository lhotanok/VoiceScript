using Google.Cloud.Speech.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript
{
    interface IVoiceTranscriptor
    {
        RecognitionConfig Configuration { get; }
        void DoRealTimeTranscription(Action<string> callback);
        string GetTranscription(string filename, Action<string> callback);
        Task<string> GetTranscriptionAsync(string filename);

    }
}
