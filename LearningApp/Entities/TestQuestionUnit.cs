using System;
using System.Collections.Generic;

namespace LearningApp.Entities
{
    public sealed class TestQuestionUnit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Question { get; set; } = string.Empty;
        public List<TestVariantUnit> TestVariants { get; set; }
    }
}
