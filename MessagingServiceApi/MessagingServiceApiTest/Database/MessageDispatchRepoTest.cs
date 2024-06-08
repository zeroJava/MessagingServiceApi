using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace WMessageServiceApiTest.Database
{
	[TestClass]
	public class MessageDispatchRepoTest
	{
		[TestMethod]
		public void InsertMessageDispatch_Mssql_ADOdotnet()
		{
			MessageDispatch dispatch = GetMessageDispatchEntity();
			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			dispatchRepo.InsertDispatch(dispatch);

			if (dispatch.Id == 0)
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void UpdateMessageDispatch_Mssql_ADOdotnet()
		{
			MessageDispatch dispatch = GetMessageDispatchEntity();
			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			dispatchRepo.InsertDispatch(dispatch);

			if (dispatch.Id == 0)
			{
				Assert.Fail();
			}

			DateTime currentDatetime = DateTime.Now;
			dispatch.MessageReceived = true;
			dispatch.MessageReceivedTime = currentDatetime;

			dispatchRepo.UpdateDispatch(dispatch);

			MessageDispatch dispatch2 = dispatchRepo.GetDispatch(dispatch.Id);
			if (!DatesMatch(currentDatetime, dispatch2.MessageReceivedTime.Value))
			{
				Assert.Fail();
			}
		}

		private bool DatesMatch(DateTime date1, DateTime date2)
		{
			DateTime extractedDate1 = new DateTime(date1.Year, date1.Month,
				 date1.Day,
				 date1.Hour,
				 date1.Minute,
				 date1.Second);
			DateTime extractedDate2 = new DateTime(date2.Year, date2.Month,
				 date2.Day,
				 date2.Hour,
				 date2.Minute,
				 date2.Second);
			int result = DateTime.Compare(extractedDate1, extractedDate2);
			return result == 0;
		}

		[TestMethod]
		public void DeleteMessageDispatch_Mssql_ADOdotnet()
		{
			MessageDispatch dispatch = GetMessageDispatchEntity();
			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			dispatchRepo.InsertDispatch(dispatch);

			if (dispatch.Id == 0)
			{
				Assert.Fail();
			}

			MessageDispatch dispatch2 = dispatchRepo.GetDispatch(dispatch.Id);
			dispatchRepo.DeleteDispatch(dispatch2);

			MessageDispatch dispatch3 = dispatchRepo.GetDispatch(dispatch.Id);
			Assert.IsNull(dispatch3);
		}

		[TestMethod]
		public void GetAllMessageDispatch_Mssql_ADOdotnet()
		{
			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			List<MessageDispatch> dispatches = dispatchRepo.GetDispatches();
			Assert.IsNotNull(dispatches);
		}

		[TestMethod]
		public void GetMessageDispatchMatchingId_Mssql_ADOdotnet()
		{
			MessageDispatch dispatch = GetMessageDispatchEntity();
			dispatch.MessageReceived = true;
			dispatch.MessageReceivedTime = DateTime.Now;

			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			dispatchRepo.InsertDispatch(dispatch);

			MessageDispatch dispatch1 = dispatchRepo.GetDispatch(1);
			Assert.IsNotNull(dispatch1);
		}

		//[TestMethod]
		public void RetrieveMessageDispatchUsingSenderId_Mysql_ADOdotnet()
		{
			IMessageDispatchRepository DispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			MessageDispatch Dispatch = DispatchRepo.GetDispatch(1);
			Assert.IsNotNull(Dispatch);
		}


		[TestMethod]
		public void GetAllMessageDispatchsBetweenSenderReceiver_Mssql_ADOdotnet()
		{
			IMessageDispatchRepository DispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			List<MessageDispatch> Dispatchs = DispatchRepo.GetDispatchesSenderReceiver("Dada@wcfmsgproj.com", "EnjayBanu@wcfmsgproj.com", 10000, 2);
			Assert.IsNotNull(Dispatchs);
		}

		[TestMethod]
		public void GetAllMessageDispatchs_Mssql_ADOdotnet()
		{
			IMessageDispatchRepository DispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			List<MessageDispatch> Dispatchs = DispatchRepo.GetDispatches();
			Assert.IsNotNull(Dispatchs);
		}

		[TestMethod]
		public void GetAllMessageDispatchMatchindId_Mssql_ADOdotnet()
		{
			IMessageDispatchRepository DispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			MessageDispatch Dispatch = DispatchRepo.GetDispatch(1);
			Assert.IsNotNull(Dispatch);
		}

		[TestMethod]
		public void GetMessagesDispatchsNotReceivedByEmailAddress_Mssql_ADOdotnet()
		{
			IMessageDispatchRepository dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			List<MessageDispatch> dispatch = dispatchRepo.GetDispatchesNotReceived("EnjayBanu@wcfmsgproj.com");
			Assert.IsNotNull(dispatch);
		}

		/*[TestMethod]
	 public void GetAllMessageDispatchsBetweenSenderReceiverStoredprocedure()
	 {
		 RetrieveMessageDispatchClassEntityFramework retrieveMessageDispatch = new RetrieveMessageDispatchClassEntityFramework(DbContextConstant.MsSqlDbContext);
		 IList<MessageDispatchTable> messages =
			 retrieveMessageDispatch.GetAllMessageDispatchsBetweenSenderReceiverStoredProcedure("Dada@wcfmsgproj.com", "EnjayBanu@wcfmsgproj.com", 0, 2);
		 Assert.IsNotNull(messages);
	 }*/

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
