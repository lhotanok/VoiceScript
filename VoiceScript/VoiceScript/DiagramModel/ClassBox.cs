using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class ClassBox : IClassBox
    {
        List<IField> fields;
        List<IMethod> methods;

        public ClassBox(string name)
        {
            Name = name;
            ContainsComponents = true;

            fields = new List<IField>();
            methods = new List<IMethod>();
        }

        public IEnumerable<IField> Fields => fields;

        public IEnumerable<IMethod> Methods => methods;

        public string Name { get; private set; }

        public bool ContainsComponents { get; }
    }
}
