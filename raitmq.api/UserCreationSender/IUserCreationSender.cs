using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raitmq.api.UserCreationSender
{
    public interface IUserCreationSender
    {
       public void Send(string firstName);
    }
}
