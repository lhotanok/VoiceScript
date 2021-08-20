namespace VoiceScript.DiagramModel.Commands
{
    class CommandExecutionContext
    {
        public Component CurrentComponent { get; set; }
        public Component TargetComponent { get; set; }
        public bool CommandExecuted { get; set; }
    }
}
