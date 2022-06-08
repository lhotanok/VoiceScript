using DiagramModel.Commands.LanguageFormats;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Representation of command for component deletion.
    /// </summary>
    public class DeleteCommand : Command
    {
        static readonly string defaultFormat = "delete";
        public static string DefaultFormat { get => defaultFormat; }
        public DeleteCommand(string name, string targetType, string targetName, LanguageFormat languageFormat)
            : base(name, targetType, targetName, languageFormat) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            var validTargetValue = translatedTargetValue ?? targetValue;

            if (context.CurrentComponent.TryDeleteChild(translatedTargetType, validTargetValue))
            {
                context.TargetComponent = context.CurrentComponent;
                context.CommandExecuted = true;
            }
            else
            {
                context.CurrentComponent = context.CurrentComponent.Parent;
            }
        }
    }
}
