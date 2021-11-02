using AuthorisationServer.Access;
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
                AuthorisationServiceBl authorisationService = new AuthorisationServiceBl();
                return authorisationService.GetAuthorisationCode(request);

            }
            catch (Exception exception)
            {
                LogError(exception.ToString());
                throw new FaultException(exception.Message);
            }
        }

        public AccessToken GetAuthorisationCodeImplicit(AuthorisationRequest request)
        {
            try
            {
                AuthorisationServiceBl authorisationService = new AuthorisationServiceBl();
                return authorisationService.GetAuthorisationCodeImplicit(request);

            }
            catch (Exception exception)
            {
                LogError(exception.ToString());
                throw new FaultException(exception.Message);
            }
        }

        private static void LogError(string message)
        {
            Logging.AppLog.LogError(message);
        }
	}
}