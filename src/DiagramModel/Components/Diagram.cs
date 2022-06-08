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
            context = new CommandExecutionContext(this);
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

        public void ConvertTextToDiagram(string text, ICommand command = null, string languageCode = null)
        {
            if (command == null)
            {
                command = new MacroCommand(GetParsedCommands(text, languageCode));
            }

            command.Execute(context);
        }

        public override string UniqueTypeName { get => TypeName; }

        public void Clear()
        {
            children.Clear();
            context.Initialize(this);
        }

        public override Component Clone()
        {
            var clone = new Diagram(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
