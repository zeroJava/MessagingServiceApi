using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
    public interface IErrorsContract
    {
        string Message { get; set; }
        int Status { get; set; }
    }
}
