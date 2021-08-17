using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    class Parameter : IParameter
    {
        public Parameter(string name)
        {
            Name = name;
            Type = new ParameterType("object");
            Required = false;
            ContainsComponents = true;
        }

        public IType Type {get; private set;}

        public bool Required { get; private set; }

        public string Name { get; private set; }

        public bool ContainsComponents { get; }
    }
}
