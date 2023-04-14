using System.Collections.Generic;

namespace LearningApp.Entities
{
    public sealed class TestUnit
    {
        public string Theme { get; set; }
        public List<TestQuestionUnit> Questions { get; set; }
    }
}
