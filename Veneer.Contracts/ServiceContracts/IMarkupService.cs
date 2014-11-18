using System.ServiceModel;
using System.ServiceModel.Web;
using Veneer.Contracts.Enums;

namespace Veneer.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IMarkupService
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string Get(ContentTypes section);
    }
}
