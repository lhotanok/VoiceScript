using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Diagram
    {
        readonly List<Class> classBoxes;

        public Diagram()
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
    }
}
