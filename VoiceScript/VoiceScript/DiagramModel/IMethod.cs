using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    interface IMethod
    {
        string Name { get; }
        AccessModifier AccessModifier { get; }
        IType ReturnType { get; }
        IEnumerable<IType> RequiredParameters { get; }
        IEnumerable<IType> OptionalParameters { get; }
    }
}
