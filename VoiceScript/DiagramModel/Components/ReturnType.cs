﻿namespace VoiceScript.DiagramModel
{
    public class ReturnType : Type
    {
        static readonly string defName = "void";
        public ReturnType(Component parent) : this(defName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
        public override Component Clone()
        {
            var clone = new ReturnType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}