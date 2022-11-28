using System;

namespace Smartwyre.DeveloperTest.Types
{
    public class MakePaymentRequest
    {
        public string CreditorAccountNumber { get; set; }

        public string DebtorAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentScheme PaymentScheme { get; set; }

        /// <summary>
        /// Validation logic for the request
        /// </summary>
        /// <returns>bool indicating if request is valid</returns>
        public virtual bool Validate()
        {
            if (Amount == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Validates if the provided account is eligible for the requested transfer
        /// </summary>
        /// <returns>bool indicating if request is valid</returns>
        public virtual bool ValidateAccountEligibility(Account account)
        {
            if (account == null)
                return false;

            return true;
        }
    }
}
