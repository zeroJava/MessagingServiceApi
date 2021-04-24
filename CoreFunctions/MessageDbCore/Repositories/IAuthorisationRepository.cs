using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbCore.Repositories
{
    public interface IAuthorisationRepository : IDisposable
    {
        void InsertAuthorisation(Authorisation authorisation);
        void UpdateAuthorisation(Authorisation authorisation);
        void DeleteAuthorisation(Authorisation authorisation);

        Authorisation GetAuthorisationMatchingId(long id);
        Authorisation GetAuthorisationMatchingAuthCode(Guid guid);
        List<Authorisation> GetAuthorisationsGreaterThanEndTime(DateTime endtime);
    }
}