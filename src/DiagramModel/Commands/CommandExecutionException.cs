using System;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Thrown if an error is detected during command execution.
    /// </summary>
    public class CommandExecutionException : InvalidOperationException
    {
        public CommandExecutionException(string message, int commandOrderNumber = 0) : base(message)
        {
            CommandNumber = commandOrderNumber;
        }

        public int CommandNumber { get; }
    }
}