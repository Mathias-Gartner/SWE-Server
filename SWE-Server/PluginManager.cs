using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interface;

namespace SWE_Server
{
    public class PluginManager
    {
        private List<IPlugin> plugins = null;
        private static volatile PluginManager Instance;

        private PluginManager()
        {
            plugins = new List<IPlugin>();
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
                foreach (string dateiname in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Properties.Settings.Default.PluginPath, "*.dll", SearchOption.AllDirectories))
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

            catch (DirectoryNotFoundException) { ExceptionHandler.ErrorMsg(1); }
            catch (FileNotFoundException) { ExceptionHandler.ErrorMsg(2); }
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
                ExceptionHandler.ErrorMsg(4, e);
            }
            return data;
        }
    }
}
