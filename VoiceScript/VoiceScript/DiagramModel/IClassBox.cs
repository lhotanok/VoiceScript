using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    interface IClassBox : IComponent
    {
        IEnumerable<IField> Fields { get; }
        IEnumerable<IMethod> Methods { get; }
    }
}
