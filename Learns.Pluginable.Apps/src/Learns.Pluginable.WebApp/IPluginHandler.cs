using System.Collections.Generic;

namespace Learns.Pluginable.WebApp
{
    public interface IPluginHandler
    {
        IEnumerable<string> GetPlugins();
    }
}