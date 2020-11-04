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
using MessageDbCore.Exceptions;
using MessageDbCore.Enumerations;

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

		public ValidationResult CheckAccessTokenValid(string encryptedToken)
		{
			if (string.IsNullOrEmpty(encryptedToken))
			{
				return GetValidationResult(false, "The encrypted token requested for validation is null.",
					ValidationFailReason.AccessTokenEmpty);
			}

			string decryptedToken = SymmetricEncryption.Decrypt(encryptedToken);
			if (string.IsNullOrEmpty(decryptedToken))
			{
				return GetValidationResult(false, "The decrypted token requested for validation is null.",
					ValidationFailReason.AccessTokenEmpty);
			}
			AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(decryptedToken);
			if (accessToken == null)
			{
				return GetValidationResult(false, "The Access-Token requested for validation is null.",
					ValidationFailReason.TokenExtractError);
			}

			DateTime currentTime = DateTime.Now;
			if (currentTime > accessToken.EndTime)
			{
				return GetValidationResult(false, "Access-Token passed expiry date.",
					ValidationFailReason.AccessTokenExpired);
			}
			if (string.IsNullOrEmpty(accessToken.Token))
			{
				return GetValidationResult(false, "Token value from Access-Token is empty.",
					ValidationFailReason.AccessTokenEmpty);
			}
			AccessEnity access = GetAccessEntity(accessToken.Token);
			if (access == null)
			{
				return GetValidationResult(false, "Could not find key matching token in the database.",
					ValidationFailReason.TokenRowNotFound);
			}
			ValidationResult result = CheckAccessTokenMatch(accessToken, access);
			return result;
		}

		private ValidationResult GetValidationResult(bool isTokenValid, string message,
			ValidationFailReason? failReason)
		{
			return new ValidationResult
			{
				IsTokenValid = isTokenValid,
				Message = message,
				FailReason = failReason,
			};
		}

		private AccessEnity GetAccessEntity(string token)
		{
			IAccessRepository accessRepo = AccessRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			return accessRepo.GetAccessMatchingToken(token);
		}

		private ValidationResult CheckAccessTokenMatch(AccessToken accessToken, AccessEnity accessEntity)
		{
			if (string.IsNullOrEmpty(accessToken.Organisation) ||
				string.IsNullOrEmpty(accessEntity.Organisation))
			{
				return GetValidationResult(false, "The encryted organisation name is empty.",
					ValidationFailReason.OrganisationNameEmpty);
			}

			string decryptedTokenOrganisation = SymmetricEncryption.Decrypt(accessToken.Organisation);
			//string decryptedEntityOrganisation = SymmetricEncryption.Decrypt(accessEntity.Organisation);
			if (decryptedTokenOrganisation != accessEntity.Organisation) // decryptedEntityOrganisation
			{
				return GetValidationResult(false, "The decrypted organisation name from Access-Token and DB Access do not match.",
					ValidationFailReason.TokenValuesDontMatch);
			}
			if (accessToken.Scope != accessEntity.Scope)
			{
				return GetValidationResult(false, "The scope from Access-Token and DB Access do not match.",
					ValidationFailReason.TokenValuesDontMatch);
			}
			if (accessToken.StartTime != accessEntity.StartTime)
			{
				return GetValidationResult(false, "The start-time from Access-Token and DB Access do not match.",
					ValidationFailReason.TokenValuesDontMatch);
			}
			if (accessToken.EndTime != accessEntity.EndTime)
			{
				return GetValidationResult(false, "The end-time from Access-Token and DB Access do not match.",
					ValidationFailReason.TokenValuesDontMatch);
			}
			return GetValidationResult(true, "Validation was successful.", null);
		}
	}
}