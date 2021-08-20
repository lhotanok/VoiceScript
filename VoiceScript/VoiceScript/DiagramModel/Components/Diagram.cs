using System;
using System.Collections.Generic;

using VoiceScript.DiagramModel.Commands;

namespace VoiceScript.DiagramModel
{
    class Diagram : Component
    {
        readonly static List<string> validChildTypes = new() { Class.TypeName };
        public Diagram(string name = "Diagram", Component parent = null) : base(name, parent, validChildTypes) { }

        public static string TypeName { get => nameof(Diagram).ToLower(); }

        public IEnumerable<Class> GetClasses()
        {
            var classes = new List<Class>();

            foreach (var child in children)
            {
                classes.Add((Class)child);
            }

            return classes;
        }

        public void ConvertTextToDiagram(string text)
        {
            var parser = new CommandParser(text);
            var parsedCommands = parser.GetParsedCommands();

            ExecuteCommands(parsedCommands);
        }

        public override string GetTypeName() => nameof(Diagram).ToLower();

        void ExecuteCommands(IEnumerable<Command> commands)
        {
            var context = new CommandExecutionContext()
            {
                TargetComponent = this,
            };

            foreach (var command in commands)
            {
                context.CurrentComponent = context.TargetComponent;
                context.CommandExecuted = false;
                command.Execute(context);
            }
        }
    }
}
