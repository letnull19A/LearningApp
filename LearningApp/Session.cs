using System;

namespace LearningApp
{
    /// <summary>
    /// Структура отвечающая за хранение сессии
    /// </summary>
    public struct Session
    {
        /// <summary>
        /// Поле с Id пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Поле с именем пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Поле с Фамилией пользователя
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Поле с Отчеством пользователя
        /// </summary>
        public string FatherName { get; set; }

        /// <summary>
        /// Поле с Id пользователя
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Поле с Ролью прользователя
        /// </summary>
        public string RoleName { get; set; }
    }
}