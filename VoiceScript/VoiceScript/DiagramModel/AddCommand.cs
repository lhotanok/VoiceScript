using System;

namespace VoiceScript.DiagramModel
{
    class AddCommand : Command
    {
        public AddCommand(string targetType, string targetName) : base(targetType, targetName) { }

        protected override void ProcessCommand(CommandContext context)
        {
            if (componentCtors.ContainsKey(targetType))
            {
                var childComponent = componentCtors[targetType](targetValue, context.CurrentComponent);
                context.CurrentComponent.AddChild(childComponent);
                context.TargetComponent = childComponent;
            }
            else
            {
                throw new InvalidOperationException("Invalid component type given.");
            }
        }
    }
}
