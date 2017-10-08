using System;
using System.Collections.Generic;
using System.Text;

namespace Ayatta.Sms
{
    public interface ISmsStorage
    {
       bool Save(SmsMessage message);
    }
}
