using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Investor.SearchTools
{
    public class FilterSearchInputString : StringReader
    {
        public List<char> Seperators { get; protected set; }

        protected int SearchTermIndex { get; set; }
        protected string FullText { get; set; }
        protected int ReaderIndex { get; set; }
        protected int ReaderCharsToCompliteWord { get; set; }

        public FilterSearchInputString(string value)
            : base(value)
        {
            Seperators = new List<char>();
            
            // add NewLine
            Seperators.AddRange(Environment.NewLine.ToCharArray());
            FullText = value;
            ReaderIndex = 0;
        }

        protected bool WordIsCorrect(string searchTerm, string possibleMatching)
        {
            var possibleMatchingCount = possibleMatching.Count();

            var startIndexOfWord = ReaderIndex - possibleMatchingCount;

            // We check if we can look at the previus character or if this is a maching at the
            // start of the reader.
            if (startIndexOfWord < 1)
            {
                return true;
            }

            var getPreviusCharacter = FullText.Substring(startIndexOfWord - 1, 1).First();

            if (Char.IsWhiteSpace(getPreviusCharacter) || Char.IsPunctuation(getPreviusCharacter) || Seperators.Contains(getPreviusCharacter))
            {
                return true;
            }

            return false;
        }

        protected bool IsEndOfWord(char c, char? currentSearchTermChar)
        {
            if (currentSearchTermChar != null && Char.IsWhiteSpace(currentSearchTermChar.Value))
            {
                return false;
            }

            return Char.IsWhiteSpace(c) || Char.IsPunctuation(c) || Seperators.Contains(c);
        }

        public IList<string> FilterMatches(string searchTerm)
        {
            var matches = new List<string>();
            var searchTermsChars = searchTerm.ToLower().ToCharArray();
            var charTermIndex = 0;
            var possibleMatch = string.Empty;
            var readToNextWord = false;
            var getFullWord = false;

            // Reset function that will change temp values back to init
            Action resetTemp = delegate
            {
                charTermIndex = 0;
                possibleMatch = string.Empty;
                getFullWord = false;
                ReaderCharsToCompliteWord = 0;
            };

            while (Peek() >= 0)
            {
                var thisCharacter = Char.ToLower((char)Read());
                char? searchTermChar = null;

                if (charTermIndex < searchTermsChars.Length)
                {
                    searchTermChar = searchTermsChars[charTermIndex];
                }
                

                if (readToNextWord)
                {
                    if (IsEndOfWord(thisCharacter, searchTermChar))
                    {
                        // We are not at a new word
                        ReaderIndex++;
                        continue;
                    }

                    // We are at a new word
                    readToNextWord = false;
                }

                
                // Check if we are at an end
                if (IsEndOfWord(thisCharacter, searchTermChar))
                {
                    // Check if we have all matchings
                    if (charTermIndex == searchTermsChars.Length)
                    {
                        if (possibleMatch.Length >= 2)
                        {
                            // We have a complite matching
                            if (WordIsCorrect(searchTerm, possibleMatch))
                            {
                                matches.Add(possibleMatch);
                            }
                        }
                    }

                    // Reset the temp for a new possible matching
                    resetTemp();
                    readToNextWord = true;

                    ReaderIndex++;

                    continue;
                }

                if (getFullWord)
                {
                    possibleMatch += thisCharacter;

                    // Check if we are at the end of the text
                    if (Peek() == -1)
                    {
                        if (possibleMatch.Length >= 2)
                        {
                            // We have a complite matching
                            // We need to check if the word is correct

                            if (WordIsCorrect(searchTerm, possibleMatch))
                            {
                                matches.Add(possibleMatch);
                            }
                            
                            resetTemp();
                        }
                    }

                    
                    ReaderIndex++;

                    continue;
                }

                // are we finished to get the whole word ?
                if ((searchTermsChars.Length - 1) == charTermIndex)
                {
                    getFullWord = true;
                }

                // Check if we have a possible found match
                if (thisCharacter.Equals(searchTermsChars[charTermIndex]))
                {
                    // Check if we are at the end of the term
                    if ((searchTermsChars.Length - 1) >= charTermIndex)
                    {
                        charTermIndex++;

                        possibleMatch += thisCharacter;
                    }
                }
                else
                {
                    // Matching failed... reset and read to start of next word
                    resetTemp();
                }

                ReaderIndex++;
            }
            
            return matches;
        }
    }
}
