using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Xml;
using System.ServiceModel.Activation;

namespace WebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAcsService" in both code and config file together.
    [ServiceContract]
    
    public interface IAcsService
    {
        [OperationContract]
       // [WebInvoke(Method = "POST", UriTemplate = "/XMLHeaderRequest", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [WebInvoke(Method = "POST", UriTemplate = "/XMLHeaderRequest" , BodyStyle = WebMessageBodyStyle.WrappedRequest)]
         
        System.IO.Stream XMLHeaderRequest();

    }
}