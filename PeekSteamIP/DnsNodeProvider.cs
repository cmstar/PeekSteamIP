using System;
using System.Collections.Generic;
using System.Linq;

namespace PeekSteamIP
{
    public static class DnsNodeProvider
    {
        private static IList<DnsNode> _nodes;

        public static IList<DnsNode> All()
        {
            if (_nodes != null)
                return _nodes;

            try
            {
                var iniFile = new IniFile(Env.ConfigFilePath);
                var entriyes = iniFile.AllKeyValues(Env.ConfigSectionDnsList);

                _nodes = entriyes
                    .Select(x => new DnsNode(x.Key, x.Value))
                    .ToArray();

                return _nodes;
            }
            catch (Exception ex)
            {
                throw new IniConfigException(ex);
            }
        }
    }
}
