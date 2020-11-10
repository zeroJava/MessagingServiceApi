using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthorisationServer.Authorisation;

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
			AuthorisationServiceBL authorisationService = new AuthorisationServiceBL();
			AuthorisationGrant grant = authorisationService.GetAuthorisationCode(request);
			return grant;
		}
	}
}
