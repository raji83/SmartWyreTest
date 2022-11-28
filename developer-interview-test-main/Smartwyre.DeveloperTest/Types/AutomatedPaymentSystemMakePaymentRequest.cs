using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Types
{
    /// <summary>
    /// Make Payment Request for Automated Payment System
    /// </summary>
    public class AutomatedPaymentSystemMakePaymentRequest : MakePaymentRequest
    {
        /// <summary>
        /// Validates the request
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            if (!base.Validate())
                return false;

            return PaymentScheme == PaymentScheme.AutomatedPaymentSystem;
        }

        /// <summary>
        /// Validates if the account is eligible for an automated payment system payment
        /// </summary>
        /// <param name="account">Account to transfer from</param>
        /// <returns>bool</returns>
        public override bool ValidateAccountEligibility(Account account)
        {
            if (!base.ValidateAccountEligibility(account))
                return false;

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.AutomatedPaymentSystem))
            {
                return false;
            }

            return account.Status == AccountStatus.Live;
        }
    }
}
