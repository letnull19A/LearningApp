﻿using System.Configuration;

namespace LearningApp
{
    public sealed class ApplicationContext
    {
        private static ApplicationContext _applicationContext;
        private static Session? _currentSession;
        private static string _connectionString = string.Empty;

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

        public static string GetConnectionString() 
        {
            _connectionString = _connectionString.Length == 0 ? 
                ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString 
                : _connectionString;

            return _connectionString;
        }

    }
}