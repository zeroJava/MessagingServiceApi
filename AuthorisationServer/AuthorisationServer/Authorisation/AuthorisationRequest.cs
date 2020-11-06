using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuthorisationServer.Authorisation
{
    [DataContract]
    public class AuthorisationRequest
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string[] Scope { get; set; }

        [DataMember]
        public string Callback { get; set; }
    }
}