namespace VoiceScript.DiagramModel
{
    interface IField
    {
        string Name { get; }
        AccessModifier AccessModifier { get; }
        IType Type { get; }
    }
}
