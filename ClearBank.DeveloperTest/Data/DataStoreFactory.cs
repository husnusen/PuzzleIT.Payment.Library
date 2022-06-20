
using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public class DataStoreFactory : IDataStoreFactory
    {

        private readonly IServiceProvider _serviceProvider;
        public DataStoreFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IDataStore GetDataStore(DataStoreTypes dataStoreType)
        {
            switch (dataStoreType) {

                case DataStoreTypes.Backup:
                    return (IDataStore)_serviceProvider.GetService(typeof(BackupAccountDataStore));
                default: return (IDataStore)_serviceProvider.GetService(typeof(AccountDataStore)); 
            }
        }
    }
}
