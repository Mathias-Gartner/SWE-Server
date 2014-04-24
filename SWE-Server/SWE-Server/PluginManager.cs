using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interface;
using log4net;

namespace SWE_Server
{
    public class PluginManager
    {
        private List<IPlugin> plugins = null;
        private static volatile PluginManager Instance;
        private ILog logger;

        private PluginManager()
        {
            plugins = new List<IPlugin>();
            logger = LogManager.GetLogger(GetType());
        }

        public static PluginManager getInstance()
        {
            if (Instance == null)
            {
                Instance = new PluginManager();
            }

            return Instance;
        }

        public void LoadPlugins()
        {
            try
            {
				foreach (string dateiname in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + System.IO.Path.DirectorySeparatorChar + Properties.Settings.Default.PluginPath, "*.dll", SearchOption.AllDirectories))
                {
                    if (dateiname != null)
                    {
                        Assembly asm = Assembly.LoadFile(dateiname);
                        foreach (Type asmtyp in asm.GetTypes())
                        {
                            if (asmtyp.GetInterface("IPlugin") != null)
                            {
                                IPlugin plugin = (IPlugin)Activator.CreateInstance(asmtyp);
                                plugins.Add(plugin);
                            }

                        }
                    }
                }
            }

            catch (DirectoryNotFoundException e) { logger.Error("Plugin directory not found", e); }
            catch (FileNotFoundException e) { logger.Error("Plugin cannot be loaded", e); }
        }

        public List<IPlugin> PluginList
        {
            get
            {
                return plugins;
            }
        }

        public Data HandleRequest(Request request)
        {
            if (request == null || request.Url == null)
                return new Data() { StatusCode = 400 };

            if (!request.Url.Parameters.Keys.Contains("action"))
                request.Url.Parameters.Add("action", "StaticFile");

            var pluginName = request.Url.Parameters["action"];

            var plugin = plugins.Where(p => p.Name == pluginName).FirstOrDefault();
            if (plugin == null)
                return new Data() { StatusCode = 404 };

            Data data;
            try
            {
                data = plugin.CreateProduct(request);
            }
            catch (Exception e)
            {
                data = new Data();
                data.StatusCode = 500;
                logger.Error("Plugin failed", e);
            }
            return data;
        }
    }
}
