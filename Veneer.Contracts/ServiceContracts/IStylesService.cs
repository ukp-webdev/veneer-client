using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Web;
using Veneer.Contracts.DataContracts;
using Veneer.Contracts.Enums;

namespace Veneer.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IStylesService
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<ContentStyle> Get(ContentTypes section);
    }
}
