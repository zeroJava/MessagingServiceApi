using AuthorisationServer.Access;
using Cryptography;
using Newtonsoft.Json;
using System;
using AccessEnity = MessageDbCore.EntityClasses.Access;
using MessageDbCore.Repositories;
using MessageDbLib.DbRepositoryFactories;
using MessageDbLib.Configurations;

namespace AuthorisationServer.Validate
{
	public class ValidateServiceBL
	{
		public ValidationResult AccessTokenValidation(string encryptedToken)
		{
			if (string.IsNullOrEmpty(encryptedToken))
			{
				return GetValidationResult(false, "The encrypted token requested for validation is null.",
					FailReason.AccessTokenEmpty);
			}

			string decryptedToken = SymmetricEncryption.Decrypt(encryptedToken);
			if (string.IsNullOrEmpty(decryptedToken))
			{
				return GetValidationResult(false, "The decrypted token requested for validation is null.",
					FailReason.AccessTokenEmpty);
			}
			AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(decryptedToken);
			if (accessToken == null)
			{
				return GetValidationResult(false, "The Access-Token requested for validation is null.",
					FailReason.ExtractError);
			}

			DateTime currentTime = DateTime.Now;
			if (currentTime > accessToken.EndTime)
			{
				return GetValidationResult(false, "Access-Token passed expiry date.",
					FailReason.AccessTokenExpired);
			}
			if (string.IsNullOrEmpty(accessToken.Token))
			{
				return GetValidationResult(false, "Token value from Access-Token is empty.",
					FailReason.AccessTokenEmpty);
			}
			AccessEnity access = GetAccessEntity(accessToken.Token);
			if (access == null)
			{
				return GetValidationResult(false, "Could not find key matching token in the database.",
					FailReason.TokenRowNotFound);
			}
			ValidationResult result = CheckAccessTokenMatch(accessToken, access);
			return result;
		}

		private ValidationResult GetValidationResult(bool isTokenValid, string message,
			FailReason? failReason)
		{
			return new ValidationResult
			{
				ValidationIsSuccess = isTokenValid,
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
					FailReason.OrganisationNameEmpty);
			}

			string decryptedTokenOrganisation = SymmetricEncryption.Decrypt(accessToken.Organisation);
			//string decryptedEntityOrganisation = SymmetricEncryption.Decrypt(accessEntity.Organisation);
			if (decryptedTokenOrganisation != accessEntity.Organisation) // decryptedEntityOrganisation
			{
				return GetValidationResult(false, "The decrypted organisation name from Access-Token and DB Access do not match.",
					FailReason.TokenValuesDontMatch);
			}
			if (accessToken.Scope != accessEntity.Scope)
			{
				return GetValidationResult(false, "The scope from Access-Token and DB Access do not match.",
					FailReason.TokenValuesDontMatch);
			}
			if (accessToken.StartTime != accessEntity.StartTime)
			{
				return GetValidationResult(false, "The start-time from Access-Token and DB Access do not match.",
					FailReason.TokenValuesDontMatch);
			}
			if (accessToken.EndTime != accessEntity.EndTime)
			{
				return GetValidationResult(false, "The end-time from Access-Token and DB Access do not match.",
					FailReason.TokenValuesDontMatch);
			}
			return GetValidationResult(true, "Validation was successful.", null);
		}

		public ValidationResult UserCredentialValidation(string encryptedCredential)
		{
			if (string.IsNullOrEmpty(encryptedCredential))
			{
				return GetValidationResult(false, "The encrypted user credential requested for validation is null.",
					FailReason.ParameterEmtpy);
			}

			string decryptedToken = SymmetricEncryption.Decrypt(encryptedCredential);
			if (string.IsNullOrEmpty(decryptedToken))
			{
				return GetValidationResult(false, "The decrypted user credential requested for validation is null.",
					FailReason.DecryptionError);
			}

			UserCredential userCredential = JsonConvert.DeserializeObject<UserCredential>(decryptedToken);
			if (userCredential == null)
			{
				return GetValidationResult(false, "The user credential requested for validation is null.",
					FailReason.ExtractError);
			}

			return null; //TODO implement this after refactor
		}
	}
}