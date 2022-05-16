using Contract.DTOs.MailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
