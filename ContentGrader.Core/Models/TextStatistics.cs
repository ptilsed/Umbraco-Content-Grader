using ContentGrader.Core.Analysers;

namespace ContentGrader.Core.Models
{
    public class TextStatistics
    {
        private FleschKincaidLevel _fleschKincaidLevel;
        private AutomatedReadabilityLevel _automatedReadabilityLevel;

        public double FleschKincaidReadingEase { get; set; }
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

        public FleschKincaidLevel GetFleschKincaidLevel()
        {
            if (_fleschKincaidLevel == null)
            {
                _fleschKincaidLevel = TextStatisticAnalyser.GetLevelForFleschKincaidScore(this);
            }

            return _fleschKincaidLevel;
        }

        public AutomatedReadabilityLevel GetAutomatedReadabiltyLevel()
        {
            if (_automatedReadabilityLevel == null)
            {
                _automatedReadabilityLevel = TextStatisticAnalyser.GetAutomatedReadabilityLevel(this);
            }

            return _automatedReadabilityLevel;
        }
    }
}
