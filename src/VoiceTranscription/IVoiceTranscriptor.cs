using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;
using Microsoft.CognitiveServices.Speech;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Manages speech-to-text transcription.
    /// </summary>
    public interface IVoiceTranscriptor
    {
        /// <summary>
        /// Current configuration for speech recognition.
        /// Contains info about audio format, transcription language and much more.
        /// </summary>
        SpeechConfig Configuration { get; }

        /// <summary>
        /// Sets file transcription and binds it to the provided callback.
        /// Encapsulates the whole process into the task. 
        /// </summary>
        /// <param name="audioFilename">Name of the audio file to create transcription for.</param>
        /// <param name="callback">Function to be called with the individual transcription parts.
        /// Typical use case is for writing new words to the stream.</param>
        /// <returns>Task representing transcription process.</returns>
        Task CreateTranscriptionTask(string audioFilename, Action<string> callback);

        /// <summary>
        /// Performs real-time transcription from audio to text.
        /// Experimental feature.
        /// </summary>
        /// <param name="callback">Function to be called with the individual transcription parts.
        /// Typical use case is for writing new words to the stream.</param>
        Task DoRealTimeTranscription(Action<string> callback);
    }
}
