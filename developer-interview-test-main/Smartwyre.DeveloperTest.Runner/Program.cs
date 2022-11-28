using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner
{
    public class Program
    {
        private static decimal _amount;
        private static string _creditorAccountNumber;
        private static string _debtorAccountNumber;
        private static DateTime _paymentDate;
        private static PaymentScheme _paymentScheme;

        public static void Main(string[] args)
        {
            try
            {
                var serviceProvider = ConfigureServices();

                var paymentService = serviceProvider.GetService<IPaymentService>();

                if (!ParseInput(args))
                {
                    Console.WriteLine(
                        "Please enter the following parameters: Amount (decimal), CreditorAccountNumber (string), DebtorAccountNumber (string), PaymentDate (Date - dd/mm/yyyy), Payment Scheme (BankToBankTransfer, ExpeditedPayments, AutomatedPaymentSystem)");
                    return;
                }

                var response = paymentService.MakePayment(CreateRequest());
                Console.WriteLine(response.Success
                    ? "Payment was successful"
                    : "An error was encountered processing the payment");

                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private static bool ParseInput(string[] args)
        {

            if (args == null || args.Length != 5)
                return false;

            if (!decimal.TryParse(args[0], out decimal amount))
                return false;

            if (!Enum.TryParse<PaymentScheme>(args[4], true, out PaymentScheme paymentScheme))
            {
                return false;
            }

            if (!DateTime.TryParse(args[3], out DateTime paymentDate))
                return false;

            _amount = amount;
            _creditorAccountNumber = args[1];
            _debtorAccountNumber = args[2];
            _paymentDate = paymentDate;
            _paymentScheme = paymentScheme;

            return true;
        }

        private static MakePaymentRequest CreateRequest()
        {
            var makePaymentRequestFactory = new MakePaymentRequestFactory();
            var makePaymentRequest = makePaymentRequestFactory.Create(_paymentScheme);

            makePaymentRequest.Amount = _amount;
            makePaymentRequest.CreditorAccountNumber = _creditorAccountNumber;
            makePaymentRequest.DebtorAccountNumber = _debtorAccountNumber;
            makePaymentRequest.PaymentDate = _paymentDate;
            makePaymentRequest.PaymentScheme = _paymentScheme;

            return makePaymentRequest;
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<IPaymentService, PaymentService>()
                .AddSingleton<IAccountDataStore, AccountDataStore>()
                .BuildServiceProvider();
        }
    }
}