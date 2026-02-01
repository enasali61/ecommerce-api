using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UnAuthurisedException(string Message = "Invalid Email or Password") : Exception(Message)
    {

    }
}
