using System;
using VoiceScript.DiagramModel.Commands.LanguageFormats;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands
{
    public class AddCommand : Command
    {
        static readonly string defaultFormat = "add";
        public static string DefaultFormat { get => defaultFormat; }

        public AddCommand(string name, string targetType, string targetName, LanguageFormat languageFormat)
            : base(name, targetType, targetName, languageFormat) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            if (ComponentFactory.CanCreateComponent(translatedTargetType))
            {
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
            else
            {
                throw new InvalidOperationException("Invalid component type given.");
            }
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
