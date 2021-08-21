using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandExecutionContext
    {
        public Component CurrentComponent { get; set; }
        public Component TargetComponent { get; set; }
        public bool CommandExecuted { get; set; }
    }
}
