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

        [Test]
        public void ExecuteCommands_CheckThatUnsetMethodReturnTypeHasDefaultValue()
        {
            diagram.ConvertTextToDiagram("add class person add method get name");

            Assert.AreEqual(ReturnType.DefaultName, diagram.GetClasses()[0].GetMethods()[0].GetReturnType().Name);
        }

        [Test]
        public void ExecuteCommands_CheckThatUnsetFieldTypeHasDefaultValue()
        {
            diagram.ConvertTextToDiagram("add class person add field name");

            Assert.AreEqual(FieldType.DefaultName, diagram.GetClasses()[0].GetFields()[0].GetFieldType().Name);
        }

        [Test]
        public void ExecuteCommands_CheckThatUnsetParameterTypeHasDefaultValue()
        {
            diagram.ConvertTextToDiagram("add class person add method get name add parameter name");

            Assert.AreEqual(ParameterType.DefaultName, diagram.GetClasses()[0].GetMethods()[0].GetParameters()[0].GetParameterType().Name);
        }

        [Test]
        public void DeleteComponentWithDefaultValue_CheckThatDefaultValueIsProvidedAfterComponentDeletion()
        {
            diagram.ConvertTextToDiagram("add class person add method get name add type string delete type string");

            Assert.AreEqual(ReturnType.DefaultName, diagram.GetClasses()[0].GetMethods()[0].GetReturnType().Name);
        }
    }
}
