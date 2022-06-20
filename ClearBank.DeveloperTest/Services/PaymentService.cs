using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataStoreFactory _dataStoreFactory;
        private readonly IConfigSettings _config;
        private IDataStore _dataStore;
        public PaymentService(IDataStoreFactory dataStoreFactory, IConfigSettings configSettings)
        {
            _dataStoreFactory = dataStoreFactory;
            _config = configSettings;
        }
        public async  Task<MakePaymentResult> MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = _config.DataStoreType;

            Account account = null;

            _dataStore = GetDataStore((DataStoreTypes)Enum.Parse(typeof(DataStoreTypes), dataStoreType));

            account = await _dataStore.GetAccount(request.DebtorAccountNumber).ConfigureAwait(false);

            var result = new MakePaymentResult();


            result.Success = GetPaymentResult(account, request.PaymentScheme, request.Amount);

            if (result.Success)
            {
                account.Balance -= request.Amount;

                await _dataStore.UpdateAccount(account).ConfigureAwait(false);
            }

            return result;
        }

        private IDataStore GetDataStore(DataStoreTypes dataStoreType) => _dataStoreFactory.GetDataStore(dataStoreType);

        private bool GetPaymentResult(Account account, PaymentScheme paymentScheme, decimal amount) {

            if (account == null) return false;

            switch (paymentScheme)
            {
                case PaymentScheme.Bacs:
                    return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);                    

                case PaymentScheme.FasterPayments:
                    return !(!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) || account.Balance < amount);                   

                case PaymentScheme.Chaps:
                    return !(!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) || account.Status != AccountStatus.Live);                   
            }
            return true;

        }
    }
}
