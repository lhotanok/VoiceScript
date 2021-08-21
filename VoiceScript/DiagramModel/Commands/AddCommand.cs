using System;

using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands
{
    public class AddCommand : Command
    {
        static readonly string format = "add";
        public static string Format { get => format; }
        public AddCommand(string targetType, string targetName) : base(targetType, targetName) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            if (ComponentFactory.CanCreateComponent(targetType))
            {
                var childComponentCtor = ComponentFactory.GetComponentCtor(targetType);
                var childComponent = childComponentCtor(targetValue, context.CurrentComponent);

                context.CurrentComponent.AddChild(childComponent);
                context.TargetComponent = childComponent;
                context.CommandExecuted = true;
            }
            else
            {
                throw new InvalidOperationException("Invalid component type given.");
            }
        }
    }
}
