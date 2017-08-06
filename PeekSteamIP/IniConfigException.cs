using System;
namespace PeekSteamIP
{
    public class IniConfigException : Exception
    {
        public IniConfigException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }
    }
}
