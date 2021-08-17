namespace VoiceScript.DiagramModel
{
    interface ICommand
    {
        string Name { get; }
        string TargetType { get; }
        string TargetValue { get; }
    }
}
