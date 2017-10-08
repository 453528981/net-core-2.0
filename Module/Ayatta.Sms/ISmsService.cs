using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ayatta.Sms
{
    public interface ISmsService
    {
        Task<SmsResut> SendMessage(string mobile,  string message, string topic="", int uid = 0);
    }
}
