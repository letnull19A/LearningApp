using System;
using System.Collections.Generic;

namespace LearningApp.Entities
{
    public sealed class TestUnit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Theme { get; set; }
        public List<TestQuestionUnit> Questions { get; set; }
    }
}
