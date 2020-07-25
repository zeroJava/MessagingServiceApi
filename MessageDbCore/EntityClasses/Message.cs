using MessageDbCore.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MessageDbCore.EntityClasses
{
    //[Table("MessageTable", Schema = "messagedbo")]
    [DataContract(Name = "MessageTable")]
    public class Message
    {
        [DataMember(Name = "Id")]
        public long Id { get; set; }

        [DataMember(Name = "MessageText")]
        public string MessageText { get; set; }

        [DataMember(Name = "SenderId")]
        public long? SenderId { get; set; }

        [DataMember(Name = "SenderEmailAddress")]
        public string SenderEmailAddress { get; set; }

        [DataMember(Name = "MessageCreated")]
        public DateTime? MessageCreated { get; set; }

        //[ForeignKey("SenderId")]
        [DataMember(Name = "User")]
        public virtual User User { get; set; }

        [DataMember(Name = "MessageDispatches")]
        public virtual ICollection<MessageDispatch> MessageDispatches { get; set; }

        public Message()
        {
            MessageDispatches = new HashSet<MessageDispatch>();
        }
    }
}
