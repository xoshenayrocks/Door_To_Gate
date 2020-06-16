using System;
using System.Threading.Tasks;

namespace Door_To_Gate.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
