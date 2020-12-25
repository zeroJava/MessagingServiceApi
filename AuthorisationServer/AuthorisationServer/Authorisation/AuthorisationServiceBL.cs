using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityClasses;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorisationServer.Authorisation
{
	public sealed class AuthorisationServiceBL
	{
		public AuthorisationGrant GetAuthorisationCode(AuthorisationRequest request)
		{
			User user = GetUserMatching(request.Username, request.Password);
			if (user == null)
			{
				throw new ApplicationException("Could not find user matching Username and Password.");
			}
			MessageDbCore.EntityClasses.Authorisation authorisation = CreateAuthorisation();
			PersistAuthorisation(authorisation);
			return CreateAuthorisationResult(request, authorisation);
		}

		private User GetUserMatching(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				throw new ApplicationException("Username or Password is not defined.");
			}

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsernameAndPassword(username, password);
			return user;
		}

		private MessageDbCore.EntityClasses.Authorisation CreateAuthorisation()
		{
			MessageDbCore.EntityClasses.Authorisation authorisation = new MessageDbCore.EntityClasses.Authorisation
			{
				AuthorisationCode = Guid.NewGuid(),
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddDays(1),
			};
			return authorisation;
		}

		private void PersistAuthorisation(MessageDbCore.EntityClasses.Authorisation authorisation)
		{
			IAuthorisationRepository authorisationRepo = AuthorisationRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			authorisationRepo.InsertAuthorisation(authorisation);
		}

		private AuthorisationGrant CreateAuthorisationResult(AuthorisationRequest request,
			MessageDbCore.EntityClasses.Authorisation authorisation)
		{
			AuthorisationGrant grant = new AuthorisationGrant
			{
				AuthorisationCode = authorisation.AuthorisationCode,
				Scope = request.Scope,
				Callback = request.Callback,
			};
			return grant;
		}
	}
}