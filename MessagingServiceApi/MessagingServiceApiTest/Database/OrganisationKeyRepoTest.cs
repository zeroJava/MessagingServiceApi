using Cryptography;
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
	public class OrganisationKeyRepoTest
	{
		[TestMethod]
		public void InsertOrganisationKey_MsSql_ADODOTNET()
		{
			string uniqueId = Guid.NewGuid().ToString();
			OrganisationKey organisationKey = new OrganisationKey()
			{
				Name = string.Format("Test Org {0}", uniqueId),
				OKey = SymmetricEncryption.Encrypt(uniqueId),
			};

			IOrganisationKeyRepository authRepo = GetOrganisationKeyRepository();
			authRepo.InsertOrganisationKey(organisationKey);

			if (organisationKey.Id == 0)
			{
				Assert.Fail("OrganisationKey did not persist.");
			}
		}

		[TestMethod]
		public void UpdateOrganisationKey_MsSql_ADODOTNET()
		{
			string uniqueId = Guid.NewGuid().ToString();
			OrganisationKey organisationKey = new OrganisationKey()
			{
				Name = string.Format("Test Org {0}", uniqueId),
				OKey = SymmetricEncryption.Encrypt(uniqueId),
			};

			IOrganisationKeyRepository organisationKeyRepo = GetOrganisationKeyRepository();
			organisationKeyRepo.InsertOrganisationKey(organisationKey);

			if (organisationKey.Id == 0)
			{
				Assert.Fail("OrganisationKey did not persist.");
			}

			OrganisationKey organisationKey2 = organisationKeyRepo.GetOrganisationKeyMatchingName(organisationKey.Name);
			if (organisationKey2 == null)
			{
				Assert.Fail("Could not find newly created OrganisationKey.");
			}
			organisationKey2.OKey = "Value Changed";
			organisationKeyRepo.UpdateOrganisationKey(organisationKey2);
		}

		[TestMethod]
		public void DeleteOrganisationKey_MsSql_ADODOTNET()
		{
			string uniqueId = Guid.NewGuid().ToString();
			OrganisationKey organisationKey = new OrganisationKey()
			{
				Name = string.Format("Test Org {0}", uniqueId),
				OKey = SymmetricEncryption.Encrypt(uniqueId),
			};

			IOrganisationKeyRepository organisationKeyRepo = GetOrganisationKeyRepository();
			organisationKeyRepo.InsertOrganisationKey(organisationKey);

			if (organisationKey.Id == 0)
			{
				Assert.Fail("OrganisationKey did not persist.");
			}

			OrganisationKey organisationKey2 = organisationKeyRepo.GetOrganisationKeyMatchingName(organisationKey.Name);
			if (organisationKey2 == null)
			{
				Assert.Fail("Could not find newly created OrganisationKey.");
			}
			organisationKeyRepo.DeleteOrganisationKey(organisationKey2);

			OrganisationKey Access3 = organisationKeyRepo.GetOrganisationKeyMatchingName(organisationKey.Name);
			Assert.IsNull(Access3);
		}

		private IOrganisationKeyRepository GetOrganisationKeyRepository()
		{
			return OrganisationKeyRepoFactory.GetOrganisationKeyRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
		}
	}
}
