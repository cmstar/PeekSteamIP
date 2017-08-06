using System.Diagnostics;
using System.IO;

namespace PeekSteamIP
{
    /// <summary>
    /// 包含DNS解析相关的方法。
    /// </summary>
    public static class DnsTester
    {
        /// <summary>
        /// 从指定的DNS服务器查询 Steam 域名所对应的IP地址。
        /// </summary>
        /// <param name="server">DNS服务器的地址，如 8.8.8.8。</param>
        /// <returns>Steam 的IP地址，若解析失败，返回 null。</returns>
        public static string ResolveSteamIp(string server)
        {
            // DNS协议简单……简单归简单，自己实现也太麻烦了。
            // 直接调用系统 nslookup 处理。

            var args = Env.SteamStoreHost + " " + server;
            var processStart = new ProcessStartInfo("nslookup", args);
            processStart.UseShellExecute = false;
            processStart.WindowStyle = ProcessWindowStyle.Hidden;
            processStart.CreateNoWindow = true;
            processStart.RedirectStandardOutput = true;
            processStart.RedirectStandardError = true;

            using (var process = new Process())
            {
                process.StartInfo = processStart;
                process.Start();

                using (process.StandardOutput)
                {
                    return ParseResult(process.StandardOutput);
                }
            }
        }

        /// <summary>
        /// 从 nslookup 程序的输出结果中读取出IP地址。
        /// 未能读取到地址则返回 null。
        /// </summary>
        private static string ParseResult(TextReader reader)
        {
#if DEBUG
            var all = reader.ReadToEnd();
            reader = new StringReader(all);
#endif

            /*
             * 一个解析成功的DNS应答结果包含下面这两行，这里忽略其他行，就处理这两行。
             * 
             * Name: store.steampowered.com     -> 第一行
             * Address: xxx.xxx.xxx.xxx         -> 第二行
             */

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // 尝试读取目标结果的第一行，用域名判断。“Name”部分可能是中文（或其他语言）的，无视之。
                if (!line.Contains(Env.SteamStoreHost))
                    continue;

                if (line.Contains("fail"))
                    return null;

                // 在读取到第一行的前提下，读取第二行。
                var address = reader.ReadLine();
                if (address == null)
                    return null;

                // 按“Address:”后的冒号分割。
                var parts = address.Split(':');
                if (parts.Length < 2)
                    return null;

                // 冒号后面的就是地址了。
                var ip = parts[1].Trim();
                return ip;
            }

            return null;
        }
    }
}
