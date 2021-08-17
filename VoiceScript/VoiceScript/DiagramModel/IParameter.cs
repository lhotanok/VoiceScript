namespace VoiceScript.DiagramModel
{
    interface IParameter : IComponent
    {
        IType Type { get; }
        bool Required { get; }
    }
}
