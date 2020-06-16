using System;
using System.Threading.Tasks;

namespace Door_To_Gate.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
