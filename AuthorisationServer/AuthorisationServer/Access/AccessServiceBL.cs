using Cryptography;
using MessageDbCore.EntityClasses;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using Newtonsoft.Json;
using System;
using AuthorisationEntity = MessageDbCore.EntityClasses.Authorisation;
using AccessEnity = MessageDbCore.EntityClasses.Access;
using MessageDbCore.DbRepositoryInterfaces;

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
			CheckKeyIsValid(organisationKey.Name, organisationKey.OKey);

			AuthorisationEntity authorisation = GetAuthorisation(accessRequest.AuthenticationCode);
			if (authorisation == null)
			{
				throw new ApplicationException("Could not find Authorisation entry in the database.");
			}

			AccessEnity access = CreateAccess(authorisation.UserId, accessRequest.Scope,
				organisationKey.Name);
			PersistAccess(access);
			DeleteAuthorisation(authorisation);

			AccessToken accessToken = CreateAccessToken(access);
			return accessToken;
		}

		/*
		 * Oraganisation-key is the license key provide to the
		 * client, which a encrypted json string containing the
		 * username and OKey.
		 */
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

		private void CheckKeyIsValid(string organisationName, string encrytedRequestOKey)
		{
			IOrganisationKeyRepository organisationKeyRepo = OrganisationKeyRepoFactory.GetOrganisationKeyRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			OrganisationKey organisationKey = organisationKeyRepo.GetOrganisationKeyMatchingName(organisationName);

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

		private AccessEnity CreateAccess(long userId, string[] scope, string organisationName)
		{
			//string encrptedOrganisationName = SymmetricEncryption.Encrypt(organisationName);
			DateTime currentDateTime = DateTime.Now;
			AccessEnity access = new AccessEnity
			{
				Organisation = organisationName,
				Token = Guid.NewGuid().ToString(),
				UserId = userId,
				StartTime = currentDateTime,
				EndTime = currentDateTime.AddMinutes(30),
				Scope = scope,
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
			string encrptedOrganisationName = SymmetricEncryption.Encrypt(access.Organisation);
			AccessToken accessToken = new AccessToken
			{
				Organisation = encrptedOrganisationName,
				Token = access.Token,
				StartTime = access.StartTime,
				EndTime = access.EndTime,
				Scope = access.Scope,
			};
			return accessToken;
		}

		public AccessToken GetAccessTokenImplicit(string encryptedUsername, string encryptedPassword)
		{
			if (string.IsNullOrEmpty(encryptedUsername) ||
				string.IsNullOrEmpty(encryptedPassword))
			{
				throw new ApplicationException("Username or Password is empty.");
			}

			string usernameDecrypted = SymmetricEncryption.Decrypt(encryptedUsername);
			string passwordDecrypted = SymmetricEncryption.Decrypt(encryptedPassword);

			User user = GetUser(usernameDecrypted, passwordDecrypted);
			if (user == null)
			{
				throw new ApplicationException("Could not find user matching Username and Password.");
			}

			AccessEnity accessEntity = CreateAccess(user.Id, new string[0], user.UserName);
			PersistAccess(accessEntity);
			AccessToken accessToken = CreateAccessToken(accessEntity);

			return accessToken;
		}

		private User GetUser(string username, string password)
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(MessageDbLib.Constants.DatabaseEngineConstant.MSSQLADODOTNET,
				DatabaseOption.DbConnectionString);
			User result = userRepo.GetUserMatchingUsernameAndPassword(username, password);

			return result;
		}
	}
}