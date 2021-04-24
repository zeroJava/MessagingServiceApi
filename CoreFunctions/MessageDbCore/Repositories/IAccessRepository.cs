using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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