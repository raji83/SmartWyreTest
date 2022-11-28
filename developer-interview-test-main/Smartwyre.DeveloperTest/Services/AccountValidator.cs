using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services
{
    public class AccountValidator
    {
        public static MakePaymentResult ValidateAccountCanSatisfyPaymentRequest(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();

            switch (request.PaymentScheme)
            {
                case PaymentScheme.BankToBankTransfer:
                    ValidateBankToBankTransfer(account, result);
                    break;

                case PaymentScheme.AutomatedPaymentSystem:
                    ValidateExpeditedPayments(request, account, result);
                    break;

                case PaymentScheme.ExpeditedPayments:
                    ValidateAutomatedPaymentSystem(account, result);
                    break;
            }

            return result;
        }

        private static void ValidateAutomatedPaymentSystem(Account account, MakePaymentResult result)
        {
            if (account == null)
            {
                result.Success = false;
            }
            else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.ExpeditedPayments))
            {
                result.Success = false;
            }
            else if (account.Status != AccountStatus.Live)
            {
                result.Success = false;
            }
            else
            {
                result.Success = true;
            }
        }

        private static void ValidateExpeditedPayments(MakePaymentRequest request, Account account, MakePaymentResult result)
        {
            if (account == null)
            {
                result.Success = false;
            }
            else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem))
            {
                result.Success = false;
            }
            else if (account.Balance < request.Amount)
            {
                result.Success = false;
            }
            else
            {
                result.Success = true;
            }
        }

        private static void ValidateBankToBankTransfer(Account account, MakePaymentResult result)
        {
            if (account == null)
            {
                result.Success = false;
            }
            else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.BankToBankTransfer))
            {
                result.Success = false;
            }
            else
            {
                result.Success = true;
            }
        }
    }
}
