using System.Configuration;

namespace LearningApp
{
    /// <summary>
    /// Класс отвечающий за контекст приложения
    /// </summary>
    public sealed class ApplicationContext
    {
        // Поле с объектом контекста приложения
        private static ApplicationContext _applicationContext;
        // Поле с сессией приложения
        private static Session? _currentSession;
        // Поле со строкой соединения
        private static string _connectionString = string.Empty;

        // Приватный конструктор ApplicationContext
        private ApplicationContext()
        {
        }

        /// <summary>
        /// Метод возвращающий контекст приложения
        /// </summary>
        public static ApplicationContext CreateContext() 
            => _applicationContext ?? (_applicationContext = new ApplicationContext());

        /// <summary>
        /// Метод возвращяющий сессию приложения
        /// </summary>
        public static Session? GetSession() 
            => _currentSession ?? new Session();
        
        /// <summary>
        /// Метод начинающий сессию
        /// </summary>
        public static void StartSession(Session session)
        {
            _currentSession = session;
        }

        /// <summary>
        /// Метод завершающий сессию
        /// </summary>
        public static void EndSession()
        {
            _currentSession = null;
        }

        /// <summary>
        /// Метод для получения строки соединения
        /// </summary>
        public static string GetConnectionString() 
        {
            _connectionString = _connectionString.Length == 0 ? 
                ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString 
                : _connectionString;

            return _connectionString;
        }

    }
}