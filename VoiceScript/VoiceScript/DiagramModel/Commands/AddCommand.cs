using System;

namespace VoiceScript.DiagramModel.Commands
{
    class AddCommand : Command
    {
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
