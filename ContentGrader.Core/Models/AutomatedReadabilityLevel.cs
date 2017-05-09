namespace ContentGrader.Core.Models
{
    /// <summary>
    /// Represents the readability score attributes
    /// </summary>
    public class AutomatedReadabilityLevel
    {
        public int Score { get; set; }

        public string Age { get; set; }

        public string Grade { get; set; }

        public override string ToString()
        {
            return $"Age {Age}: {Grade}";
        }
    }
}
