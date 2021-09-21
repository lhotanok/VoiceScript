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

        /// <summary>
        /// Number of command whose execution thrown an exception.
        /// Is useful to emphasise where exactly the error happened.
        /// </summary>
        public int CommandNumber { get; }
    }
}