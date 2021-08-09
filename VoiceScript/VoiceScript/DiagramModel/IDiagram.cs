using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    interface IDiagram
    {
        string Name { get; }
        int Count { get; }
        IEnumerable<IField> Fields { get; }
        IEnumerable<IMethod> Methods { get; }
    }
}
