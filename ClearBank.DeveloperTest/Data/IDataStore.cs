using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Data
{
    public interface IDataStore
    {
        Task<Account> GetAccount(string accountNumber);
        Task UpdateAccount(Account account);
    }
}
