using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Types
{
    /// <summary>
    /// Make Payment Request for Bank to Bank Transfer
    /// </summary>
    public class BankToBankTransferMakePaymentRequest : MakePaymentRequest
    {
        /// <summary>
        /// Validates the request
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (!base.Validate())
                return false;

            return PaymentScheme == PaymentScheme.BankToBankTransfer;
        }
        /// <summary>
        /// Validates if the account is eligible for a bank to bank transfer
        /// </summary>
        /// <param name="account">Account to transfer from</param>
        /// <returns>bool</returns>
        public override bool ValidateAccountEligibility(Account account)
        {
            if (!base.ValidateAccountEligibility(account))
                return false;

            return account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.BankToBankTransfer);
        }
    }
}
