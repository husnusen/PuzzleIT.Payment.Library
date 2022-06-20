using ClearBank.DeveloperTest.Types;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStore :IDataStore
    {
        public async Task<Account> GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            return await Task.FromResult( new Account());
        }

        public async Task UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
