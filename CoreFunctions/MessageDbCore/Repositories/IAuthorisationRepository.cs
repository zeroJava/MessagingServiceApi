using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;

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