using System;
using DiagramModel.Commands.LanguageFormats;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Representation of command for component editation.
    /// </summary>
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
                executedIn = context.CurrentComponent;
                clonedExecutedIn = context.CurrentComponent.Clone();

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
                throw new CommandExecutionException("Edit command can not be executed in the current context.");

        }
        protected override void ProcessCommand(CommandExecutionContext context)
        {
            if (IsEditNameCommand())
            {
                ChangeEditedComponentName(context);
            }
            else
            {
                EnterEditedComponentContext(context);
            }
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
            var validTargetValue = translatedTargetValue ?? targetValue;
            var childToEdit = context.CurrentComponent.FindChild(translatedTargetType, validTargetValue);

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
    }
}
