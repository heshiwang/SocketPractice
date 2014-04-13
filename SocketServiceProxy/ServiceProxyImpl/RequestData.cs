using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ServiceProxyImpl
{
    /// <summary>
    /// 服务请求数据
    /// </summary>
    public class RequestData
    {
        public Uri ServiceUri { get; private set; }
        public string MethodName { get; set; }
        public List<KeyValuePair<string, string>> ParameterNameValues { get; private set; }

        public RequestData(Uri serviceUri, string methodName)
            : this(serviceUri, methodName, new List<KeyValuePair<string, string>>())
        { 
        }

        public RequestData(Uri serviceUri, string methodName, List<KeyValuePair<string, string>> parameterNameValues)
        {
            this.ServiceUri = serviceUri;
            this.MethodName = methodName;
            this.ParameterNameValues = parameterNameValues;
        }

        public void AddParam(string paramName, string paramValue)
        {
            ParameterNameValues.Add(new KeyValuePair<string, string>(paramName, paramValue));
        }
    } 
}