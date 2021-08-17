using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    interface IMethod : IComponent
    {
        IVisibility Visibility { get; }
        IType ReturnType { get; }
        IEnumerable<IParameter> RequiredParameters { get; }
        IEnumerable<IParameter> OptionalParameters { get; }
    }
}
