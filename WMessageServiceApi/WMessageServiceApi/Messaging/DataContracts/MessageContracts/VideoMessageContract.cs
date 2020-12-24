using System;
using System.Runtime.Serialization;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
    [DataContract(Name = "VideoMessageContract")]
    public class VideoMessageContract : MessageContract, IMultiMediaMessageContract
    {
        [DataMember(Name = "UniqueGuid")]
        public Guid UniqueGuid { get; set; }

        [DataMember(Name = "FileName")]
        public string FileName { get; set; }

        [DataMember(Name = "MediaFileType")]
        public string MediaFileType { get; set; }

        [DataMember(Name = "FileSize")]
        public double? FileSize { get; set; }

        [DataMember(Name = "Length")]
        public double? Length { get; set; }

        [DataMember(Name = "MediaRawData")]
        public byte[] MediaRawData { get; set; }
    }
}