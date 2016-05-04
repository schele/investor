using Examine.SearchCriteria;

namespace Investor.UmbExamine
{
    public class ExamineValue : IExamineValue
    {
        public Examineness Examineness { get; private set; }

        public string Value { get; private set; }

        public float Level { get; private set; }

        public ExamineValue(Examineness vagueness, string value)
            : this(vagueness, value, 1f)
        {
        }

        public ExamineValue(Examineness vagueness, string value, float level)
        {
            Examineness = vagueness;
            Value = value;
            Level = level;
        }
    }
}