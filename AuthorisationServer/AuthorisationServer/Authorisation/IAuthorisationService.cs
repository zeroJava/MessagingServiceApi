using AuthorisationServer.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Authorisation
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthorisationService" in both code and config file together.
    [ServiceContract]
    public interface IAuthorisationService
    {
        [OperationContract]
        AuthorisationGrant GetAuthorisationCode(AuthorisationRequest request);

        [OperationContract]
        AccessToken GetAuthorisationCodeImplicit(AuthorisationRequest request);
    }
}