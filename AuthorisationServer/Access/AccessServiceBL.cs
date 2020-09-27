using Cryptography;
using MessageDbCore.EntityClasses;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using Newtonsoft.Json;
using System;
using AuthorisationEntity = MessageDbCore.EntityClasses.Authorisation;
using AccessEnity = MessageDbCore.EntityClasses.Access;

namespace AuthorisationServer.Access
{
	public sealed class AccessServiceBL
	{
		public AccessToken GetAccessToken(AccessRequest accessRequest)
		{
			OrganisationKeySerDes organisationKey = ExtractOrganisationKey(accessRequest.Key);
			if (organisationKey == null)
			{
				throw new ApplicationException("Extract Organisation-Key process returned a null key.");
			}
			CheckKeyIsValid(organisationKey.OKey);

			AuthorisationEntity authorisation = GetAuthorisation(accessRequest.AuthenticationCode);
			if (authorisation == null)
			{
				throw new ApplicationException("Could not find Authorisation entry in the database.");
			}

			AccessEnity access = CreateAccess(authorisation, accessRequest,
				organisationKey.Name);
			PersistAccess(access);
			DeleteAuthorisation(authorisation);
			
			AccessToken accessToken = CreateAccessToken(access);
			return accessToken;
		}

		private OrganisationKeySerDes ExtractOrganisationKey(string encryptedKey)
		{
			string decryptedKey = SymmetricEncryption.Decrypt(encryptedKey);
			if (string.IsNullOrEmpty(decryptedKey))
			{
				throw new ApplicationException("Encrpyted organisation key return an empty value.");
			}
			OrganisationKeySerDes organisationKey = JsonConvert.DeserializeObject<OrganisationKeySerDes>(decryptedKey);
			return organisationKey;
		}

		private void CheckKeyIsValid(string encrytedRequestOKey)
		{
			IOrganisationKeyRepository organisationKeyRepo = OrganisationKeyRepoFactory.GetOrganisationKeyRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			OrganisationKey organisationKey = organisationKeyRepo.GetOrganisationKeyMatchingName(encrytedRequestOKey);

			if (organisationKey == null)
			{
				throw new ApplicationException("Could not find key matching rquested key name.");
			}

			string decryptedRequestOKey = SymmetricEncryption.Decrypt(encrytedRequestOKey);
			string decryptedDBOKey = SymmetricEncryption.Decrypt(organisationKey.OKey);

			if (decryptedRequestOKey != decryptedDBOKey)
			{
				throw new ApplicationException("The request OKey does not match OKey from the database.");
			}
		}

		private AuthorisationEntity GetAuthorisation(string authorisationCode)
		{
			Guid authorisationCodeGuid;
			if (string.IsNullOrEmpty(authorisationCode) ||
				!Guid.TryParse(authorisationCode, out authorisationCodeGuid))
			{
				return null;
			}
			IAuthorisationRepository authorisationRepo = AuthorisationRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			AuthorisationEntity authorisation = authorisationRepo.GetAuthorisationMatchingAuthCode(authorisationCodeGuid);
			return authorisation;
		}

		private AccessEnity CreateAccess(AuthorisationEntity authorisation, AccessRequest accessRequest,
			string organisationName)
		{
			string encrptedOrganisationName = SymmetricEncryption.Encrypt(organisationName);
			DateTime currentDateTime = DateTime.Now;
			AccessEnity access = new AccessEnity
			{
				Organisation = encrptedOrganisationName,
				Token = Guid.NewGuid().ToString(),
				UserId = authorisation.UserId,
				StartTime = currentDateTime,
				EndTime = currentDateTime.AddMinutes(30),
				Scope = accessRequest.Scope,
			};
			return access;
		}

		private void PersistAccess(AccessEnity access)
		{
			IAccessRepository accessRepo = AccessRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			accessRepo.InsertAccess(access);
		}

		private void DeleteAuthorisation(AuthorisationEntity authorisation)
		{
			IAuthorisationRepository authorisationRepo = AuthorisationRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			authorisationRepo.DeleteAuthorisation(authorisation);
		}

		private AccessToken CreateAccessToken(AccessEnity access)
		{
			AccessToken accessToken = new AccessToken
			{
				Organisation = access.Organisation,
				Token = access.Token,
				StartTime = access.StartTime,
				EndTime = access.EndTime,
				Scope = access.Scope,
			};
			return accessToken;
		}
	}
}