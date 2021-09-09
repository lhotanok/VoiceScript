using System;
using NUnit.Framework;
using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    public class IndividualAddComponentChecker : CommandExecution
    {
        [Test]
        public void AddClass_CheckThatDiagramHasOneClass()
        {
            ExecuteCommands("add class car");

            Assert.AreEqual(1, diagram.GetClasses().Count);
        }

        [Test]
        public void AddClass_CheckThatDiagramHasOneClassWithCorrectName()
        {
            ExecuteCommands("add class car");

            Assert.AreEqual("Car", diagram.GetClasses()[0].Name);
        }

        [Test]
        public void AddField_CheckThatClassHasOneField()
        {
            ExecuteCommands("add class car add field color");

            Assert.AreEqual(1, diagram.GetClasses()[0].GetFields().Count);
        }

        [Test]
        public void AddField_CheckThatClassHasOneFieldWithCorrectName()
        {
            ExecuteCommands("add class car add field color");

            Assert.AreEqual("Color", diagram.GetClasses()[0].GetFields()[0].Name);
        }

        [Test]
        public void AddMethod_CheckThatClassHasOneMethod()
        {
            ExecuteCommands("add class car add method get color");

            Assert.AreEqual(1, diagram.GetClasses()[0].GetMethods().Count);
        }

        [Test]
        public void AddMethod_CheckThatClassHasOneMethodWithCorrectName()
        {
            ExecuteCommands("add class car add method get color");

            Assert.AreEqual("GetColor", diagram.GetClasses()[0].GetMethods()[0].Name);
        }

        [Test]
        public void AddFieldType_CheckThatFieldHasFieldTypeWithCorrectName()
        {
            ExecuteCommands("add class car add field color add type string");

            Assert.AreEqual("String", diagram.GetClasses()[0].GetFields()[0].GetFieldType().Name);
        }

        [Test]
        public void AddReturnType_CheckThatMethodHasReturnTypeWithCorrectName()
        {
            ExecuteCommands("add class car add method get color add type string");

            Assert.AreEqual("String", diagram.GetClasses()[0].GetMethods()[0].GetReturnType().Name);
        }

        [Test]
        public void AddParameter_CheckThatMethodHasParameterWithCorrectName()
        {
            ExecuteCommands("add class car add method set color add parameter my color");

            Assert.AreEqual("myColor", diagram.GetClasses()[0].GetMethods()[0].GetParameters()[0].Name);
        }

        [Test]
        public void AddParameterType_CheckThatParameterHasParameterTypeWithCorrectName()
        {
            ExecuteCommands("add class car add method set color add parameter color add type string");

            Assert.AreEqual("String", diagram.GetClasses()[0].GetMethods()[0].GetParameters()[0].GetParameterType().Name);
        }

        [Test]
        public void AddRequiredParameter_CheckThatParameterIsRequired()
        {
            ExecuteCommands("add class car add method set color add parameter color add required true");

            Assert.AreEqual(true, diagram.GetClasses()[0].GetMethods()[0].GetParameters()[0].IsRequired);
        }

        [Test]
        public void AddVisibility_CheckThatFieldHasVisibilityWithCorrectName()
        {
            ExecuteCommands("add class car add field color add visibility protected");

            Assert.AreEqual("protected", diagram.GetClasses()[0].GetFields()[0].GetVisibility().Name);
        }

        [Test]
        public void AddVisibility_CheckThatMethodHasVisibilityWithCorrectName()
        {
            ExecuteCommands("add class car add method get color add visibility public");

            Assert.AreEqual("public", diagram.GetClasses()[0].GetMethods()[0].GetVisibility().Name);
        }

        [Test]
        public void TryAddVisibilityWithInvalidName_CheckThatExceptionIsThrown()
        {
            Assert.Throws<CommandExecutionException>(
                () => ExecuteCommands("add class car add method get color add visibility specific"));
        }

        [Test]
        public void ExecuteAddUndefinedCommand_CheckThatExceptionIsThrownIfTargetTypeIsUnknown()
        {
            Assert.Throws<CommandExecutionException>(() => ExecuteCommands("add undefined car"));
        }

        void ExecuteCommands(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);
        }
    }
}
