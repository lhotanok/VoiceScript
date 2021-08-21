using System;
using System.Collections.Generic;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands
{
    public class AddCommand : Command
    {
        static readonly string defaultFormat = "add";
        public static string DefaultFormat { get => defaultFormat; }
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
