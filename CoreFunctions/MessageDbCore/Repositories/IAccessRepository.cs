using MessageDbCore.RepoEntity;

namespace MessageDbCore.Repositories
{
	public interface IAccessRepository
	{
		void InsertAccess(Access access);
		void UpdateAccess(Access access);
		void DeleteAccess(Access access);

		Access GetAccessMatchingId(long id);
		Access GetAccessMatchingToken(string token);
	}
}