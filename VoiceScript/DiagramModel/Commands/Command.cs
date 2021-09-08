using System;

using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands
{
    public abstract class Command
    {
        protected readonly string name, targetType, targetValue;

        public Command(string commandName, string commandTargetType, string commandTargetValue)
        {
            name = commandName;
            targetType = commandTargetType;
            targetValue = commandTargetValue;
        }

        public string Name { get => name; }
        public string TargetType { get => targetType; }
        public string TargetValue { get => targetValue; }

        public virtual void Execute(CommandExecutionContext context)
        {
            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                if (IsChildComponentTypeCompatible(context.CurrentComponent, targetType))
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
        protected static bool IsChildComponentTypeCompatible(Component component, string componentChildType)
            => component.ValidChildrenTypes.Contains(componentChildType);

        /// <summary>
        /// Tries to execute command.
        /// Throws an exception if command can not be executed.
        /// </summary>
        /// <param name="context"></param>
        protected abstract void ProcessCommand(CommandExecutionContext context);
    }
}
