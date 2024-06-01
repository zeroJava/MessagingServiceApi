using WMessageServiceApi.Messaging.DataContracts.UserContracts;
using WMessageServiceApi.Messaging.ServiceHelpers;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class CreateUserService : BaseService, ICreateUserService
	{
		public void CreateNewAdvancedUser(NewAdvancedUserDataContract advanceUser)
		{
			CreateUserServiceHelper serviceHelper = new CreateUserServiceHelper();
			serviceHelper.CreateNewAdvancedUser(GetToken(), advanceUser);
		}

		public void CreateNewUser(NewUserDataContract userContract)
		{
			CreateUserServiceHelper serviceHelper = new CreateUserServiceHelper();
			serviceHelper.CreateNewUser(GetToken(), userContract);
		}
	}
}

/*var advanceUser = new AdvancedUser()
{
	USERNAME = user.USERNAME,
	Password = user.Password,
	DOB = user.DOB,
	ADVANCEENDDATETIME = DateTime.Now.AddDays(50d),
	ADVANCESTARTDATETIME = DateTime.Now
};*/
