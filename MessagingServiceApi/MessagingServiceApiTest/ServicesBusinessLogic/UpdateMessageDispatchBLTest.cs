using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceFunctions.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class UpdateMessageDispatchBLTest
	{
		[TestMethod]
		public void UpdateDispatchAsReceived_BL_Test()
		{
			MessageDispatch messageDispatch = GetMessageDispatchEntity();

			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseOption.DatabaseEngine,
				 DatabaseOption.DbConnectionString);
			dispatchRepo.InsertDispatch(messageDispatch);

			if (messageDispatch.Id == 0)
			{
				Assert.Fail();
			}

			MessageDispatchUpdater dispatchUpdater = new MessageDispatchUpdater();
			dispatchUpdater.UpdateDispatchAsReceived(messageDispatch.Id, DateTime.Now);
		}

		private MessageDispatch GetMessageDispatchEntity()
		{
			return new MessageDispatch
			{
				EmailAddress = "EnjayBanu@wcfmsgproj.com",
				MessageId = 1,
				MessageReceived = false,
				MessageReceivedTime = null
			};
		}
	}
}
