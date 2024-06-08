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
	public class AccessRepoTest
	{
		[TestMethod]
		public void InsertAccess_MsSql_ADODOTNET()
		{
			Access access = new Access()
			{
				Organisation = "TestOrg",
				Token = Guid.NewGuid().ToString(),
				UserId = 1,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(2),
				Scope = new string[] { "Test" },
			};

			IAccessRepository authRepo = GetAccessRepository();
			authRepo.InsertAccess(access);

			if (access.Id == 0)
			{
				Assert.Fail("Access did not persist.");
			}
		}

		[TestMethod]
		public void UpdateAccess_MsSql_ADODOTNET()
		{
			Access access = new Access()
			{
				Organisation = "TestOrg",
				Token = Guid.NewGuid().ToString(),
				UserId = 1,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(2),
				Scope = new string[] { "Test" },
			};

			IAccessRepository accessRepo = GetAccessRepository();
			accessRepo.InsertAccess(access);

			if (access.Id == 0)
			{
				Assert.Fail("Access did not persist.");
			}

			Access access2 = accessRepo.GetAccessMatchingId(access.Id);
			if (access2 == null)
			{
				Assert.Fail("Could not find newly created Access.");
			}
			access2.EndTime = DateTime.Now.AddDays(2);
			accessRepo.UpdateAccess(access2);
		}

		[TestMethod]
		public void DeleteAccess_MsSql_ADODOTNET()
		{
			Access access = new Access()
			{
				Organisation = "TestOrg",
				Token = Guid.NewGuid().ToString(),
				UserId = 1,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(2),
				Scope = new string[] { "Test" },
			};

			IAccessRepository accessRepo = GetAccessRepository();
			accessRepo.InsertAccess(access);

			if (access.Id == 0)
			{
				Assert.Fail("Access did not persist.");
			}

			Access access2 = accessRepo.GetAccessMatchingId(access.Id);
			if (access2 == null)
			{
				Assert.Fail("Could not find newly created Access.");
			}
			accessRepo.DeleteAccess(access2);

			Access Access3 = accessRepo.GetAccessMatchingId(access.Id);
			Assert.IsNull(Access3);
		}

		private IAccessRepository GetAccessRepository()
		{
			return AccessRepoFactory.GetAuthorisationRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
		}
	}
}
