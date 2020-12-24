namespace WMessageServiceApi.Messaging.DataContracts.LoginContracts
{
    public interface ILoginTokenContract
    {
        bool LoginSuccessful { get; set; }
        string UserName { get; set; }
        string UserEmailAddress { get; set; }
        //User User { get; set; }
    }
}
