using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketMessageRoute
{
    /// <summary>
    /// 路由信息
    /// </summary>
    public class RouteData
    {
        /// <summary>
        /// 请求主机名
        /// </summary>
        public string SourceHostName { get; set; }
        /// <summary>
        /// 请求主机名端口
        /// </summary>
        public int SourcePort { get; set; }

        /// <summary>
        /// 路由主机名
        /// </summary>
        public string TargetHostName { get; set; }

        /// <summary>
        /// 路由端口
        /// </summary>
        public int TargetPort { get; set; }
    }
}
