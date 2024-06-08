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
	/// <summary>
	/// Summary description for MessageRepoTest
	/// </summary>
	[TestClass]
	public class MessageRepoTest
	{
		public MessageRepoTest()
		{
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void InsertMessage_MsSql_ADODOTNET()
		{
			Random randomNumber = new Random();
			string messagetext = "Unit test message mssql custom engine. Hello for the " + randomNumber.Next(1000000) + " time.";

			Message message = new Message()
			{
				MessageText = messagetext,
				MessageCreated = DateTime.Now,
				SenderId = 1,
				SenderEmailAddress = @"Dada@wcfmsgproj.com"
			};

			IMessageRepository messageRepo = GetMessageRepository();
			messageRepo.InsertMessage(message);

			if (message.Id == 0)
			{
				Assert.Fail("Message did not persist.");
			}
		}

		[TestMethod]
		public void UpdateMessage_MsSql_ADODOTNET()
		{
			Random randomNumber = new Random();
			string messagetext = "Unit test UPDATE message mssql." + randomNumber.Next(1000000) + " time.";

			Message message = new Message()
			{
				MessageText = messagetext,
				MessageCreated = DateTime.Now,
				SenderId = 1,
				SenderEmailAddress = @"Dada@wcfmsgproj.com"
			};

			IMessageRepository messageRepo = GetMessageRepository();
			messageRepo.InsertMessage(message);

			if (message.Id == 0)
			{
				Assert.Fail("Message did not persist");
			}

			Message message1 = messageRepo.GetMessage(message.Id);
			if (message1 == null)
			{
				Assert.Fail("Could not find newly created message.");
			}
			message1.MessageText = string.Format("Messages text has been updated on {0}",
				 DateTime.Now.ToString());
			messageRepo.UpdateMessage(message1);
		}

		[TestMethod]
		public void DeleteMessage_MsSql_ADODOTNET()
		{
			Random randomNumber = new Random();
			string messagetext = "Unit test UPDATE message mssql." + randomNumber.Next(1000000) + " time.";

			Message message = new Message()
			{
				MessageText = messagetext,
				MessageCreated = DateTime.Now,
				SenderId = 1,
				SenderEmailAddress = @"Dada@wcfmsgproj.com"
			};

			IMessageRepository messageRepo = GetMessageRepository();
			messageRepo.InsertMessage(message);

			if (message.Id == 0)
			{
				Assert.Fail("Message did not persist");
			}

			Message message1 = messageRepo.GetMessage(message.Id);
			if (message1 == null)
			{
				Assert.Fail("Could not find newly created message.");
			}
			messageRepo.DeleteMessage(message1);

			Message message2 = messageRepo.GetMessage(message.Id);
			Assert.IsNull(message2);
		}

		[TestMethod]
		public void GetAllMessage_Mssql_ADOdetnet()
		{
			IMessageRepository messageRepo = GetMessageRepository();
			List<Message> messages = messageRepo.GetMessages();
			Assert.IsNotNull(messages);
		}

		[TestMethod]
		public void GetMessageMatchingId_Mssql_ADODOTNET()
		{
			Random randomNumber = new Random();
			string messagetext = "Unit test get message id mssql." + randomNumber.Next(1000000) + " time.";

			Message message = new Message()
			{
				MessageText = messagetext,
				MessageCreated = DateTime.Now,
				SenderId = 1,
				SenderEmailAddress = @"Dada@wcfmsgproj.com"
			};

			IMessageRepository messageRepo = GetMessageRepository();
			messageRepo.InsertMessage(message);

			if (message.Id == 0)
			{
				Assert.Fail("Message did not persist");
			}

			Message message1 = messageRepo.GetMessage(message.Id);
			Assert.IsNotNull(message1);
		}

		[TestMethod]
		public void GetMessagesMatchingText_Mssql_ADODOTNET()
		{
			Random randomNumber = new Random();
			string messagetext = "Unit test get message matching text mssql." + randomNumber.Next(1000000) + " time.";

			Message message = new Message()
			{
				MessageText = messagetext,
				MessageCreated = DateTime.Now,
				SenderId = 1,
				SenderEmailAddress = @"Dada@wcfmsgproj.com"
			};

			IMessageRepository messageRepo = GetMessageRepository();
			messageRepo.InsertMessage(message);

			if (message.Id == 0)
			{
				Assert.Fail("Message did not persist");
			}

			List<Message> messages = messageRepo.GetMessagesMatchingText(messagetext);
			Assert.IsNotNull(messages);
		}

		[TestMethod]
		public void DoesEntityExistMatchingId_Mssql_ADOdotnet()
		{
			IMessageRepository messageRepo = GetMessageRepository();
			Message message = messageRepo.GetMessage(1);
			Assert.IsNotNull(message);
		}



		/*[TestMethod]
	 public void GetMessagesNotReceivedByEmailAddress_MssqlADOdotnet()
	 {
		 IMessageRepository messageRepo = MessageRepoFactory.GetMessageRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
			 DatabaseOption.DbConnectionString);
		 List<Message> result = messageRepo.GetMessagesNotReceivedByEmailAddress("EnjayBanu@wcfmsgproj.com");
		 Assert.IsNotNull(result);
	 }*/

		/*[TestMethod]
	 public void GetMessagesMatchingMessageIds_Mssql_ADOdotnet()
	 {
		 long[] array = new long[] { 1, 2, 3, 4, 5, 6, 7 };
		 IMessageRepository messageRepo = MessageRepoFactory.GetMessageRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
			 DatabaseOption.DbConnectionString);
		 List<MessageTable> result = messageRepo.GetMessage(array);
		 Assert.IsNotNull(result);
	 }*/

		//[TestMethod]
		/*public void GetAllMessagesNotReceivedByEmailAddressMssql()
	 {
		 string connectionString = "server=ABU-PC;Database=WcfMessaging;integrated security=True;";
		 CustomMessageRetrieverMssql messageClass = new CustomMessageRetrieverMssql(connectionString);
		 List<MessageTable> messages = messageClass.GetMessagesNotReceivedByEmailAddress("EnjayBanu@wcfmsgproj.com");
		 //Assert.IsNotNull(messages);
	 }*/

		private IMessageRepository GetMessageRepository()
		{
			return MessageRepoFactory.GetMessageRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
		}
	}
}