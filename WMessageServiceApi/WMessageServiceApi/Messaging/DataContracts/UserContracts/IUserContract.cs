using System;

namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
    public interface IUserContract
    {
        string UserName { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Surname { get; set; }
        DateTime? Dob { get; set; }
        string Gender { get; set; }
    }
}
