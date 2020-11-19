using AuthorisationServer.Access;
using Cryptography;
using Newtonsoft.Json;
using System;
using AccessEnity = MessageDbCore.EntityClasses.Access;
using UserEntity = MessageDbCore.EntityClasses.User;
using MessageDbCore.Repositories;
using MessageDbLib.DbRepositoryFactories;
using MessageDbLib.Configurations;
using MessageDbCore.DbRepositoryInterfaces;

namespace AuthorisationServer.Validate
{
	public class ValidateServiceBL
	{
		public ValidationResult AccessTokenValidation(string encryptedToken)
		{
			AccessToken accessToken = null;
			try
			{
				accessToken = GetAccessToken(encryptedToken);
			}
			catch (ValidationException exception)
			{
				return GetValidationResult(false, exception.Message,
					exception.Status);
			}

			DateTime currentTime = DateTime.Now;
			if (currentTime > accessToken.EndTime ||
				string.IsNullOrEmpty(accessToken.Token))
			{
				bool isTokenExpired = currentTime > accessToken.EndTime;
				string message = isTokenExpired ? "Access-Token passed expiry date." : "Token value from Access-Token is empty.";
				int status = isTokenExpired ? StatusDictionary.ACCESS_TOKEN_EXPIRED : StatusDictionary.ACCESS_TOKEN_EMPTY;

				return GetValidationResult(false, message, status);
			}

			AccessEnity access = GetAccessEntity(accessToken.Token);
			if (access == null)
			{
				return GetValidationResult(false, "Could not find key matching token in the database.",
					StatusDictionary.TOKEN_ROW_NOT_FOUND);
			}
			ValidationResult result = CheckAccessTokenMatch(accessToken, access);
			return result;
		}

		private AccessToken GetAccessToken(string encryptedToken)
		{
			if (string.IsNullOrEmpty(encryptedToken))
			{
				throw new ValidationException("The encrypted token requested for validation is null.",
					StatusDictionary.ACCESS_TOKEN_EMPTY);
			}

			string decryptedToken = SymmetricEncryption.Decrypt(encryptedToken);
			if (string.IsNullOrEmpty(decryptedToken))
			{
				throw new ValidationException("The decrypted token requested for validation is null.",
					StatusDictionary.ACCESS_TOKEN_EMPTY);
			}

			AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(decryptedToken);
			if (accessToken == null)
			{
				throw new ValidationException("The Access-Token requested for validation is null.",
					StatusDictionary.EXTRACTION_ERROR);
			}
			return accessToken;
		}

		private ValidationResult GetValidationResult(bool validationIsSuccess, string message,
			int status)
		{
			return new ValidationResult
			{
				ValidationIsSuccess = validationIsSuccess,
				Message = message,
				Status = status,
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
					StatusDictionary.ORGANISATION_NAME_EMPTY);
			}

			string decryptedTokenOrganisation = SymmetricEncryption.Decrypt(accessToken.Organisation);
			//string decryptedEntityOrganisation = SymmetricEncryption.Decrypt(accessEntity.Organisation);
			bool propertyMatchFailed = false;
			string propertyName = string.Empty;

			if (decryptedTokenOrganisation != accessEntity.Organisation) // decryptedEntityOrganisation
			{
				propertyMatchFailed = true;
				propertyName = "Organisation";
			}
			if (accessToken.Scope != accessEntity.Scope)
			{
				propertyMatchFailed = true;
				propertyName = "Scope";
			}
			if (accessToken.StartTime != accessEntity.StartTime)
			{
				propertyMatchFailed = true;
				propertyName = "StartTime";
			}
			if (accessToken.EndTime != accessEntity.EndTime)
			{
				propertyMatchFailed = true;
				propertyName = "EndTime";
			}

			bool validationSuccessful = true;
			string message = "Validation was successful.";
			int status = StatusDictionary.SUCCESS;

			if (propertyMatchFailed)
			{
				validationSuccessful = false;
				message = string.Format("The property: {0} from Access-Token and DB Access do not match.", propertyName);
				status = StatusDictionary.TOKEN_VALUE_DOES_NOT_MATCH;
			}
			return GetValidationResult(validationSuccessful, message, status);
		}

		public ValidationResult UserCredentialValidation(string encryptedCredential)
		{
			if (string.IsNullOrEmpty(encryptedCredential))
			{
				return GetValidationResult(false, "The encrypted user credential requested for validation is empty.",
					StatusDictionary.PARAMETER_EMPTY);
			}

			string decryptedToken = SymmetricEncryption.Decrypt(encryptedCredential);
			if (string.IsNullOrEmpty(decryptedToken))
			{
				return GetValidationResult(false, "The decrypted user credential requested for validation is null.",
					StatusDictionary.DECRYPTION_ERROR);
			}

			UserCredential userCredential = JsonConvert.DeserializeObject<UserCredential>(decryptedToken);
			if (userCredential == null)
			{
				return GetValidationResult(false, "The user credential requested for validation is null.",
					StatusDictionary.EXTRACTION_ERROR);
			}

			return CheckUsernamePassword(userCredential.Username, userCredential.Password);
		}

		private ValidationResult CheckUsernamePassword(string usernname, string password)
		{
			bool validationIsSuccess = false;
			string message = "User matching username and password could not be found.";
			int status = StatusDictionary.USER_NOT_FOUND;

			UserEntity user = GetUserEntity(usernname, password);
			if (user != null)
			{
				validationIsSuccess = true;
				message = "Found user matching username and password.";
				status = StatusDictionary.SUCCESS;
			}
			return GetValidationResult(validationIsSuccess, message, status);
		}

		private UserEntity GetUserEntity(string username, string password)
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			return userRepo.GetUserMatchingUsernameAndPassword(username, password);
		}
	}
}