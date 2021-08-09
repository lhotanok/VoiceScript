namespace VoiceScript.DiagramModel
{
    interface IParameter
    {
        string Name { get; }
        IType Type { get; }
        bool Required { get; }
    }
}
