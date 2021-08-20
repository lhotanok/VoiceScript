using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Diagram : Component
    {
        readonly List<Class> classBoxes;
        readonly static List<string> validChildTypes = new() { Class.TypeName };
        public Diagram() : base("Diagram", null, validChildTypes)
        {
            classBoxes = new List<Class>();
        }

        public IEnumerable<Class> ClassBoxes { get => classBoxes; }

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
