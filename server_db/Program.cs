using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Program
{
    static void Main(string[] args)
    {
        // 初始化 PKG 模板生成物代码 序列化的 id:type 映射
        PKG.AllTypes.Register();

        // 创建 libuv 运行核心循环
        var loop = new xx.UvLoop();

        // 初始化 rpc 管理器, 设定超时参数: 精度(ms), 默认超时 interval( duration = 精度ms * interval )
        loop.InitRpcManager(1000, 5);

        // 创建数据库服务实例
        var dbService = new DbService(loop);

        // 开始运行
        loop.Run();
    }
}

