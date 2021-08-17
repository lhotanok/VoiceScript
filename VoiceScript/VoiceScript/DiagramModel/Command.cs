using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    class Command : ICommand
    {
        public Command(string name, string targetType, string targetName)
        {
            Name = name;
            TargetType = targetType;
            TargetName = targetName;
        }
        public string Name { get; }

        public string TargetType { get; }

        public string TargetName { get; }
    }
}
