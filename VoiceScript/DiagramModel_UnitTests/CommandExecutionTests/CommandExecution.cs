using NUnit.Framework;

using DiagramModel.Commands;
using DiagramModel.Components;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class CommandExecution
    {
        protected Diagram diagram;
        protected CommandParser parser;
        protected CommandExecutionContext context;

        [SetUp]
        public void SetUp()
        {
            diagram = new Diagram();
            parser = new CommandParser();
            context = new CommandExecutionContext();
        }

        protected void InitializeCommandExecutionContext(Component target)
        {
            context.TargetComponent = target;
            context.CurrentComponent = target;
            context.CommandExecuted = false;
        }

        protected void ExecuteCommandsManually(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);
            InitializeCommandExecutionContext(diagram);

            foreach (var command in parsedCommands)
            {
                InitializeCommandExecutionContext(context.TargetComponent);
                command.Execute(context);
            }
        }
    }
}
