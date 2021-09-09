﻿using System;
using VoiceScript.DiagramModel.Commands.LanguageFormats;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands
{
    public abstract class Command
    {
        protected readonly string name, targetType, targetValue;

        protected readonly string translatedTargetType;
        protected readonly string translatedTargetValue;

        protected readonly LanguageFormat language;
        public Command(string commandName, string commandTargetType, string commandTargetValue, LanguageFormat languageFormat)
        {
            name = commandName;
            targetType = commandTargetType;
            targetValue = commandTargetValue;
            language = languageFormat;

            translatedTargetType = null;
            translatedTargetValue = null;

            if (language.ComponentNames.ContainsKey(targetType))
            {
                translatedTargetType = language.ComponentNames[targetType];
            }

            var lowerTargetValue = targetValue.ToLower();
            if (language.BoolValues.ContainsKey(lowerTargetValue))
            {
                translatedTargetValue = language.BoolValues[lowerTargetValue];
            }
        }

        public string Name { get => name; }
        public string TargetType { get => targetType; }
        public string TargetValue { get => targetValue; }

        public virtual void Execute(CommandExecutionContext context)
        {
            if (translatedTargetType == null)
                throw new InvalidOperationException($"Unsupported component type with name: {targetType}.");

            while (context.CurrentComponent != null && !context.CommandExecuted)
            {
                if (IsChildComponentTypeCompatible(context.CurrentComponent, translatedTargetType))
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
        protected bool IsChildComponentTypeCompatible(Component component, string componentChildType)
        {
            return component.ValidChildrenTypes.Contains(componentChildType);
        }

        /// <summary>
        /// Tries to execute command.
        /// Throws an exception if command can not be executed.
        /// </summary>
        /// <param name="context"></param>
        protected abstract void ProcessCommand(CommandExecutionContext context);
    }
}
