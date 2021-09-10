using System;

namespace DiagramModel.Commands
{
    public class CommandExecutionException : InvalidOperationException
    {
        public CommandExecutionException(string message) : base(message) { }
    }
}