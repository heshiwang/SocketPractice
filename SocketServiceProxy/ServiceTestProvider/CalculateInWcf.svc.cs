using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceTestClient
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“CalculateService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 CalculateService.svc 或 CalculateService.svc.cs，然后开始调试。
    public class CalculateInWcf : ICalculate
    {
        public int Add(int num1, int num2)
        {
            return num1 + num2;
        }

        public void AddReturnOutResult(int num1, int num2, out int result)
        {
            result = num1 + num2;
        }
    }
}
