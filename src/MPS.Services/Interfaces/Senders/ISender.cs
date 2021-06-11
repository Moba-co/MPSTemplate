using System.Collections.Generic;
using System.Threading.Tasks;
using MPS.Common.DTOs;

namespace MPS.Services.Interfaces.Senders
{
    public interface ISender
    {
        Task<ReturnMessageDto> SendManyAsync(IEnumerable<string> to , string subject, string message, bool isMessageHtml = false);
        Task<ReturnMessageDto> SendAsync(string to, string subject, string message, bool isMessageHtml = false);
        //Task<ReturnMessageDto> FastSendAsyn(string to,string message);
    }
}