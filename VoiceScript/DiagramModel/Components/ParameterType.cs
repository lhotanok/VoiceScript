﻿namespace VoiceScript.DiagramModel
{
    public class ParameterType : Type
    {
        static readonly string defName = "object";
        public ParameterType(Component parent) : this(defName, parent) { }
        public ParameterType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
        public override Component Clone()
        {
            var clone = new ParameterType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}