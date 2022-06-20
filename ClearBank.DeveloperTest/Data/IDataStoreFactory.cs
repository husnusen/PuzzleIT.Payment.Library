﻿using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClearBank.DeveloperTest.Data
{
    public interface  IDataStoreFactory
    {
        IDataStore GetDataStore(DataStoreTypes dataStoreType);
    }
}
