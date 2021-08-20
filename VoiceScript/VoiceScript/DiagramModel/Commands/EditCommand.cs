using System;

namespace VoiceScript.DiagramModel.Commands
{
    class EditCommand : Command
    {
        public EditCommand(string targetType, string targetName) : base(targetType, targetName) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            if (targetType == "name")
            {
                context.CurrentComponent.Name = targetValue;
                context.CommandExecuted = true;
            }
            else
            {
                while (context.CurrentComponent != null && !context.CommandExecuted)
                {
                    var childToEdit = context.CurrentComponent.FindChild(targetType, targetValue);

                    if (childToEdit != null)
                    {
                        context.TargetComponent = childToEdit;
                        context.CommandExecuted = true;
                    }
                    else
                    {
                        context.CurrentComponent = context.CurrentComponent.Parent;
                    }
                }

                throw new InvalidOperationException("Component can not be edited. It does not exist in the current context.");
            }
        }
    }
}
