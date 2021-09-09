using System;
using VoiceScript.DiagramModel.Commands.LanguageFormats;

namespace VoiceScript.DiagramModel.Commands
{
    public class DeleteCommand : Command
    {
        static readonly string defaultFormat = "delete";
        public static string DefaultFormat { get => defaultFormat; }
        public DeleteCommand(string name, string targetType, string targetName, LanguageFormat languageFormat)
            : base(name, targetType, targetName, languageFormat) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                var validTargetValue = translatedTargetValue ?? targetValue;

                if (context.CurrentComponent.TryDeleteChild(translatedTargetType, validTargetValue))
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

            throw new CommandExecutionException("Component can not be deleted. It does not exist in the current context.");
        }
    }
}
