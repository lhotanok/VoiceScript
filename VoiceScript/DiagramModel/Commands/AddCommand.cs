using DiagramModel.Commands.LanguageFormats;
using DiagramModel.Components;

namespace DiagramModel.Commands
{
    public class AddCommand : Command
    {
        static readonly string defaultFormat = "add";
        public static string DefaultFormat { get => defaultFormat; }

        public AddCommand(string name, string targetType, string targetName, LanguageFormat languageFormat)
            : base(name, targetType, targetName, languageFormat) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            if (!ComponentFactory.CanCreateComponent(translatedTargetType))
            {
                throw new CommandExecutionException("Invalid component type given.\n" +
                    $"Original type: {targetType}.\nDerived type: {translatedTargetType}");
            }
            
            var validTargetValue = translatedTargetValue ?? targetValue;

            var sameNameChild = GetChildWithSameName(validTargetValue, context.CurrentComponent);

            if (sameNameChild == null)
            {
                var childComponent = ComponentFactory.CreateComponent(translatedTargetType, validTargetValue, context.CurrentComponent);

                context.CurrentComponent.AddChild(childComponent);
                context.TargetComponent = childComponent;
            }
            else
            {
                context.TargetComponent = sameNameChild;
            }

            context.CommandExecuted = true;
        }

        static Component GetChildWithSameName(string childName, Component parent)
        {
            var lowerChildName = childName.ToLower();

            foreach (var child in parent.Children)
            {
                if (child.Name.ToLower() == lowerChildName) return child;
            }

            return null;
        }

    }
}
