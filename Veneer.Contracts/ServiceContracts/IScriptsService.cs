using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IScriptsService
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<ContentScript> Get(ContentTypes section);
    }
}
