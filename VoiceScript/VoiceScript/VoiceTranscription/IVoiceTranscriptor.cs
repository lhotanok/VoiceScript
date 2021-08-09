using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    interface IVoiceTranscriptor
    {
        RecognitionConfig Configuration { get; }

        /// <summary>
        /// Synchronous conversion from the file saved under the given filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        string GetTranscription(string filename, Action<string> callback);

        /// <summary>
        /// Performs real-time transcription from audio to text.
        /// </summary>
        /// <param name="callback">Action to be done with the 
        /// transcription in string format.</param>
        void DoRealTimeTranscription(Action<string> callback);
    }
}
