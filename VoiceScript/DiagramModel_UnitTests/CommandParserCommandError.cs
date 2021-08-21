using System;
using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests
{
    [TestFixture("class person")]
    [TestFixture("person")]
    public class CommandParser_ParseInputWithNoCommand
    {
        readonly CommandParser parser;
        readonly string input;

        public CommandParser_ParseInputWithNoCommand(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void ParseInputWithNoCommand_CheckThatExceptionIsNotThrown()
        {
            Assert.DoesNotThrow(() => parser.GetParsedCommands(input));
        }
    }


    [TestFixture("add class")]
    [TestFixture("add person")]
    [TestFixture("add")]
    public class CommandParser_TryCreateCommandFromIncompleteInput
    {
        readonly CommandParser parser;
        readonly string input;

        public CommandParser_TryCreateCommandFromIncompleteInput(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void TryParseCommand_CheckThatExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() => parser.GetParsedCommands(input));
        }
    }
}