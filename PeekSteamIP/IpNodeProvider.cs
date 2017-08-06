using System;
using System.Collections.Generic;
using System.Linq;

namespace PeekSteamIP
{
    public static class IpNodeProvider
    {
        private static IList<IpNode> _nodes;

        public static IList<IpNode> All()
        {
            if (_nodes != null)
                return _nodes;

            try
            {
                var iniFile = new IniFile(Env.ConfigFilePath);
                var entriyes = iniFile.AllKeyValues(Env.ConfigSectionIpList);

                _nodes = entriyes
                    .Select(x => new IpNode(x.Key, x.Value))
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
