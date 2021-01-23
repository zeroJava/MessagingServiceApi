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
using System.Linq;

namespace AuthorisationServer.Validation
{
	public class ValidationServiceBl
	{
		public ValidationResponse AccessTokenValidation(string encryptedToken)
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
			if (currentTime > accessToken.EndTime)
			{
				return GetValidationResult(false, "Access-Token passed expiry date.",
					StatusDictionary.ACCESS_TOKEN_EXPIRED);
			}

			if (string.IsNullOrEmpty(accessToken.Token))
			{
				return GetValidationResult(false, "Token value from Access-Token is empty.",
					StatusDictionary.ACCESS_TOKEN_EMPTY);
			}

			AccessEnity access = GetAccessEntity(accessToken.Token);
			if (access == null)
			{
				return GetValidationResult(false, "Could not find key matching token in the database.",
					StatusDictionary.TOKEN_NOT_FOUND);
			}
			ValidationResponse response = CheckAccessTokenMatch(accessToken, access);
			return response;
		}

		private AccessToken GetAccessToken(string encryptedToken)
		{
			//string decryptedToken = !string.IsNullOrEmpty(encryptedToken) ? SymmetricEncryption.Decrypt(encryptedToken) : string.Empty;
			string decryptedToken = encryptedToken; // for now, we're not using the encrypted token.

			if (string.IsNullOrEmpty(decryptedToken))
			{
				throw new ValidationException("The requested token for validation is empty.", StatusDictionary.ACCESS_TOKEN_EMPTY);
			}

			AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(decryptedToken);
			if (accessToken == null)
			{
				throw new ValidationException("The Access-Token requested for validation is null.", StatusDictionary.EXTRACTION_ERROR);
			}

			return accessToken;
		}

		private ValidationResponse GetValidationResult(bool validationIsSuccess, string message,
			int status)
		{
			return new ValidationResponse
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

		private ValidationResponse CheckAccessTokenMatch(AccessToken accessToken, AccessEnity accessEntity)
		{
			if (string.IsNullOrEmpty(accessToken.Organisation))
			{
				return GetValidationResult(false, "The organisation name is empty.",
					StatusDictionary.PROPERTY_EMPTY);
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

			if (!Enumerable.SequenceEqual(accessToken.Scope, accessEntity.Scope))
			{
				propertyMatchFailed = true;
				propertyName = "Scope";
			}

			if (!CompareDates(accessToken.StartTime, accessEntity.StartTime))
			{
				propertyMatchFailed = true;
				propertyName = "StartTime";
			}

			if (!CompareDates(accessToken.EndTime, accessEntity.EndTime))
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
			ValidationResponse response = GetValidationResult(validationSuccessful, message, status);
			return response;
		}

		private static bool CompareDates(DateTime datetimeOne, DateTime datetimeTwo)
        {
			return datetimeOne.Year == datetimeTwo.Year && datetimeOne.Month == datetimeTwo.Month &&
				datetimeOne.Day == datetimeTwo.Day &&
				datetimeOne.Hour == datetimeTwo.Hour &&
				datetimeOne.Minute == datetimeTwo.Minute &&
				datetimeOne.Second == datetimeTwo.Second;
        }

		public ValidationResponse UserCredentialValidation(string credential)
		{
			/*if (string.IsNullOrEmpty(credential))
			{
				return GetValidationResult(false, "The encrypted user credential requested for validation is empty.", StatusDictionary.PROPERTY_EMPTY);
			}

			//string decryptedToken = SymmetricEncryption.Decrypt(encryptedCredential);
			string decryptedToken = credential;
			if (string.IsNullOrEmpty(decryptedToken))
			{
				return GetValidationResult(false, "The decrypted user credential requested for validation is null.", StatusDictionary.DECRYPTION_ERROR);
			}*/

			UserCredential userCredential = JsonConvert.DeserializeObject<UserCredential>(credential); // decrytedToken
			if (userCredential == null)
			{
				return GetValidationResult(false, "The user credential requested for validation is null.", StatusDictionary.EXTRACTION_ERROR);
			}

			ValidationResponse response = CheckUsernamePassword(userCredential.Username, userCredential.Password);
			return response;
		}

		private ValidationResponse CheckUsernamePassword(string usernname, string password)
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