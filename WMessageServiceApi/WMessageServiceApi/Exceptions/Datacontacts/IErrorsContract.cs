namespace WMessageServiceApi.Exceptions.Datacontacts
{
    public interface IErrorsContract
    {
        string Message { get; set; }
        int Status { get; set; }
    }
}
