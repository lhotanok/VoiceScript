using NUnit.Framework;
using DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class SingleAddCommand : CommandExecution
    {
        [TestCase("add class person")]
        [TestCase("Add class person")]
        public void ExecuteAddClassCommandManually_CheckThatDiagramHasExactlyOneClass(string inputText)
        {
            ExecuteCommandsManually(inputText);

            Assert.AreEqual(1, diagram.Children.Count);
        }

        [TestCase("add class person")]
        [TestCase("Add class person")]
        public void ExecuteAddClassCommandUsingDiagramAPI_CheckThatDiagramHasExactlyOneClass(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(1, diagram.Children.Count);
        }

        [TestCase("add field name")]
        [TestCase("Add parameter age")]
        [TestCase("Add diagram my diagram")]
        public void TryExecuteAddCommandManuallyWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);
            InitializeCommandExecutionContext(diagram);

            Assert.Throws<CommandExecutionException>(() => parsedCommands[0].Execute(context));
        }

        [TestCase("add field name")]
        [TestCase("Add parameter age")]
        [TestCase("Add diagram my diagram")]
        public void TryExecuteAddCommandUsingDiagramAPIWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<CommandExecutionException>(() => diagram.ConvertTextToDiagram(inputText));
        }
    }
}
