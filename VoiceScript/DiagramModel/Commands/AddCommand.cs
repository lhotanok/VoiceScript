﻿using System;
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
                var sameNameChild = GetChildWithSameName(targetValue, context.CurrentComponent);

                if (sameNameChild == null)
                {
                    var childComponentCtor = ComponentFactory.GetComponentCtor(targetType);
                    var childComponent = childComponentCtor(targetValue, context.CurrentComponent);

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
            foreach (var child in parent.Children)
            {
                if (child.Name == childName) return child;
            }

            return null;
        }
    }
}
