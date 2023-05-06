using System;
using System.Collections.Generic;

namespace LearningApp.Entities
{
    /// <summary>
    /// Класс отвечающий за вопрос в тесте
    /// </summary>
    public sealed class TestQuestionUnit
    {
        /// <summary>
        /// Id вопроса
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string Question { get; set; } = string.Empty;
        /// <summary>
        /// Список вариантов ответов
        /// </summary>
        public List<TestVariantUnit> TestVariants { get; set; }
    }
}
