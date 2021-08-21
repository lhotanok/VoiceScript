using System;
using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;

namespace VoiceScript.VoiceTranscription
{
    static class ServerResponseProcessor
    {
        public static void ProcessSpeechRecognitionTranscript(RepeatedField<SpeechRecognitionResult> results, Action<string> callback)
        {
            foreach (SpeechRecognitionResult result in results)
            {
                ProcessRecognitionAlternative(result.Alternatives, callback);
            }
        }

        public static void ProcessStreamRecognitionTranscript(RepeatedField<StreamingRecognitionResult> results, Action<string> callback)
        {
            foreach (StreamingRecognitionResult result in results)
            {
                ProcessRecognitionAlternative(result.Alternatives, callback);
            }
        }

        static void ProcessRecognitionAlternative(RepeatedField<SpeechRecognitionAlternative> alternatives, Action<string> callback)
        {
            foreach (SpeechRecognitionAlternative alternative in alternatives)
            {
                callback(alternative.Transcript);
            }
        }
    }
}
