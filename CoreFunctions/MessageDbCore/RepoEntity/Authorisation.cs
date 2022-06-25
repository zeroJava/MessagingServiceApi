using System;
using System.Runtime.Serialization;

namespace MessageDbCore.RepoEntity
{
	[DataContract(Name = "Authorisation")]
	public class Authorisation
	{
		[DataMember(Name = "Id")]
		public long Id { get; set; }

		[DataMember(Name = "AuthorisationCode")]
		public Guid AuthorisationCode { get; set; }

		[DataMember(Name = "StartTime")]
		public DateTime StartTime { get; set; }

		[DataMember(Name = "EndTime")]
		public DateTime EndTime { get; set; }

		[DataMember(Name = "UserId")]
		public long UserId { get; set; }
	}
}