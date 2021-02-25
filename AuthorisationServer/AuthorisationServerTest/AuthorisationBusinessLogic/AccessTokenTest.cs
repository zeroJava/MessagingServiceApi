using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthorisationServer.Authorisation;
using AuthorisationServer.Access;
using Newtonsoft;
using Newtonsoft.Json;
using Cryptography;
using AuthorisationServer.Validation;
using MessageDbLib.DbRepository.ADO;
using MessageDbLib.Configurations;

namespace WMessageServiceApiTest.AuthorisationBusinessLogic
{
	/// <summary>
	/// Summary description for AccessTokenTest
	/// </summary>
	[TestClass]
	public class AccessTokenTest
	{
		[TestMethod]
		public void GetAccessToken()
		{
			AuthorisationGrant authorisationGrant = GetAuthorisationGrant();
			AccessToken accessToken = GetAccessToken(authorisationGrant);
			Assert.IsNotNull(accessToken);
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

		private AccessToken GetAccessToken(AuthorisationGrant grant)
		{
			AccessRequest request = new AccessRequest
			{
				Key = GetOrganisationKey(),
				AuthenticationCode = grant.AuthorisationCode.ToString(),
				Scope = grant.Scope,
			};
			AccessServiceBl accessService = new AccessServiceBl();
			AccessToken accessToken = accessService.GetAccessToken(request);
			return accessToken;
		}

		private string GetOrganisationKey()
		{
			OrganisationKeySerDes organisationKey = new OrganisationKeySerDes
			{
				Name = "Test Org fa0e349e-36f5-499f-96c2-1e9742718c66",
				OKey = "UwsyMnydZtMcHp8GSzjQcn/BVs/G8dL+QxvpugdZdMigP5/djNDcRVsS514SWIeG",
			};
			string result = JsonConvert.SerializeObject(organisationKey);
			string enryptedResult = SymmetricEncryption.Encrypt(result);
			return enryptedResult;
		}

		[TestMethod]
		public void CheckAccessTokenValidation()
		{
			AuthorisationGrant authorisationGrant = GetAuthorisationGrant();
			AccessToken accessToken = GetAccessToken(authorisationGrant);
			string accessTokenStr = JsonConvert.SerializeObject(accessToken);
			string encryptedAccessToken = SymmetricEncryption.Encrypt(accessTokenStr);
			ValidationResponse result = CheckAccessTokenValid(encryptedAccessToken);
			if (result == null)
			{
				Assert.Fail();
			}
		}

		private ValidationResponse CheckAccessTokenValid(string encryptedToken)
		{
			string decrytedValyedTemp = SymmetricEncryption.Decrypt(encryptedToken);
			ValidationServiceBl accessService = new ValidationServiceBl();
			ValidationResponse result = accessService.AccessTokenValidation(decrytedValyedTemp);
			return result;
		}

		[TestMethod]
		public void CreateOrganisationKey()
		{
			OrganisationKeySerDes organisationKey = new OrganisationKeySerDes
			{
				Name = "Test App Org",
				OKey = "Test application",
			};
			string result = JsonConvert.SerializeObject(organisationKey);
			string enryptedResult = SymmetricEncryption.Encrypt(result);

			Console.WriteLine(enryptedResult);
		}
	}
}
