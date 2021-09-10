using System;

namespace DiagramModel.Commands
{
    public class CommandExecutionException : InvalidOperationException
    {
        public CommandExecutionException(string message, int commandOrderNumber = 0) : base(message)
        {
            CommandNumber = commandOrderNumber;
        }

        public int CommandNumber { get; }
    }
}