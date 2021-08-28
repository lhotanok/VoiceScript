using System;
using System.Text;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandParser
    {
        readonly static List<string> validKeywords = new() {
            AddCommand.DefaultFormat,
            EditCommand.DefaultFormat,
            DeleteCommand.DefaultFormat,
            DelimiterWrapper.CommandDefaultFormat
        };

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

            if (word.Contains('\n')) word = GetWordAfterLineFeed(word);

            while (word != string.Empty && !IsKeyword(word))
            {
                parsedOffset++;
                word = GetNextWord();
                if (word.Contains('\n')) word = GetWordAfterLineFeed(word);
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
                if (word.Contains('\n'))
                {
                    word = GetWordBeforeLineFeed(word);
                    if (word != string.Empty) nameParts.Add(word);
                    break;
                }
                if (IsKeyword(word))
                {
                    delimiter.UpdateDelimiterContext(word);
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

        string GetWordBeforeLineFeed(string word)
        {
            var substring = new StringBuilder();

            int index = 0;
            while (index < word.Length && word[index] != '\n')
            {
                substring.Append(word[index]);
                index++;
            }

            return substring.ToString();
        }
        string GetWordAfterLineFeed(string word)
        {
            var wordFragments = word.Split('\n');
            var substring = string.Empty;

            for (int i = wordFragments.Length - 1; i >= 0; i--)
            {
                if (wordFragments[i] != string.Empty)
                {
                    substring = wordFragments[i];
                    break;
                }
            }

            return substring;
        }
    }
}
