using MessageDbCore.RepoEntity;

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