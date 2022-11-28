using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Factories
{
    /// <summary>
    /// Factory to create an appropriate instance of payment request for a specified payment scheme
    /// </summary>
    public class MakePaymentRequestFactory
    {
        public MakePaymentRequest Create(PaymentScheme paymentScheme)
        {
            switch (paymentScheme)
            {
                case PaymentScheme.BankToBankTransfer:
                    {
                        return new BankToBankTransferMakePaymentRequest();
                    }
                case PaymentScheme.AutomatedPaymentSystem:
                    {
                        return new AutomatedPaymentSystemMakePaymentRequest();
                    }
                case PaymentScheme.ExpeditedPayments:
                    {
                        return new ExpeditedPaymentsMakePaymentRequest();
                    }
                default:
                    {
                        return new MakePaymentRequest();
                    }
            }
        }
    }
}
