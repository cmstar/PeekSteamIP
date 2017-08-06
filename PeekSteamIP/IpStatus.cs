namespace PeekSteamIP
{
    /// <summary>
    /// 定义 Steam IP 的可用性状态。
    /// </summary>
    public enum IpStatus
    {
        /// <summary>
        /// 正常。
        /// </summary>
        Good,

        /// <summary>
        /// 响应超时。
        /// </summary>
        Timeout,

        /// <summary>
        /// 可以访问，但网页内容错误。
        /// </summary>
        ContentError,

        /// <summary>
        /// 连接被重置。
        /// </summary>
        ConnectionReset,

        /// <summary>
        /// 其他异常情况。
        /// </summary>
        Exception
    }
}
