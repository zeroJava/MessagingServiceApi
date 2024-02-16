using System;
using System.Runtime.Serialization;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	[DataContract(Name = "PictureMessageRequest")]
	public class PictureMessageRequest : MessageRequest, IMultiMediaMessage
	{
		[DataMember(Name = "UniqueGuid")]
		public Guid UniqueGuid { get; set; }

		[DataMember(Name = "FileName")]
		public string FileName { get; set; }

		[DataMember(Name = "MediaFileType")]
		public string MediaFileType { get; set; }

		[DataMember(Name = "FileSize")]
		public double? FileSize { get; set; }

		[DataMember(Name = "ImageType")]
		public string ImageType { get; set; }

		[DataMember(Name = "MediaRawData")]
		public byte[] MediaRawData { get; set; }
	}
}