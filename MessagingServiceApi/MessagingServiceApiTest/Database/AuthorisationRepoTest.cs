using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WMessageServiceApiTest.Database
{
	[TestClass]
	public class AuthorisationRepoTest
	{
		[TestMethod]
		public void InsertAuthorisation_MsSql_ADODOTNET()
		{
			Authorisation authorisation = new Authorisation()
			{
				AuthorisationCode = Guid.NewGuid(),
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(2),
			};

			IAuthorisationRepository authRepo = GetAuthRepository();
			authRepo.InsertAuthorisation(authorisation);

			if (authorisation.Id == 0)
			{
				Assert.Fail("Authorisation did not persist.");
			}
		}

		[TestMethod]
		public void UpdateAuthorisation_MsSql_ADODOTNET()
		{
			Authorisation authorisation = new Authorisation()
			{
				AuthorisationCode = Guid.NewGuid(),
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(2),
			};

			IAuthorisationRepository authRepo = GetAuthRepository();
			authRepo.InsertAuthorisation(authorisation);

			if (authorisation.Id == 0)
			{
				Assert.Fail("Authorisation did not persist.");
			}

			Authorisation authorisation2 = authRepo.GetAuthorisationMatchingId(authorisation.Id);
			if (authorisation2 == null)
			{
				Assert.Fail("Could not find newly created authorisation.");
			}
			authorisation2.EndTime = DateTime.Now.AddDays(2);
			authRepo.UpdateAuthorisation(authorisation2);
		}

		[TestMethod]
		public void DeleteAuthorisation_MsSql_ADODOTNET()
		{
			Authorisation authorisation = new Authorisation()
			{
				AuthorisationCode = Guid.NewGuid(),
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(2),
			};

			IAuthorisationRepository authRepo = GetAuthRepository();
			authRepo.InsertAuthorisation(authorisation);

			if (authorisation.Id == 0)
			{
				Assert.Fail("Authorisation did not persist.");
			}

			Authorisation authorisation2 = authRepo.GetAuthorisationMatchingId(authorisation.Id);
			if (authorisation2 == null)
			{
				Assert.Fail("Could not find newly created authorisation.");
			}
			authRepo.DeleteAuthorisation(authorisation2);

			Authorisation authorisation3 = authRepo.GetAuthorisationMatchingId(authorisation.Id);
			Assert.IsNull(authorisation3);
		}

		private IAuthorisationRepository GetAuthRepository()
		{
			return AuthorisationRepoFactory.GetAuthorisationRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
		}
	}
}
