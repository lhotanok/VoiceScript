using System;

namespace VoiceScript.DiagramModel.Commands
{
    public class DeleteCommand : Command
    {
        public DeleteCommand(string targetType, string targetName) : base(targetType, targetName) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                if (context.CurrentComponent.TryDeleteChild(targetType, targetValue))
                {
                    context.TargetComponent = context.CurrentComponent;
                    context.CommandExecuted = true;
                    return;
                }
                else
                {
                    context.CurrentComponent = context.CurrentComponent.Parent;
                }
            }

            throw new InvalidOperationException("Component can not be deleted. It does not exist in the current context.");
        }
    }
}
