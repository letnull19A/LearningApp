using System.Collections.Generic;

namespace LearningApp.Entities
{
    public sealed class TestQuestionUnit
    {
        public string Question { get; set; } = string.Empty;
        public List<TestVariantUnit> TestVariants { get; set; }
    }
}
