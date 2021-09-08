using System;

namespace VoiceScript.DiagramModel.Commands
{
    public class EditCommand : Command
    {
        static readonly string defaultFormat = "edit";
        public static string DefaultFormat { get => defaultFormat; }
        public EditCommand(string name, string targetType, string targetName) : base(name, targetType, targetName) { }

        public override void Execute(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                if (IsEditNameCommand() || IsChildComponentTypeCompatible(context.CurrentComponent, targetType))
                {
                    ProcessCommand(context);
                }
                else
                {
                    context.CurrentComponent = context.CurrentComponent.Parent;
                }
            }

            if (context.CurrentComponent == null)
                throw new InvalidOperationException("Command can not be executed in the current context.");
        }
        protected override void ProcessCommand(CommandExecutionContext context)
        {
            if (IsEditNameCommand()) ChangeEditedComponentName(context);
            else EnterEditedComponentContext(context);
        }

        bool IsEditNameCommand() => targetType == "name";

        void ChangeEditedComponentName(CommandExecutionContext context)
        {
            context.CurrentComponent.Name = targetValue;
            context.CommandExecuted = true;
        }

        void EnterEditedComponentContext(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                var childToEdit = context.CurrentComponent.FindChild(targetType, targetValue);

                if (childToEdit != null)
                {
                    context.TargetComponent = childToEdit;
                    context.CommandExecuted = true;
                    return;
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
