using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestRestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TestRestService.svc or TestRestService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TestRestService : ITestRestService
    {
        public int GetTest(int number)
        {
            WriteToFile(string.Format("GetTest executed on the {0}", DateTime.Now.ToString()));
            return 10;
        }

        public void PostTest()
        {
            WriteToFile(string.Format("PostTest executed on the {0}", DateTime.Now.ToString()));
        }

        public void PostTestTwo(string number)
        {
            WriteToFile(string.Format("PostTestTwo executed on the {0}, with values: {1}", DateTime.Now.ToString(), number));
        }

        public void PostTestThree(string number, int intNumber)
        {
            WriteToFile(string.Format("PostTestThree executed on the {0}, with values: {1}, {2}", DateTime.Now.ToString(), number, intNumber));
        }

        private void WriteToFile(string message)
        {
            try
            {
                //string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = @"C:\Users\zero\Downloads";
                string fileName = Path.Combine(path, "test.txt");
                using (StreamWriter file = new StreamWriter(fileName, true))
                {
                    file.WriteLine(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}