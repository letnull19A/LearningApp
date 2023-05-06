using System;
using System.Collections.Generic;

namespace LearningApp.DataBase
{
    public sealed class DataBaseLoader
    {
        private List<TableLoader> _loaders;

        public DataBaseLoader() 
        {
            _loaders = new List<TableLoader>();
        }

        public void AddLoader(TableLoader loader)
        {
            if (_loaders == null)
                throw new NullReferenceException(nameof(loader));
        }

        public void Load() 
        {
            
        }

    }
}
