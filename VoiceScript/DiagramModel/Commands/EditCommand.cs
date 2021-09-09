using System;
using VoiceScript.DiagramModel.Commands.LanguageFormats;

namespace VoiceScript.DiagramModel.Commands
{
    public class EditCommand : Command
    {
        static readonly string defaultFormat = "edit";
        public static string DefaultFormat { get => defaultFormat; }
        public EditCommand(string name, string targetType, string targetName, LanguageFormat languageFormat)
            : base(name, targetType, targetName, languageFormat) { }

        public override void Execute(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                if (IsEditNameCommand() || IsChildComponentTypeCompatible(context.CurrentComponent, translatedTargetType))
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

        bool IsEditNameCommand() => targetType == language.ComponentNameFormat;

        void ChangeEditedComponentName(CommandExecutionContext context)
        {
            var validTargetValue = translatedTargetValue ?? targetValue;

            context.CurrentComponent.Name = validTargetValue;
            context.CommandExecuted = true;
        }

        void EnterEditedComponentContext(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                var validTargetValue = translatedTargetValue ?? targetValue;
                var childToEdit = context.CurrentComponent.FindChild(translatedTargetType, validTargetValue);

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
