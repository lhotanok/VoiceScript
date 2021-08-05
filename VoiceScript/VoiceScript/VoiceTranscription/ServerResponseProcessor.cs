using System;
using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;

namespace VoiceScript.VoiceTranscription
{
    class ServerResponseProcessor
    {
        public void ProcessSpeechRecognitionTranscript(RepeatedField<SpeechRecognitionResult> results, Action<string> callback)
        {
            foreach (SpeechRecognitionResult result in results)
            {
                ProcessRecognitionAlternative(result.Alternatives, callback);
            }
        }

        public void ProcessStreamRecognitionTranscript(RepeatedField<StreamingRecognitionResult> results, Action<string> callback)
        {
            foreach (StreamingRecognitionResult result in results)
            {
                ProcessRecognitionAlternative(result.Alternatives, callback);
            }
        }

        void ProcessRecognitionAlternative(RepeatedField<SpeechRecognitionAlternative> alternatives, Action<string> callback)
        {
            foreach (SpeechRecognitionAlternative alternative in alternatives)
            {
                callback(alternative.Transcript);
            }
        }
    }
}
