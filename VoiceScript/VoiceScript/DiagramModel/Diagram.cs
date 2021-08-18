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
            foreach (var command in commands)
            {
                command.Execute(new CommandContext()
                {
                    CurrentComponent = this,
                    TargetComponent = this
                });
            }
        }
    }
}
