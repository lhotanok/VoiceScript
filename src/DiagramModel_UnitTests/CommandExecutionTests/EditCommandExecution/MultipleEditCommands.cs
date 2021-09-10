using NUnit.Framework;
using DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class MultipleEditCommands : CommandExecution
    {
        [TestCase("add class person add field name add visibility private edit class person edit field name edit visibility private")]
        [TestCase("add class person add field name add visibility private edit field name edit visibility private")]
        [TestCase("add class person add field name add visibility private edit visibility private")]
        public void ExecuteEditCommands_CheckThatVisibilityContextWasEntered(string inputText)
        {
            ExecuteCommandsManually(inputText);

            Assert.AreEqual(context.TargetComponent, diagram.GetClasses()[0].GetFields()[0].GetVisibility());
        }

        [TestCase("add class person add field name add type string add method get name add type string edit method get name edit type string")]
        [TestCase("add class person add field name add type string add method get name add type string edit class person edit method get name edit type string")]
        public void ExecuteEditCommands_CheckThatMethodReturnTypeContextWasEntered(string inputText)
        {
            ExecuteCommandsManually(inputText);

            Assert.AreEqual(context.TargetComponent, diagram.GetClasses()[0].GetMethods()[0].GetReturnType());
        }


        [TestCase("add class person add field name add type string edit class person edit field name edit type string edit name name type")]
        [TestCase("add class person add field name add type string edit field name edit type string edit name name type")]
        [TestCase("add class person add field name add type string edit type string edit name name type")]
        public void ExecuteEditCommandsEndingWithEditNameCommand_CheckThatNameHasChanged(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual("NameType", diagram.GetClasses()[0].GetFields()[0].GetFieldType().Name);
        }

        [TestCase("add class person add field name add type string add method get name add visibility public edit field name edit visibility public")]
        public void TryExecuteEditCommandsWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<CommandExecutionException>(() => diagram.ConvertTextToDiagram(inputText));
        }
    }
}
