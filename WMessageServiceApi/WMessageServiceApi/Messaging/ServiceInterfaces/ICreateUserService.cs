using System.ServiceModel;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICreateUserService" in both code and config file together.
    [ServiceContract]
    public interface ICreateUserService
    {
        [OperationContract]
        void CreateNewUser(NewUserDataContract user);

        [OperationContract]
        void CreateNewAdvancedUser(NewAdvancedUserDataContract user);
    }
}
