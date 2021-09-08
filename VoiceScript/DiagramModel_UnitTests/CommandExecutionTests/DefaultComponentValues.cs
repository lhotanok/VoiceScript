using System;
using NUnit.Framework;
using VoiceScript.DiagramModel.Components;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    class DefaultComponentValues : CommandExecution
    {
        [Test]
        public void ExecuteCommands_CheckThatUnsetFieldVisibilityHasDefaultValue()
        {
            diagram.ConvertTextToDiagram("add class person add field name");

            Assert.AreEqual(Visibility.DefaultName, diagram.GetClasses()[0].GetFields()[0].GetVisibility().Name);
        }

        [Test]
        public void ExecuteCommands_CheckThatUnsetMethodVisibilityHasDefaultValue()
        {
            diagram.ConvertTextToDiagram("add class person add method get name");

            Assert.AreEqual(Visibility.DefaultName, diagram.GetClasses()[0].GetMethods()[0].GetVisibility().Name);
        }
    }
}
