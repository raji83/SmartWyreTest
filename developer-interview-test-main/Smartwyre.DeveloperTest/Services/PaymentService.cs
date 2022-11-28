using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            try
            {
                var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

                if (!request.Validate())
                    return new MakePaymentResult { Success = false };

                if (!request.ValidateAccountEligibility(account))
                    return new MakePaymentResult { Success = false };

                UpdateAccount(request, account);

            }
            catch (Exception e)
            {
                return new MakePaymentResult { Success = false };
            }

            return new MakePaymentResult { Success = true };
        }

        private void UpdateAccount(MakePaymentRequest request, Account account)
        {
            account.Balance -= request.Amount;
            _accountDataStore.UpdateAccount(account);
        }
    }
}
