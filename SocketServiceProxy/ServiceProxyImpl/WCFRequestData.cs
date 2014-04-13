using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProxyImpl
{
    /// <summary>
    /// WCF服务请求数据
    /// </summary>
    public class WCFRequestData : RequestData
    {
        public string ServiceInterfaceName { get; private set; }

        public WCFRequestData(Uri serviceUri, string methodName, string serviceInterfaceName)
            : base(serviceUri, methodName)
        {
            this.ServiceInterfaceName = serviceInterfaceName;
        }

        public WCFRequestData(Uri serviceUri, string methodName, List<KeyValuePair<string, string>> parameterNameValues, string serviceInterfaceName)
            : base(serviceUri, methodName, parameterNameValues)
        {
            this.ServiceInterfaceName = serviceInterfaceName;
        }
    }
}