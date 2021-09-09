using System;
using System.Collections.Generic;
using System.Drawing;
using VoiceScript.DiagramModel.Commands;

namespace VoiceScript.CommandDesign
{
    class CommandDesigner
    {
        readonly Action<string, Color> textCallback;

        public CommandDesigner(Action<string, Color> writeTextCallback)
        {
            textCallback = writeTextCallback;
        }

        public void DesignCommands(IList<Command> commands)
        {
            foreach (var command in commands)
            {
                DesignCommand(command);
            }
        }

        public void DesignCommand(Command command)
        {
            textCallback(command.Name + " ", CommandColor.NameColor);
            textCallback(command.TargetType + " ", CommandColor.TargetTypeColor);
            textCallback(command.TargetValue + Environment.NewLine, CommandColor.TargetValueColor);
        }
    }
}
