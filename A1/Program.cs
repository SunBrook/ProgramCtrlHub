using Common.Lib;
using System;

namespace A1
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            // 正式环境，隐藏程序到后台运行
            ConsoleCtrl.ConsoleHide(args, "A1");
#endif
            Console.ReadKey();
        }

        
    }
}
