using System;
using System.Collections.Generic;

using DiagramModel.Commands;

namespace DiagramModel.Components
{
    public class Diagram : Component
    {
        readonly static List<string> validChildTypes = new() { Class.TypeName };
        readonly CommandExecutionContext context;
        public Diagram(string name = "Diagram", Component parent = null) : base(name, parent, validChildTypes)
        {
            context = new CommandExecutionContext();
            InitializeCommandExecutionContext(this);
        }

        public static string TypeName { get => nameof(Diagram).ToLower(); }

        public IReadOnlyList<Class> GetClasses()
        {
            var classes = new List<Class>();

            foreach (var child in children)
            {
                classes.Add((Class)child);
            }

            return classes;
        }

        public static IList<Command> GetParsedCommands(string text, string languageCode = null)
        {
            var parser = new CommandParser(languageCode);

            return parser.GetParsedCommands(text);
        }

        public void ConvertTextToDiagram(string text, IList<Command> parsedCommands = null, string languageCode = null)
        {
            if (parsedCommands == null)
            {
                parsedCommands = GetParsedCommands(text, languageCode);
            }

            ExecuteCommands(parsedCommands);
        }

        public override string GetUniqueTypeName() => TypeName;

        public void Clear()
        {
            children.Clear();
            InitializeCommandExecutionContext(this);
        }

        void ExecuteCommands(IEnumerable<Command> commands)
        {

            var clonedDiagram = Clone();
            var currentCommandNumber = 1;

            try
            {
                foreach (var command in commands)
                {
                    InitializeCommandExecutionContext(context.TargetComponent);
                    command.Execute(context);

                    currentCommandNumber++;
                }
            }
            catch (Exception ex)
            {
                RevertChanges(clonedDiagram);
                throw new CommandExecutionException($"Error while executing {currentCommandNumber}. command.\n\n" + ex.Message,
                    currentCommandNumber);
            }
            
        }

        void InitializeCommandExecutionContext(Component target)
        {
            context.TargetComponent = target;
            context.CurrentComponent = target;
            context.CommandExecuted = false;
        }

        public override Component Clone()
        {
            var clone = new Diagram(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }

        void RevertChanges(Component original)
        {
            Name = original.Name;
            Parent = original.Parent;
            children.Clear();
            children.AddRange(original.Children);
        }
    }
}
