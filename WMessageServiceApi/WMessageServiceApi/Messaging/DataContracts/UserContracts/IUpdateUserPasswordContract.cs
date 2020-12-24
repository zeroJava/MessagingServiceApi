namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
    public interface IUpdateUserPasswordContract
    {
        string UserName { get; set; }
        string OldPassword { get; set; }
        string NewPassword { get; set; }
    }
}
