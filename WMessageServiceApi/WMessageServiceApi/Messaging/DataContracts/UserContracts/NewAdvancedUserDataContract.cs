using System;
using System.Runtime.Serialization;

namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
    [DataContract(Name = "NewAdvancedUserDataContract")]
    public class NewAdvancedUserDataContract : NewUserDataContract
    {
        [DataMember(Name = "AdvanceStartDatetime")]
        public DateTime? AdvanceStartDatetime { get; set; }

        [DataMember(Name = "AdvanceEndDatetime")]
        public DateTime? AdvanceEndDatetime { get; set; }
    }
}