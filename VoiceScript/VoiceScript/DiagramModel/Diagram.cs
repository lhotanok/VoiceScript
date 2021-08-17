using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Diagram
    {
        List<IClassBox> classBoxes;

        public Diagram()
        {
            classBoxes = new List<IClassBox>();
        }

        public IEnumerable<IClassBox> ClassBoxes { get => classBoxes; }

        public void ConvertTextToDiagram(string text)
        {
            var parser = new CommandParser(text);
            var parsedCommands = parser.GetParsedCommands();

            // process command tree
        }
    }
}
