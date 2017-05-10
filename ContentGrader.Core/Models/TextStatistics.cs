using ContentGrader.Core.Analysers;

namespace ContentGrader.Core.Models
{
    public class TextStatistics
    {
        private FleschKincaidLevel _fleschKincaidLevel;
        private AutomatedReadabilityLevel _automatedReadabilityLevel;

        public double FleschKincaidReadingEase { get; set; }

        public char FleschKincaidReadingEaseGrade
        {
            get
            {
                if (this.FleschKincaidReadingEase < 20)
                    return 'F';

                if (this.FleschKincaidReadingEase < 30)
                    return 'E';

                if (this.FleschKincaidReadingEase < 40)
                    return 'D';

                if (this.FleschKincaidReadingEase < 60)
                    return 'C';

                if (this.FleschKincaidReadingEase >= 60 && this.FleschKincaidReadingEase <= 90)
                    return 'A';

                if (this.FleschKincaidReadingEase > 90)
                    return 'B';

                return ' ';
            }
        }

        public double FleschKincaidGradeLevel { get; set; }
        public double GunningFogScore { get; set; }
        public double ColemanLiauIndex { get; set; }
        public double SMOGIndex { get; set; }
        public double AutomatedReadabilityIndex { get; set; }
        public string CleanText { get; set; }
        public int LetterCount { get; set; }
        public int SentenceCount { get; set; }
        public int WordCount { get; set; }
        public double AverageSyllablesPerWord { get; set; }
        public double AverageWordsPerSentence { get; set; }
        public int ReadingTime { get; set; }
        public int SpeakingTime { get; set; }

        public FleschKincaidLevel FleschKincaidLevel
        {
            get
            {
                if (_fleschKincaidLevel == null)
                {
                    _fleschKincaidLevel = TextStatisticAnalyser.GetLevelForFleschKincaidScore(this);
                }

                return _fleschKincaidLevel;
            }
        }

        public AutomatedReadabilityLevel AutomatedReadabiltyLevel
        {
            get
            {
                if (_automatedReadabilityLevel == null)
                {
                    _automatedReadabilityLevel = TextStatisticAnalyser.GetAutomatedReadabilityLevel(this);
                }

                return _automatedReadabilityLevel;
            }
        }
    }
}
