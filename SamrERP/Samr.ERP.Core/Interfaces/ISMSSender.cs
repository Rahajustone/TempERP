using System.Threading.Tasks;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Interfaces
{
    public interface ISMSSender
    {
        Task SendSMSToUserAsync(User destUser, string message, bool hideMessage = false);
    }
}