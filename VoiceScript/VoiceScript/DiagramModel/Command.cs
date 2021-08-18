using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    abstract class Command
    {
        protected static readonly Dictionary<string, Func<string, Component, Component>> componentCtors = new()
        {
            { Class.TypeName, (childName, parent) => new Class(childName, parent) },
            { Field.TypeName, (childName, parent) => new Field(childName, parent) },
            { Method.TypeName, (childName, parent) => new Method(childName, parent) },
            { Parameter.TypeName, (childName, parent) => new Parameter(childName, parent) },
            { Required.TypeName, (childName, parent) => new Required(parent) },
            { VariableType.TypeName, (childName, parent) => new VariableType(childName, parent) },
            { Visibility.TypeName, (childName, parent) => new Visibility(childName, parent) }
        };
        public Command(string targetType, string targetName)
        {
            this.targetType = targetType;
            this.targetValue = targetName;
        }

        protected readonly string targetType, targetValue;

        public void Execute(CommandContext context)
        {
            int counter = 0;
            while (context.CurrentComponent != null)
            {
                if (IsChildComponentTypeCompatible(context.CurrentComponent, targetType))
                {
                    counter++;
                    Console.WriteLine($"Bug tracer says: {counter}, {context.CurrentComponent.Name}");
                    ProcessCommand(context);
                }
                else
                {
                    counter++;
                    Console.WriteLine($"Bug tracer says: {counter}, {context.CurrentComponent.Name}");
                    context.CurrentComponent = context.CurrentComponent.Parent;
                }
            }
        }
        static bool IsChildComponentTypeCompatible(Component component, string componentChildType)
            => component.ValidChildrenTypes.Contains(componentChildType);
        protected abstract void ProcessCommand(CommandContext context);
    }
}
