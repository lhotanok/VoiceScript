namespace VoiceScript.DiagramModel
{
    interface IComponent
    {
        string Name { get; }
        bool ContainsComponents { get; }
    }
}
