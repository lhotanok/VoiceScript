using DiagramModel.Components;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Context for command execution. Contains info about
    /// start component, target component and current command
    /// execution result.
    /// </summary>
    public class CommandExecutionContext
    {
        /// <summary>
        /// Component which should be inspected as a starting point
        /// for command execution. If the command is not valid in the
        /// context of this component, <see cref="CurrentComponent"/>
        /// value is updated to <see cref="Component.Parent"/>. 
        /// </summary>
        public Component CurrentComponent { get; set; }

        /// <summary>
        /// Component in whose context the current command was finally executed.
        /// </summary>
        public Component TargetComponent { get; set; }

        /// <summary>
        /// Specifies whether current command was executed or not.
        /// </summary>
        public bool CommandExecuted { get; set; }
    }
}
