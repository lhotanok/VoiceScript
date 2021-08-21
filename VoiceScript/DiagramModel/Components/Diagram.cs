using System;
using System.Collections.Generic;

using VoiceScript.DiagramModel.Commands;

namespace VoiceScript.DiagramModel
{
    public class Diagram : Component
    {
        readonly static List<string> validChildTypes = new() { Class.TypeName };
        public Diagram(string name = "Diagram", Component parent = null) : base(name, parent, validChildTypes) { }

        public static string TypeName { get => nameof(Diagram).ToLower(); }

        public IEnumerable<Class> GetClasses()
        {
            var classes = new List<Class>();

            foreach (var child in children)
            {
                classes.Add((Class)child);
            }

            return classes;
        }

        public void ConvertTextToDiagram(string text)
        {
            var parser = new CommandParser();
            var parsedCommands = parser.GetParsedCommands(text);

            ExecuteCommands(parsedCommands);
        }

        public override string GetTypeName() => TypeName;

        void ExecuteCommands(IEnumerable<Command> commands)
        {
            var context = new CommandExecutionContext()
            {
                TargetComponent = this,
            };

            var clonedDiagram = Clone();

            try
            {
                foreach (var command in commands)
                {
                    context.CurrentComponent = context.TargetComponent;
                    context.CommandExecuted = false;

                    command.Execute(context);
                }
            }
            catch (Exception)
            {
                RevertChanges(clonedDiagram);
                throw;
            }
            
        }

        public override Component Clone()
        {
            var clone = new Diagram(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }

        void RevertChanges(Component original)
        {
            Name = original.Name;
            Parent = original.Parent;
            children.Clear();
            children.AddRange(original.Children);
        }
    }
}
