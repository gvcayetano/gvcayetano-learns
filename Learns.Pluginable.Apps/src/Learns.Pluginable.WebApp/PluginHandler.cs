using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Learns.Pluginable.WebApp
{
    public class PluginHandler : IPluginHandler
    {
        private readonly ILogger<PluginHandler> logger;
        public PluginHandler(ILogger<PluginHandler> logger)
        {
            this.logger = logger;
        }
/*

        logger.LogInformation($"{Directory.GetCurrentDirectory()}");
        */

        public IEnumerable<string> GetPlugins()
        {
            logger.LogInformation("GetPlugins hit!");
            return System.Array.Empty<string>();
        }
    }

}