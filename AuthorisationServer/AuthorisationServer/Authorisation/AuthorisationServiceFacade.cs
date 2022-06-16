using AuthorisationServer.Access;
using AuthorisationServer.Logging;
using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;

namespace AuthorisationServer.Authorisation
{
   public sealed class AuthorisationServiceFacade
   {
      public AuthorisationGrant GetAuthorisationCode(AuthorisationRequest request)
      {
         LogInfo("Getting authorisation code");
         User user = GetUserMatching(request.Username, request.Password);
         if (user == null)
         {
            throw new ApplicationException("Could not find user matching Username and Password.");
         }
         MessageDbCore.RepoEntity.Authorisation authorisation = CreateAuthorisation(user.Id);
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

      private MessageDbCore.RepoEntity.Authorisation CreateAuthorisation(long userId)
      {
         MessageDbCore.RepoEntity.Authorisation authorisation = new MessageDbCore.RepoEntity.Authorisation
         {
            AuthorisationCode = Guid.NewGuid(),
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddDays(1),
            UserId = userId,
         };
         return authorisation;
      }

      private void PersistAuthorisation(MessageDbCore.RepoEntity.Authorisation authorisation)
      {
         IAuthorisationRepository authorisationRepo = AuthorisationRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
            DatabaseOption.DbConnectionString);
         authorisationRepo.InsertAuthorisation(authorisation);
      }

      private AuthorisationGrant CreateAuthorisationResult(AuthorisationRequest request,
         MessageDbCore.RepoEntity.Authorisation authorisation)
      {
         AuthorisationGrant grant = new AuthorisationGrant
         {
            AuthorisationCode = authorisation.AuthorisationCode,
            Scope = request.Scope,
            Callback = request.Callback,
         };
         return grant;
      }

      public AccessToken GetAuthorisationCodeImplicit(AuthorisationRequest request)
      {
         User user = GetUserMatching(request.Username, request.Password);
         if (user == null)
         {
            throw new ApplicationException("Could not find user matching Username and Password.");
         }

         MessageDbCore.RepoEntity.Access access = CreateAccess(user.Id, request.Scope,
            user.UserName);
         PersistAccess(access);

         AccessToken accessToken = CreateAccessToken(access);
         return accessToken;
      }

      private MessageDbCore.RepoEntity.Access CreateAccess(long userId, string[] scope, string name)
      {
         DateTime currentDateTime = DateTime.Now;
         MessageDbCore.RepoEntity.Access access = new MessageDbCore.RepoEntity.Access
         {
            Organisation = name,
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            StartTime = currentDateTime,
            EndTime = DateTime.Now.AddYears(100),
            Scope = scope,
         };
         return access;
      }

      private void PersistAccess(MessageDbCore.RepoEntity.Access access)
      {
         IAccessRepository accessRepo = AccessRepoFactory.GetAuthorisationRepository(DatabaseOption.DatabaseEngine,
            DatabaseOption.DbConnectionString);
         accessRepo.InsertAccess(access);
      }

      private AccessToken CreateAccessToken(MessageDbCore.RepoEntity.Access access)
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

      private void LogInfo(string message)
      {
         AppLog.LogInfo(message);
      }
   }
}