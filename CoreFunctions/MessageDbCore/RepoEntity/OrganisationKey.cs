using System.Runtime.Serialization;

namespace MessageDbCore.RepoEntity
{
	[DataContract(Name = "OrganisationKey")]
	public class OrganisationKey
	{
		[DataMember(Name = "Id")]
		public long Id { get; set; }

		[DataMember(Name = "Name")]
		public string Name { get; set; }

		[DataMember(Name = "OKey")]
		public string OKey { get; set; }
	}
}