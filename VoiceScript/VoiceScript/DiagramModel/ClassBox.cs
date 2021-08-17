using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    class ClassBox : Component, IClassBox
    {
        public ClassBox(string name) : base(name) { }
        public IEnumerable<IField> Fields => throw new NotImplementedException();

        public IEnumerable<IMethod> Methods => throw new NotImplementedException();
    }
}
