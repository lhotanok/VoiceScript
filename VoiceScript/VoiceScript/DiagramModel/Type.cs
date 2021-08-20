using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    abstract class Type : Component
    {
        readonly static List<string> validChildTypes = new();
        public static string TypeName { get => "type"; }

        public Type(string name, Component parent) : base(name, parent, validChildTypes) { }

        public abstract string GetDefaultName();
        public override string GetTypeName() => TypeName;
    }
}
