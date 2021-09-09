using System;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandExecutionException : InvalidOperationException
    {
        public CommandExecutionException(string message) : base(message) { }
    }
}