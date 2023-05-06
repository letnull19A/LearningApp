using System;
using System.Collections.Generic;

namespace LearningApp.Entities
{
    public sealed class TestUnit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Theme { get; set; }
        public List<TestQuestionUnit> Questions { get; set; }

        public TestUnit() 
        { }

        public TestUnit(TestUnit testUnit) 
        {
            Id = testUnit.Id;
            Theme = testUnit.Theme;
            Questions = testUnit.Questions;
        }
    }
}
