using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;
using VoiceScript.DiagramModel.Commands.LanguageFormats;

namespace DiagramModel_UnitTests.CommandParserTests
{
    [TestFixture("attach class person attach field name attach method get name")]
    [TestFixture("attach class person annex field name insert method get name")]
    [TestFixture("attach class person add field name insert method get name")]
    public class EnglishAddCommands
    {
        readonly CommandParser parser;
        readonly string input;

        public EnglishAddCommands(string inputText)
        {
            parser = new CommandParser(EnglishFormat.GetCode());
            input = inputText;
        }

        [Test]
        public void ParseAditionCommands_ParseExactlyThreeCommands()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(3, parsedCommands.Count);
        }

        [Test]
        public void ParseAditionCommands_CheckIfEachParsedCommandIsAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            foreach (var command in parsedCommands)
            {
                Assert.IsTrue(command is AddCommand);
            }
        }
    }

    [TestFixture("add class person add field name change class person modify field name")]
    [TestFixture("add class person add field name correct class person edit field name")]
    public class EnglishEditCommands
    {
        readonly CommandParser parser;
        readonly string input;

        public EnglishEditCommands(string inputText)
        {
            parser = new CommandParser(EnglishFormat.GetCode());
            input = inputText;
        }

        [Test]
        public void ParseCommands_ParseExactlyFourCommands()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(4, parsedCommands.Count);
        }

        [Test]
        public void ParseCommands_CheckIfLastCommandsAreEditCommands()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.IsTrue(parsedCommands[2] is EditCommand && parsedCommands[3] is EditCommand);
        }
    }

    [TestFixture("add class person add field name remove field name cut class person")]
    [TestFixture("add class person add field name change class person erase field name")]
    public class EnglishDeleteCommands
    {
        readonly CommandParser parser;
        readonly string input;

        public EnglishDeleteCommands(string inputText)
        {
            parser = new CommandParser(EnglishFormat.GetCode());
            input = inputText;
        }

        [Test]
        public void ParseCommands_ParseExactlyFourCommands()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(4, parsedCommands.Count);
        }

        [Test]
        public void ParseCommands_CheckIfLastCommandIsDeleteCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.IsTrue(parsedCommands[3] is DeleteCommand);
        }
    }
}