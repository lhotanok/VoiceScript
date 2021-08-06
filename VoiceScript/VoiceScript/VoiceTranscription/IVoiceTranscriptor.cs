using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    interface IVoiceTranscriptor
    {
        RecognitionConfig Configuration { get; }
        void DoRealTimeTranscription(Action<string> callback);
        string GetTranscription(string filename, Action<string> callback);
        string GetLongTranscription(string filename, Action<string> callback);
        Task MakeTranscriptionAsync(string filename, Action<string> callback);
    }
}
