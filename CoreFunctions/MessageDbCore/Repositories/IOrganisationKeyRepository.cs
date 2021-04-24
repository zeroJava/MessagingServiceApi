using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbCore.Repositories
{
	public interface IOrganisationKeyRepository
	{
		void InsertOrganisationKey(OrganisationKey organisationKey);
		void UpdateOrganisationKey(OrganisationKey organisationKey);
		void DeleteOrganisationKey(OrganisationKey organisationKey);

		OrganisationKey GetOrganisationKeyMatchingName(string name);
	}
}