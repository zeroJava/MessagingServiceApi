using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthorisationServer.Authorisation;
using AuthorisationServer.Access;

namespace WMessageServiceApiTest.AuthorisationBusinessLogic
{
	/// <summary>
	/// Summary description for AuthorisationServiceTest
	/// </summary>
	[TestClass]
	public class AuthorisationServiceTest
	{
		[TestMethod]
		public void AuthorisationGrantTestMethod1()
		{
			AuthorisationGrant authorisationGrant = GetAuthorisationGrant();
			if (authorisationGrant == null)
			{
				Assert.Fail();
			}
		}

		private AuthorisationGrant GetAuthorisationGrant()
		{
			AuthorisationRequest request = new AuthorisationRequest
			{
				Username = "adtestone",
				Password = "adtest",
				Scope = new string[0],
			};
			AuthorisationServiceBl authorisationService = new AuthorisationServiceBl();
			AuthorisationGrant grant = authorisationService.GetAuthorisationCode(request);
			return grant;
		}

		[TestMethod]
		public void AuthorisationCodeImplicit()
		{
			AccessToken authorisationGrant = GetAuthorisationCodeImplicit();
			if (authorisationGrant == null)
			{
				Assert.Fail();
			}
		}

		private AccessToken GetAuthorisationCodeImplicit()
		{
			AuthorisationRequest request = new AuthorisationRequest
			{
				Username = "adtestone",
				Password = "adtest",
				Scope = new string[0],
			};
			AuthorisationServiceBl authorisationService = new AuthorisationServiceBl();
			AccessToken accessToken = authorisationService.GetAuthorisationCodeImplicit(request);
			return accessToken;
		}
	}
}
