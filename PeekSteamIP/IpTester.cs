using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace PeekSteamIP
{
    /// <summary>
    /// 包含验证IP地址可用性的的相关方法。
    /// </summary>
    public static class IpTester
    {
        static IpTester()
        {
            // 放大一个域名的并发请求数。
            ServicePointManager.DefaultConnectionLimit = Env.ConnectionLimit;
        }

        /// <summary>
        /// 测试一个 Steam IP 地址是否是可用。
        /// </summary>
        public static TestResult TestSteamIp(string ip)
        {
            var req = PrepareRequest(ip);
            var testResult = new TestResult();
            var beginTime = DateTime.Now;

            try
            {
                var res = (HttpWebResponse)req.GetResponse();
                testResult.HttpStatusCode = res.StatusCode;

                var stream = res.GetResponseStream();
                if (stream == null)
                {
                    testResult.Status = IpStatus.ContentError;
                }
                else
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var body = reader.ReadToEnd();
                        testResult.Status = body.Contains("Steam")
                            ? IpStatus.Good
                            : IpStatus.ContentError;
                    }
                }
            }
            catch (Exception ex)
            {
                testResult.Exception = ex;
                testResult.Status = IpStatus.Exception;

                var webEx = ex as WebException;
                if (webEx != null)
                {
                    switch (webEx.Status)
                    {
                        case WebExceptionStatus.Timeout:
                            testResult.Status = IpStatus.Timeout;
                            break;

                        case WebExceptionStatus.ReceiveFailure:
                            testResult.Status = IpStatus.ConnectionReset;
                            break;

                        default:
                            var res = webEx.Response as HttpWebResponse;
                            if (res != null)
                            {
                                testResult.HttpStatusCode = res.StatusCode;
                                testResult.Status = IpStatus.Exception;
                            }
                            break;
                    }
                }
            }

            var endTime = DateTime.Now;
            var elapsed = (endTime - beginTime).TotalMilliseconds;
            testResult.ElapsedMilliseconds = (int)elapsed;
            return testResult;
        }

        private static HttpWebRequest PrepareRequest(string ip)
        {
            var url = "http://" + ip + "/";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
            req.KeepAlive = true;
            req.Headers["Accept-Language"] = "zh-CN";
            req.Timeout = 5000;
            req.ReadWriteTimeout = 3000;

            FixSteamHost(req);

            return req;
        }

        private static void FixSteamHost(HttpWebRequest req)
        {
            /*
             * 参考 https://stackoverflow.com/questions/1450937/how-to-set-custom-host-header-in-httpwebrequest
             * 要三管齐下：
             * 1 改Host属性；
             * 2 反射调用ChangeInternal
             * 3 反射修改m_ProxyServicePoint=false
            */

            // 1
            req.Host = Env.SteamStoreHost;

            // 2
            req.Headers.GetType().InvokeMember(
                "ChangeInternal",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod,
                null,
                req.Headers,
                new object[] { "Host", Env.SteamStoreHost }
            );

            // 3
            var horribleProxyServicePoint = typeof(ServicePoint).GetField(
                "m_ProxyServicePoint", BindingFlags.NonPublic | BindingFlags.Instance);

            horribleProxyServicePoint?.SetValue(req.ServicePoint, false);
        }
    }
}
