using System.ServiceModel;
using System.ServiceModel.Web;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IContentService
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Content Get(ContentTypes section);
    }
}
