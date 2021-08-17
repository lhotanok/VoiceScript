using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    interface ICommand
    {
        string Name { get; }
        string TargetType { get; }
        string TargetName { get; }
    }
}
