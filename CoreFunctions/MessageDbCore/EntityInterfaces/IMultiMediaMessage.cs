using System;

namespace MessageDbCore.EntityInterfaces
{
   public interface IMultiMediaMessage
   {
      Guid UniqueGuid { get; set; }
      string FileName { get; set; }
      string MediaFileType { get; set; }
      double? FileSize { get; set; }
   }
}