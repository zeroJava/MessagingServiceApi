using MessageDbCore.Enumerations;
using MessageDbCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Access
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccessService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AccessService.svc or AccessService.svc.cs at the Solution Explorer and start debugging.
    public class AccessService : IAccessService
    {
        public AccessToken GetAccessToken(AccessRequest accessRequest)
        {
            try
            {
                AccessToken accessToken = new AccessServiceBl().GetAccessToken(accessRequest);
                return accessToken;
            }
            catch (Exception exception)
            {
                LogError(exception.ToString());
                throw new FaultException(exception.ToString());
            }
        }

        public AccessToken GetAccessTokenImplicit(string encryptedUsername, string encryptedPassword)
        {
            try
            {
                AccessToken accessToken = new AccessServiceBl().GetAccessTokenImplicit(encryptedUsername, encryptedPassword);
                return accessToken;
            }
            catch (Exception exception)
            {
                LogError(exception.ToString());
                throw new FaultException(exception.ToString());
            }
        }

        private static void LogError(string message)
		{
            Logging.AppLog.LogError(message);
		}
    }
}