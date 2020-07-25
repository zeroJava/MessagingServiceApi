using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	public interface IMultiMediaMessageContract
	{
		Guid UniqueGuid { get; set; }
		string FileName { get; set; }
		string MediaFileType { get; set; }
		double? FileSize { get; set; }
		byte[] MediaRawData { get; set; }
	}
}
