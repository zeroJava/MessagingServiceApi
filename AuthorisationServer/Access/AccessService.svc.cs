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
				AccessServiceBL accessService = new AccessServiceBL();
				AccessToken accessToken = accessService.GetAccessToken(accessRequest);
				return accessToken;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}

		public ValidationResult CheckAccessTokenValid(string encryptedToken)
		{
			try
			{
				AccessServiceBL accessService = new AccessServiceBL();
				ValidationResult result = accessService.CheckAccessTokenValid(encryptedToken);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}
	}
}