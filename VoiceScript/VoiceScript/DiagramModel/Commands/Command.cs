using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    abstract class Command
    {
        
        public Command(string targetType, string targetName)
        {
            this.targetType = targetType;
            this.targetValue = targetName;
        }

        protected readonly string targetType, targetValue;

        public void Execute(CommandExecutionContext context)
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
        }
        static bool IsChildComponentTypeCompatible(Component component, string componentChildType)
            => component.ValidChildrenTypes.Contains(componentChildType);

        /// <summary>
        /// Tries to execute command.
        /// Throws an exception if command can not be executed.
        /// </summary>
        /// <param name="context"></param>
        protected abstract void ProcessCommand(CommandExecutionContext context);
    }
}
