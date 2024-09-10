using Microsoft.AspNetCore.Http;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPKBeverageManagement.PAYPAL
{
    public interface IPayPalService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
        Task<Payment> ExecutePayment(string paymentId, string payerId);
    }
}
