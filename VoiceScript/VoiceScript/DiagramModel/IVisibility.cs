using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    interface IVisibility : IComponent
    {
        IEnumerable<string> ValidNames { get; }
        string DefaultName { get; }
    }
}
