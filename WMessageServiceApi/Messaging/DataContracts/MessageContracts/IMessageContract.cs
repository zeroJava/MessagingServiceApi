using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
    public interface IMessageContract
    {
        string AccessToken { get; set; }
        string UserName { get; set; }
        string Message { get; set; }
        List<string> EmailAccounts { get; set; }
        DateTime MessageCreated { get; set; }
    }
}