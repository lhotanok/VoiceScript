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
            context = new CommandExecutionContext(diagram);
        }

        protected void ExecuteCommandsManually(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);

            foreach (var command in parsedCommands)
            {
                context.Initialize();
                command.Execute(context);
            }
        }
    }
}
