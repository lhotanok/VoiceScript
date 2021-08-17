namespace VoiceScript.DiagramModel
{
    interface IField : IComponent
    {
        IVisibility Visibility { get; }
        IType Type { get; }
    }
}
