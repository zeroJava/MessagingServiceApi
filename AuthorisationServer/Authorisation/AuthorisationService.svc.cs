using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Authorisation
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuthorisationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AuthorisationService.svc or AuthorisationService.svc.cs at the Solution Explorer and start debugging.
    public class AuthorisationService : IAuthorisationService
    {
        public AuthorisationGrant GetAuthorisationCode(AuthorisationRequest request)
        {
            try
            {
                AuthorisationServiceBL authorisationService = new AuthorisationServiceBL();
                return authorisationService.GetAuthorisationCode(request);

            }
            catch (Exception exception)
            {
                throw new FaultException(exception.ToString());
            }
        }
    }
}