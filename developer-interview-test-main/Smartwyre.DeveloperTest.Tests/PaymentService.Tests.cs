using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        [Fact]
        public void BankToBankTransferShouldBeTrue()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.BankToBankTransfer));

            Assert.True(response.Success);
        }

        [Fact]
        public void BankToBankTransferIncorrectPaymentSchemeShouldBeFalse()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.AutomatedPaymentSystem & AllowedPaymentSchemes.ExpeditedPayments,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.BankToBankTransfer));

            Assert.False(response.Success);
        }

        [Fact]
        public void ExpeditedPaymentsShouldBeTrue()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments,
                Balance = 1000,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);


            var response = paymentService.MakePayment(GetRequest(PaymentScheme.ExpeditedPayments));

            Assert.True(response.Success);
        }

        [Fact]
        public void ExpeditedPaymentsIncorrectPaymentSchemeShouldBeFalse()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer & AllowedPaymentSchemes.AutomatedPaymentSystem,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.ExpeditedPayments));

            Assert.False(response.Success);
        }

        [Fact]
        public void ExpeditedPaymentsBalanceLessThanAmountShouldBeFalse()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.ExpeditedPayments,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.ExpeditedPayments, 1000));

            Assert.False(response.Success);
        }


        [Fact]
        public void AutomatedPaymentsSystemShouldBeTrue()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.AutomatedPaymentSystem,
                Balance = 1000,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.AutomatedPaymentSystem));

            Assert.True(response.Success);
        }

        [Fact]
        public void AutomatedPaymentsSystemIncorrectPaymentSchemeShouldBeFalse()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.BankToBankTransfer & AllowedPaymentSchemes.ExpeditedPayments,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.AutomatedPaymentSystem));

            Assert.False(response.Success);
        }

        [Fact]
        public void AutomatedPaymentsSystemAccountNotLiveShouldBeFalse()
        {
            var mockAccountDataStore = SetupMockAccountDataStore(new Account
            {
                AccountNumber = "12345",
                AllowedPaymentSchemes = AllowedPaymentSchemes.AutomatedPaymentSystem,
                Balance = 100,
                Status = AccountStatus.Disabled
            });

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.AutomatedPaymentSystem));

            Assert.False(response.Success);
        }

        [Fact]
        public void AccountNotFoundShouldReturnFalse()
        {
            var mockAccountDataStore = new Mock<IAccountDataStore>();

            mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>()));

            mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));

            var paymentService = new PaymentService(mockAccountDataStore.Object);

            var response = paymentService.MakePayment(GetRequest(PaymentScheme.AutomatedPaymentSystem));

            Assert.False(response.Success);
        }


        private Mock<IAccountDataStore> SetupMockAccountDataStore(Account account)
        {
            var mockAccountDataStore = new Mock<IAccountDataStore>();

            mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));

            return mockAccountDataStore;
        }

        private MakePaymentRequest GetRequest(PaymentScheme paymentScheme, decimal amount = 100)
        {
            var makePaymentRequestFactory = new MakePaymentRequestFactory();
            var makePaymentRequest = makePaymentRequestFactory.Create(paymentScheme);

            makePaymentRequest.Amount = amount;
            makePaymentRequest.CreditorAccountNumber = "12345";
            makePaymentRequest.DebtorAccountNumber = "67890";
            makePaymentRequest.PaymentDate = DateTime.Today;
            makePaymentRequest.PaymentScheme = paymentScheme;

            return makePaymentRequest;
        }
    }
}
