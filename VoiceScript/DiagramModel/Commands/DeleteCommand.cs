using System;

namespace VoiceScript.DiagramModel.Commands
{
    public class DeleteCommand : Command
    {
        static readonly string defaultFormat = "delete";
        public static string DefaultFormat { get => defaultFormat; }
        public DeleteCommand(string name, string targetType, string targetName) : base(name, targetType, targetName) { }

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
