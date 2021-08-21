using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandParser
    {
        readonly static List<string> validKeywords = new() { "add", "edit", "delete", DelimiterWrapper.Command };

        string[] parsedWords;
        int parsedOffset;

        public IList<Command> GetParsedCommands(string inputText)
        {
            parsedWords = inputText.Split(' ');
            parsedOffset = 0;

            var parsedCommands = new List<Command>();
            var command = GetNextCommand();

            while (command != null)
            {
                parsedCommands.Add(command);
                command = GetNextCommand();
            }

            return parsedCommands;
        }

        Command GetNextCommand()
        {
            var commandName = GetCommandName();
            parsedOffset++;

            if (!IsKeyword(commandName)) return null;

            var targetType = GetNextWord().ToLower();
            parsedOffset++;

            var targetName = GetTargetName();

            if (IncompleteCommandTarget(targetType, targetName) || !CommandFactory.CanCreateCommand(commandName))
            {
                throw new InvalidOperationException("Invalid command.");
            }

            return CommandFactory.GetCommandCtor(commandName)(targetType, targetName);
        }

        string GetCommandName()
        {
            var word = GetNextWord();

            while (word != string.Empty && !IsKeyword(word))
            {
                parsedOffset++;
                word = GetNextWord();
            }

            return word.ToLower();
        }

        string GetTargetName()
        {
            var nameParts = new List<string>();
            var delimiter = new DelimiterWrapper();

            var word = GetNextWord();

            while (word != string.Empty)
            {
                if (IsKeyword(word))
                {
                    delimiter.HandleDelimiter(word);
                    if (!delimiter.Escape() && !delimiter.DelimiterSet) break;

                    if (delimiter.Escape()) nameParts.Add(word);
                }
                else
                {
                    nameParts.Add(word);
                }

                parsedOffset++;
                word = GetNextWord();
            }

            return ConvertToPascalCase(nameParts);
        }

        string GetNextWord() => parsedOffset < parsedWords.Length ? parsedWords[parsedOffset] : string.Empty;

        static bool IsKeyword(string word) => validKeywords.Contains(word.ToLower());

        static bool IncompleteCommandTarget(string targetType, string targetName)
            => targetType == string.Empty || targetName == string.Empty;

        static string ConvertToPascalCase(IEnumerable<string> words)
        {
            var pascalCaseWord = new StringBuilder();

            foreach (var word in words)
            {
                var lowerCaseWord = word.ToLower();
                var firstLetter = lowerCaseWord[0].ToString().ToUpper();
                pascalCaseWord.Append(firstLetter + lowerCaseWord[1..]);
            }

            return pascalCaseWord.ToString();
        }
    }
}
