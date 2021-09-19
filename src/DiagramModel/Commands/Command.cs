using DiagramModel.Commands.LanguageFormats;
using DiagramModel.Components;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Representation of parsed command.
    /// </summary>
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
        }

        /// <summary>
        /// Command identification name.
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// Name of the target component type a command is associated with.
        /// For possible values, see <see cref="LanguageFormat.ComponentNames"/>
        /// </summary>
        public string TargetType { get => targetType; }

        /// <summary>
        /// Value representing target component's <see cref="Component.Name"/>.
        /// </summary>
        public string TargetValue { get => targetValue; }

        /// <summary>
        /// Executes command with respect to the provided execution context.
        /// </summary>
        /// <param name="context">Context for command execution. Gets modified 
        /// if command is executed successfully to provide a correct context 
        /// for subsequent command execution.</param>
        /// <exception cref="CommandExecutionException">Thrown if command can not
        /// be executed in the current context.</exception>
        public virtual void Execute(CommandExecutionContext context)
        {
            if (translatedTargetType == null)
                throw new CommandExecutionException($"Unsupported component type: {targetType}.");

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
                throw new CommandExecutionException("Command can not be executed in the current context.");
        }
        protected static bool IsChildComponentTypeCompatible(Component component, string componentChildType)
        {
            return component.ValidChildrenTypes.Contains(componentChildType);
        }

        /// <summary>
        /// Tries to execute command.
        /// Throws an exception if command can not be executed.
        /// </summary>
        /// <param name="context">Context for command execution. Gets modified 
        /// if command is executed successfully to provide a correct context 
        /// for subsequent command execution.</param>
        /// <exception cref="CommandExecutionException">Thrown if command can not
        /// be executed in the current context.</exception>
        protected abstract void ProcessCommand(CommandExecutionContext context);
    }
}
