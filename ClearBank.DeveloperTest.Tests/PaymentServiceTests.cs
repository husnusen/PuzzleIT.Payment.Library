using System;
using System.Collections.Generic;
using System.Text;
using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xbehave;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private PaymentService _paymentService;
        private readonly Mock<IDataStoreFactory> mockDataStoryFactory;
        private readonly Mock<IConfigSettings> mockConfigSettings; 
        private readonly Mock<IDataStore> mockDataStory;
        public PaymentServiceTests()
        {
            mockDataStoryFactory = new Mock<IDataStoreFactory>();
            mockDataStory = new Mock<IDataStore>();
            mockConfigSettings = new Mock<IConfigSettings>();
            _paymentService = new PaymentService(mockDataStoryFactory.Object, mockConfigSettings.Object);
        }

        [Scenario]
        public void ShouldMakePaymentFromBackupStoreSuccesfully_When_PaymentSchemeIsBacs() {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false}; 
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Status = AccountStatus.Live };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Backup)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Backup");


            });
            "when the payment is proceed".x(async () => {

             response =  await  _paymentService.MakePayment(req);

            });
            "Then the payment should be done succesfully".x(() =>
            {
                Assert.True(response.Success);

            });
            "And verify the account is updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Once());

            });
        }


        [Scenario]
        public void ShouldMakePaymentFromBackupStoreSuccesfully_When_PaymentSchemeIsFasterPayment()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Status = AccountStatus.Live };
            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.FasterPayments;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Backup)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Backup");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should be done succesfully".x(() =>
            {
                Assert.True(response.Success);

            });
            "And verify the account is updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Once());

            });
        }


        [Scenario]
        public void ShouldMakePaymentFromBackupStoreSuccesfully_When_PaymentSchemeIsChaps_And_AccountStatusIsLive()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false}; 
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Live };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.Chaps;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Backup)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Backup");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should be done succesfully".x(() =>
            {
                Assert.True(response.Success);

            });
            "And verify the account is updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Once());

            });
        }
       
        [Scenario]
        public void ShouldMakePaymentFromBackupStoreFailed_When_PaymentSchemeIsChaps_And_AccountStatusIsNotLive()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.InboundPaymentsOnly };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.Chaps;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Backup)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Backup");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should failed".x(() =>
            {
                Assert.False(response.Success);

            });
            "And verify the account is not updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Never());

            });
        }
      
        [Scenario]
        public void ShouldMakePaymentFromBackupStoreFailed_When_PaymentSchemeIsFasterPayment_But_BalanceIsLessThenAmount()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 10, AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Status = AccountStatus.Live };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.FasterPayments;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Backup)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Backup");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should failed".x(() =>
            {
                Assert.False(response.Success);

            });
            "And verify the account is NOT updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Never());

            });
        }


        [Scenario]
        public void ShouldMakePaymentFromAccountStoreSuccesfully_WhenPaymentSchemeIsBacs()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false}; 
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Status = AccountStatus.Live };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Account)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Account");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then it should be done succesfully".x(() =>
            {
                Assert.True(response.Success);

            });
        }

        [Scenario]
        public void ShouldMakePaymentFromAccountStoreSuccesfully_When_PaymentSchemeIsFasterPayment()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Status = AccountStatus.Live };
            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.FasterPayments;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Account)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Account");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should be done succesfully".x(() =>
            {
                Assert.True(response.Success);

            });
            "And verify the account is updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Once());

            });
        }


        [Scenario]
        public void ShouldMakePaymentFromAccountStoreSuccesfully_When_PaymentSchemeIsChaps_And_AccountStatusIsLive()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Live };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.Chaps;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Account)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Account");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should be done succesfully".x(() =>
            {
                Assert.True(response.Success);

            });
            "And verify the account is updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Once());

            });
        }

        [Scenario]
        public void ShouldMakePaymentFromAccountStoreFailed_When_PaymentSchemeIsChaps_And_AccountStatusIsNotLive()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 1000, AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.InboundPaymentsOnly };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.Chaps;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Account)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Account");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should failed".x(() =>
            {
                Assert.False(response.Success);

            });
            "And verify the account is not updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Never());

            });
        }

        [Scenario]
        public void ShouldMakePaymentFromAccountStoreFailed_When_PaymentSchemeIsFasterPayment_But_BalanceIsLessThenAmount()
        {

            MakePaymentRequest req = null;
            MakePaymentResult response = new MakePaymentResult { Success = false};
            var account = new Account { AccountNumber = "C41264", Balance = 10, AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Status = AccountStatus.Live };

            "Given a valid payment request".x(() => {

                req = MockPaymentRequest();
                req.PaymentScheme = PaymentScheme.FasterPayments;
                mockDataStoryFactory.Setup(_ => _.GetDataStore(DataStoreTypes.Account)).Returns(mockDataStory.Object);
                mockDataStory.Setup(_ => _.GetAccount(req.DebtorAccountNumber)).ReturnsAsync(account);
                mockDataStory.Setup(_ => _.UpdateAccount(account));
                mockConfigSettings.Setup(_ => _.DataStoreType).Returns("Account");


            });
            "when the payment is proceed".x(async () => {

                response = await _paymentService.MakePayment(req);

            });
            "Then the payment should failed".x(() =>
            {
                Assert.False(response.Success);

            });
            "And verify the account is NOT updated".x(async () =>
            {
                mockDataStory.Verify(_ => _.UpdateAccount(account), Times.Never());

            });
        }

        private MakePaymentRequest MockPaymentRequest() =>new MakePaymentRequest
            {
                Amount = 100,
                CreditorAccountNumber = "C123",
                DebtorAccountNumber = "N4564",
                PaymentDate = DateTime.Now.AddDays(1),
                PaymentScheme = PaymentScheme.Bacs
            };

    }
}
