using System;
using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Handles recognition results in Google Cloud Speech format.
    /// </summary>
    public static class ServerResponseProcessor
    {
        /// <summary>
        /// Triggers provided callback with each <see cref="SpeechRecognitionResult"/>.
        /// Inspects result alternatives <see cref="SpeechRecognitionResult.Alternatives"/>
        /// and binds them with callback.
        /// </summary>
        /// <param name="results">Collection of speech recognition results with transcription details.</param>
        /// <param name="callback">Function to be called with the individual transcription parts
        /// from <see cref="SpeechRecognitionAlternative.Transcript"/>.
        /// Typical use case is for writing transcripted words to the stream.</param>
        public static void ProcessSpeechRecognitionTranscript(RepeatedField<SpeechRecognitionResult> results, Action<string> callback)
        {
            foreach (SpeechRecognitionResult result in results)
            {
                ProcessRecognitionAlternative(result.Alternatives, callback);
            }
        }

        /// <summary>
        /// Triggers provided callback with each <see cref="StreamingRecognitionResult"/>.
        /// Inspects result alternatives <see cref="StreamingRecognitionResult.Alternatives"/>
        /// and binds them with callback.
        /// </summary>
        /// <param name="results">Collection of speech recognition results with transcription details.</param>
        /// <param name="callback">Function to be called with the individual transcription parts
        /// from <see cref="SpeechRecognitionAlternative.Transcript"/>.
        /// Typical use case is for writing transcripted words to the stream.</param>
        public static void ProcessStreamRecognitionTranscript(RepeatedField<StreamingRecognitionResult> results, Action<string> callback)
        {
            foreach (StreamingRecognitionResult result in results)
            {
                ProcessRecognitionAlternative(result.Alternatives, callback);
            }
        }

        /// <summary>
        /// Triggers provided callback with each <see cref="SpeechRecognitionAlternative"/>.
        /// </summary>
        /// <param name="alternatives">Collection of speech recognition alternatives with transcripted words.</param>
        /// <param name="callback">Function to be called with the individual transcription alternatives
        /// from <see cref="SpeechRecognitionAlternative"/>. Typical use case is for writing
        /// transcripted words to the stream.</param>
        static void ProcessRecognitionAlternative(RepeatedField<SpeechRecognitionAlternative> alternatives, Action<string> callback)
        {
            foreach (SpeechRecognitionAlternative alternative in alternatives)
            {
                callback(alternative.Transcript);
            }
        }
    }
}
