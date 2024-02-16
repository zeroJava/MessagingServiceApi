using System;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	public interface IMultiMediaMessage
	{
		Guid UniqueGuid { get; set; }
		string FileName { get; set; }
		string MediaFileType { get; set; }
		double? FileSize { get; set; }
		byte[] MediaRawData { get; set; }
	}
}
