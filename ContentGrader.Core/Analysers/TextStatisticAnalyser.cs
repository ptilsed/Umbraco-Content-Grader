using ContentGrader.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContentGrader.Core.Analysers
{
    public class TextStatisticAnalyser
    {
        int avarageReadingWordsPerMinute = 225; // from readability-score.com
        int avarageSpeakingWordsPerMinute = 125; // from readability-score.com
        public static TextStatisticAnalyser Parse(string text)
        {
            var cleanText = CleanText(text);
            return new TextStatisticAnalyser(cleanText);
        }

        private static readonly string[] FullStopTags = new[] { "li", "p", "h1", "h2", "h3", "h4", "h5", "h6", "dd" };


        public static readonly List<FleschKincaidLevel> FleschKincaidLevels = new List<FleschKincaidLevel>()
        {
            new FleschKincaidLevel()
            {
                LowerBound = 90, UpperBound = 999, SchoolLevel = "5th Grade", Readability = "Very easy to read. Easily understood by an average 11-year-old student."
            },
            new FleschKincaidLevel()
            {
                LowerBound = 80, UpperBound = 90, SchoolLevel = "6th Grade", Readability = "Easy to read. Conversational English for consumers."
            },
            new FleschKincaidLevel()
            {
                LowerBound = 70, UpperBound = 80, SchoolLevel = "7th Grade", Readability = "Fairly easy to read for most age levels."
            },
            new FleschKincaidLevel()
            {
                LowerBound = 60, UpperBound = 70, SchoolLevel = "8th and 9th Grade", Readability = "Plain English. Easily understood by 13 to 15-year-old students."
            },
            new FleschKincaidLevel()
            {
                LowerBound = 50, UpperBound = 60, SchoolLevel = "10th to 12th Grade", Readability = "Fairly difficult to read."
            },
            new FleschKincaidLevel()
            {
                LowerBound = 40, UpperBound = 50, SchoolLevel = "College Level", Readability = "Difficult to read."
            },
            new FleschKincaidLevel()
            {
                LowerBound = 0, UpperBound = 30, SchoolLevel = "College Graduate", Readability = "Very difficult to read. Best understood by university graduates."
            }
        };

        public static readonly List<AutomatedReadabilityLevel> AutomatedReadabilityLevels = new List<AutomatedReadabilityLevel>()
        {
            new AutomatedReadabilityLevel() { Score = 1, Age = "5-6", Grade = "Kindergarten" },
            new AutomatedReadabilityLevel() { Score = 2, Age = "6-7", Grade = "First Grade" },
            new AutomatedReadabilityLevel() { Score = 3, Age = "7-8", Grade = "Second Grade" },
            new AutomatedReadabilityLevel() { Score = 4, Age = "8-9", Grade = "Third Grade" },
            new AutomatedReadabilityLevel() { Score = 5, Age = "9-10", Grade = "Fourth Grade" },
            new AutomatedReadabilityLevel() { Score = 6, Age = "10-11", Grade = "Fifth Grade" },
            new AutomatedReadabilityLevel() { Score = 7, Age = "11-12", Grade = "Sixth Grade" },
            new AutomatedReadabilityLevel() { Score = 8, Age = "12-13", Grade = "Seventh Grade" },
            new AutomatedReadabilityLevel() { Score = 9, Age = "13-14", Grade = "Eight Grade" },
            new AutomatedReadabilityLevel() { Score = 10, Age = "14-15", Grade = "Ninth Grade" },
            new AutomatedReadabilityLevel() { Score = 11, Age = "15-16", Grade = "Tenth Grade" },
            new AutomatedReadabilityLevel() { Score = 12, Age = "16-17", Grade = "Eleventh Grade" },
            new AutomatedReadabilityLevel() { Score = 13, Age = "17-18", Grade = "Twelfth Grade" },
            new AutomatedReadabilityLevel() { Score = 14, Age = "18+", Grade = "College" },
        };


        private static string CleanText(string text)
        {
            //Guard against null text
            if (text == null)
                return string.Empty;

            foreach (var tag in FullStopTags)
            {
                text = Regex.Replace(text, "</" + tag + ">", ".", RegexOptions.IgnoreCase);
            }
            text = Regex.Replace(text, @"<[^>]+>", string.Empty);   // strip tags - this may fail in some cases
            text = Regex.Replace(text, @"["",:;\()-]", " ");        // Replace commas, hyphens, quotes etc
            text = Regex.Replace(text, @"[\.!?]", ".");             // Unify terminators
            text = text.Trim() + ".";                               // Add final terminator
            text = Regex.Replace(text, @"\s+", " ");                // Replace new lines, multiple spaces
            text = Regex.Replace(text, @"[\.][\. ]+", ".");         // Remove duplicated terminators
            text = Regex.Replace(text, @"[ ]*[\.]", ". ").TrimEnd();// Pad sentence terminators
            text = Regex.Replace(text, @"[\.] [^ ]+", m => m.Value.ToLower()); // Lower case all words following terminators

            return text;
        }

        private readonly string _text;

        protected TextStatisticAnalyser(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Calculate all the available measurements and return a Statistics strut
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Statistics</returns>
        public static TextStatistics Calculate(string text)
        {
            var parsed = Parse(text);

            return new TextStatistics()
            {
                FleschKincaidReadingEase = parsed.FleschKincaidReadingEase(),
                FleschKincaidGradeLevel = parsed.FleschKincaidGradeLevel(),
                ColemanLiauIndex = parsed.ColemanLiauIndex(),
                GunningFogScore = parsed.GunningFogScore(),
                SMOGIndex = parsed.SMOGIndex(),
                AutomatedReadabilityIndex = parsed.AutomatedReadabilityIndex(),
                CleanText = parsed._text,
                LetterCount = parsed.LetterCount,
                SentenceCount = parsed.SentenceCount,
                WordCount = parsed.WordCount,
                AverageSyllablesPerWord = parsed.AverageSyllablesPerWord,
                AverageWordsPerSentence = parsed.AverageWordsPerSentence,
                ReadingTime = parsed.ReadingTime(),
                SpeakingTime = parsed.SpeakingTime()
            };
        }

        /// <summary>
        /// Gives the Flesch-Kincaid Reading Ease of text entered rounded to one digit
        /// </summary>
        /// <returns></returns>
        public double FleschKincaidReadingEase()
        {
            return Math.Round(206.835 - (1.015 * AverageWordsPerSentence) - (84.6 * AverageSyllablesPerWord), 1);
        }

        /// <summary>
        /// Gives the Flesch-Kincaid Grade level of text entered rounded to one digit
        /// </summary>
        /// <returns></returns>
        public double FleschKincaidGradeLevel()
        {
            return Math.Round(((0.39 * AverageWordsPerSentence) + (11.8 * AverageSyllablesPerWord) - 15.59), 1);
        }

        /// <summary>
        /// Gives the Gunning-Fog score of text entered rounded to one digit
        /// </summary>
        /// <returns></returns>
        public double GunningFogScore()
        {
            return Math.Round(((AverageWordsPerSentence + PercentageWordsWithThreeSyllables * 100) * 0.4), 1);
        }


        /// <summary>
        /// Gives the Coleman-Liau Index of text entered rounded to one digit
        /// </summary>
        /// <returns></returns>
        public double ColemanLiauIndex()
        {
            return Math.Round(((5.89 * ((double)LetterCount / (double)WordCount)) - (0.3 * ((double)SentenceCount / (double)WordCount)) - 15.8), 1);
        }

        /// <summary>
        /// Gives the SMOG Index of text entered rounded to one digit
        /// </summary>
        /// <returns></returns>
        public double SMOGIndex()
        {
            return Math.Round(1.043 * Math.Sqrt(((double)WordsWithThreeSyllables * (30 / (double)SentenceCount)) + 3.1291), 1);
        }

        /// <summary>
        /// Gives the Automated Readability Index of text entered rounded to one digit
        /// </summary>
        /// <returns></returns>
        public double AutomatedReadabilityIndex()
        {
            return Math.Round(((4.71 * ((double)LetterCount / (double)WordCount)) + (0.5 * ((double)WordCount / (double)SentenceCount)) - 21.43), 1);
        }

        /// <summary>
        /// Readings the time in seconds
        /// </summary>
        /// <returns></returns>
        public int ReadingTime()
        {
            //(60/225) * WordCount
            int secondsInMinute = 60;
            return (int)Math.Round((secondsInMinute / (double)avarageReadingWordsPerMinute) * WordCount);
        }

        /// <summary>
        /// Speakings the time in seconds
        /// </summary>
        /// <returns></returns>
        public int SpeakingTime()
        {
            int secondsInMinute = 60;
            return (int)Math.Round((secondsInMinute / (double)avarageSpeakingWordsPerMinute) * WordCount);
        }

        public int Length
        {
            get { return _text.Length; }
        }

        private int? _letterCount;
        /// <summary>
        /// Gets the letter count, ignoring all non-word characters.
        /// </summary>
        public int LetterCount
        {
            get
            {
                if (_letterCount == null)
                {
                    var charString = Regex.Replace(_text, @"[^A-Za-z]+", string.Empty);

                    _letterCount = charString.Length;
                }

                return _letterCount.Value;
            }
        }

        private int? _sentenceCount;
        public int SentenceCount
        {
            get
            {
                if (_sentenceCount == null)
                {
                    _sentenceCount = Math.Max(1, _text.ToCharArray().Count(c => c == '.'));
                }

                return _sentenceCount.Value;
            }
        }

        private int? _wordCount;
        public int WordCount
        {
            get
            {
                if (_wordCount == null)
                {
                    _wordCount = 1 + _text.ToCharArray().Count(c => c == ' ');
                }

                return _wordCount.Value;
            }
        }

        private int? _wordsWithThreeSyllables;
        public int WordsWithThreeSyllables
        {
            get
            {
                if (_wordsWithThreeSyllables == null)
                {
                    _wordsWithThreeSyllables = CountWordsWithThreeSyllables();
                }

                return _wordsWithThreeSyllables.Value;
            }
        }


        private double? _percentageWordsWithThreeSyllables;
        public double PercentageWordsWithThreeSyllables
        {
            get
            {
                if (_percentageWordsWithThreeSyllables == null)
                {
                    _percentageWordsWithThreeSyllables =
                        (double)CountWordsWithThreeSyllables(false) / (double)WordCount;
                }

                return _percentageWordsWithThreeSyllables.Value;
            }
        }

        private double? _averageSyllablesPerWord;
        public double AverageSyllablesPerWord
        {
            get
            {
                if (_averageSyllablesPerWord == null)
                {
                    int totalSyllables = _text.Split(' ').Sum(w => SyllableCount(w));
                    _averageSyllablesPerWord = (double)totalSyllables / WordCount;
                }

                return _averageSyllablesPerWord.Value;
            }
        }

        private double? _averageWordsPerSentence;
        public double AverageWordsPerSentence
        {
            get
            {
                if (_averageWordsPerSentence == null)
                {
                    _averageWordsPerSentence = (double)WordCount / SentenceCount;
                }

                return _averageWordsPerSentence.Value;
            }
        }

        private int CountWordsWithThreeSyllables(bool countProperNouns = true)
        {
            return _text.Split(' ')
                .Count(w => (countProperNouns || !Regex.IsMatch(w, @"^[A-Z]")) && SyllableCount(w) > 2);
        }

        public static int SyllableCount(string word)
        {
            word = Regex.Replace(word.ToLower(), @"[^a-z]", string.Empty);

            int syllableCount;

            var problemWords = new Dictionary<string, int>
            {
                {"abalone"          , 4},
                {"abare"            , 3},
                {"abed"             , 2},
                {"abruzzese"        , 4},
                {"abbruzzese"       , 4},
                {"aborigine"        , 5},
                {"acreage"          , 3},
                {"adame"            , 3},
                {"adieu"            , 2},
                {"adobe"            , 3},
                {"anemone"          , 4},
                {"apache"           , 3},
                {"aphrodite"        , 4},
                {"apostrophe"       , 4},
                {"ariadne"          , 4},
                {"cafe"             , 2},
                {"calliope"         , 4},
                {"catastrophe"      , 4},
                {"chile"            , 2},
                {"chloe"            , 2},
                {"circe"            , 2},
                {"coyote"           , 3},
                {"epitome"          , 4},
                {"forever"          , 3},
                {"gethsemane"       , 4},
                {"guacamole"        , 4},
                {"hyperbole"        , 4},
                {"jesse"            , 2},
                {"jukebox"          , 2},
                {"karate"           , 3},
                {"machete"          , 3},
                {"maybe"            , 2},
                {"people"           , 2},
                {"recipe"           , 3},
                {"sesame"           , 3},
                {"shoreline"        , 2},
                {"simile"           , 3},
                {"syncope"          , 3},
                {"tamale"           , 3},
                {"yosemite"         , 4},
                {"daphne"           , 2},
                {"eurydice"         , 4},
                {"euterpe"          , 3},
                {"hermione"         , 4},
                {"penelope"         , 4},
                {"persephone"       , 4},
                {"phoebe"           , 2},
                {"zoe"              , 2}
           };

            if (problemWords.TryGetValue(word, out syllableCount))
            {
                return syllableCount;
            }

            // These syllables would be counted as two but should be one
            var subSyllables = new[]
            {
                 "cia(l|$)" // glacial, acacia
                ,"tia"
                ,"cius"
                ,"cious"
                ,"[^aeiou]giu"
                ,"[aeiouy][^aeiouy]ion"
                ,"iou"
                ,"sia$"
                ,"eous$"
                ,"[oa]gue$"
                ,".[^aeiuoycgltdb]{2,}ed$"
                ,".ely$"
                ,"^jua"
                ,"uai"
                ,"eau"
                ,"[aeiouy](b|c|ch|d|dg|f|g|gh|gn|k|l|ll|lv|m|mm|n|nc|ng|nn|p|r|rc|rn|rs|rv|s|sc|sk|sl|squ|ss|st|t|th|v|y|z)e$"
                ,"[aeiouy](b|c|ch|dg|f|g|gh|gn|k|l|lch|ll|lv|m|mm|n|nc|ng|nch|nn|p|r|rc|rn|rs|rv|s|sc|sk|sl|squ|ss|th|v|y|z)ed$"
                ,"[aeiouy](b|ch|d|f|gh|gn|k|l|lch|ll|lv|m|mm|n|nch|nn|p|r|rn|rs|rv|s|sc|sk|sl|squ|ss|st|t|th|v|y)es$"
                ,"^busi$"
            };

            // These syllables would be counted as one but should be two
            var addSyllables = new[] {
                 @"ia"
                ,@"riet"
                ,@"dien"
                ,@"iu"
                ,@"io"
                ,@"ii"
                ,@"[aeiouym]bl$"
                ,@"[aeiou]{3}"
                ,@"^mc"
                ,@"ism$"
                ,@"([^aeiouy])\1l$"
                ,@"[^l]lien"
                ,@"^coa[dglx]."
                ,@"[^gq]ua[^auieo]"
                ,@"dnt$"
                ,@"uity$"
                ,@"ie(r|st)$"
            };

            // Single syllable prefixes and suffixes
            var prefixSuffix = new[]{
                 @"^un"
                ,@"^fore"
                ,@"ly$"
                ,@"less$"
                ,@"ful$"
                ,@"ers?$"
                ,@"ings?$"
            };

            int prefixSuffixCount = 0;
            foreach (var regex in prefixSuffix)
            {
                if (Regex.IsMatch(word, regex))
                {
                    word = Regex.Replace(word, regex, string.Empty);
                    prefixSuffixCount++;
                }
            }

            int wordPartCount = Regex.Split(word, @"[^aeiouy]+").Count(s => s != string.Empty);

            syllableCount = wordPartCount + prefixSuffixCount;
            foreach (var regex in subSyllables)
            {
                if (Regex.IsMatch(word, regex)) syllableCount--;
            }
            foreach (var regex in addSyllables)
            {
                if (Regex.IsMatch(word, regex)) syllableCount++;
            }

            return Math.Max(1, syllableCount);
        }

        public static FleschKincaidLevel GetLevelForFleschKincaidScore(double score)
        {
            return FleschKincaidLevels.FirstOrDefault(l => score > l.LowerBound && score <= l.UpperBound);
        }

        public static FleschKincaidLevel GetLevelForFleschKincaidScore(TextStatistics stats)
        {
            return GetLevelForFleschKincaidScore(stats.FleschKincaidReadingEase);
        }

        public static AutomatedReadabilityLevel GetAutomatedReadabilityLevel(double score)
        {
            return AutomatedReadabilityLevels.FirstOrDefault(l => l.Score == (int)Math.Ceiling(score));
        }

        public static AutomatedReadabilityLevel GetAutomatedReadabilityLevel(TextStatistics stats)
        {
            return GetAutomatedReadabilityLevel(stats.AutomatedReadabilityIndex);
        }
    }
}
