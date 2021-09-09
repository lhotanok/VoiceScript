using System;
using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandParserTests
{
    public class ParsedInputWithNoCommand
    {
        readonly CommandParser parser;

        public ParsedInputWithNoCommand()
        {
            parser = new CommandParser();
        }

        [TestCase("class person")]
        [TestCase("person")]
        [TestCase("")]
        [TestCase("\n")]
        [TestCase("    \n   \n\n\n")]
        public void ParseInputWithNoCommand_CheckThatExceptionIsNotThrown(string inputText)
        {
            Assert.DoesNotThrow(() => parser.GetParsedCommands(inputText));
        }
    }

    public class UnsuccessfulCommandParsingFromIncompleteInput
    {
        CommandParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new CommandParser();
        }

        [TestCase("add class")]
        [TestCase("add person")]
        [TestCase("add")]
        public void TryParseCommand_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<CommandParseException>(() => parser.GetParsedCommands(inputText));
        }
    }
}