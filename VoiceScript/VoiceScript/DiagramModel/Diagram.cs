using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Diagram : Component
    {
        readonly List<Class> classBoxes;
        public static List<string> ValidChildTypes = new() { Class.TypeName };
        public Diagram() : base("Diagram", null, ValidChildTypes)
        {
            classBoxes = new List<Class>();
        }

        public IEnumerable<Class> ClassBoxes { get => classBoxes; }

        public void ConvertTextToDiagram(string text)
        {
            var parser = new CommandParser(text);
            var parsedCommands = parser.GetParsedCommands();

            // process command tree
        }

        public override string GetTypeName() => nameof(Diagram).ToLower();
    }
}
