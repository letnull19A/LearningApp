using System;

namespace LearningApp.Entities
{
    public sealed class TestVariantUnit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuestionId { get; set; }
        public string Answer { get; set; } = string.Empty;
        public bool IsRight { get; set; } = false;
    }
}
