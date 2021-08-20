using System;

namespace VoiceScript.DiagramModel
{
    class EditCommand : Command
    {
        public EditCommand(string targetType, string targetName) : base(targetType, targetName) { }

        protected override void ProcessCommand(CommandExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
