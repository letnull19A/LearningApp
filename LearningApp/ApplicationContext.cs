namespace LearningApp
{
    public sealed class ApplicationContext
    {
        private static ApplicationContext _applicationContext;
        private static Session? _currentSession;

        private ApplicationContext()
        {
        }

        public static ApplicationContext CreateContext() 
            => _applicationContext ?? (_applicationContext = new ApplicationContext());

        public static Session? GetSession() 
            => _currentSession ?? new Session();
        
        public static void StartSession(Session session)
        {
            _currentSession = session;
        }

        public static void EndSession()
        {
            _currentSession = null;
        }

    }
}