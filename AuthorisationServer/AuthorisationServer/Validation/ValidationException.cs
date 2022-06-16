using System;

namespace AuthorisationServer.Validation
{
   public class ValidationException : Exception
   {
      public int Status { get; set; }

      public ValidationException(string message, int status) : base(message)
      {
         Status = status;
      }
   }
}