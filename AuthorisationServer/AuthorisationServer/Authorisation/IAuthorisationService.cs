using AuthorisationServer.Access;
using System.ServiceModel;

namespace AuthorisationServer.Authorisation
{
   // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthorisationService" in both code and config file together.
   [ServiceContract]
   public interface IAuthorisationService
   {
      [OperationContract]
      AuthorisationGrant GetAuthorisationCode(AuthorisationRequest request);

      [OperationContract]
      AccessToken GetAuthorisationCodeImplicit(AuthorisationRequest request);
   }
}