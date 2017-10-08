using System;
using System.Collections.Generic;
using System.Text;

namespace Ayatta.Nsq
{
    public interface INsqStorage
    {
       bool Save(Message message);
    }
}
