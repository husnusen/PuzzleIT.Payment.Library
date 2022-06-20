using ClearBank.DeveloperTest.Types;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentService
    {
        Task<MakePaymentResult> MakePayment(MakePaymentRequest request);
    }
}
