using System;
using System.ServiceModel.Activation;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestRestService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select TestRestService.svc or TestRestService.svc.cs at the Solution Explorer and start debugging.
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class TestRestService : ITestRestService
	{
		public RValue GetTest(int number)
		{
			var message = string.Format("GetTest executed on the {0}",
				DateTime.Now.ToString());
			Write(message);
			return new RValue
			{
				Value = message,
			};
		}

		public void PostTest()
		{
			Write(string.Format("PostTest executed on the {0}",
				DateTime.Now.ToString()));
		}

		public void PostTestTwo(string number)
		{
			Write(string.Format("PostTestTwo executed on the {0}, with values: {1}",
				DateTime.Now.ToString(),
				number));
		}

		public void PostTestThree(string number, int intNumber)
		{
			Write(string.Format("PostTestThree executed on the {0}, with values: {1}, {2}",
				DateTime.Now.ToString(),
				number,
				intNumber));
		}

		private void Write(string message)
		{
			Console.WriteLine(message);
		}
	}
}