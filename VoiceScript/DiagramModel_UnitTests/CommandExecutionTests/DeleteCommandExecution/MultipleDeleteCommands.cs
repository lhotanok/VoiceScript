using System;
using NUnit.Framework;
using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class MultipleDeleteCommands : CommandExecution
    {
        [TestCase("add class person add class employee add class student delete class employee delete class student")]
        [TestCase("add class person edit class person add class student delete class student")]
        public void ExecuteDeleteCommands_CheckThatClassesWereDeleted(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(1, diagram.GetClasses().Count);
        }

        [TestCase("add class person add field name add method get name delete field name delete method get name")]
        [TestCase("add class person add field name add type string add method get name add type string delete field name delete method get name")]
        public void ExecuteDeleteCommands_CheckThatParentContextRemained(string inputText)
        {
            ExecuteCommandsManually(inputText);

            Assert.AreEqual(context.TargetComponent, diagram.GetClasses()[0]);
        }

        [TestCase("add class person add field name add type string add method get name add visibility public delete field name delete visibility public")]
        public void TryExecuteDeleteCommandsWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<CommandExecutionException>(() => diagram.ConvertTextToDiagram(inputText));
        }
    }
}
