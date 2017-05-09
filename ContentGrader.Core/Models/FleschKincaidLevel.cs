namespace ContentGrader.Core.Models
{
    /// <summary>
    /// Represents the Flesch Kincaid scroing level
    /// </summary>
    public class FleschKincaidLevel
    {
        public double LowerBound { get; set; }

        public int UpperBound { get; set; }

        public string SchoolLevel { get; set; }

        public string Readability { get; set; }

        public override string ToString()
        {
            return $"{SchoolLevel}: {Readability}";
        }
    }
}
