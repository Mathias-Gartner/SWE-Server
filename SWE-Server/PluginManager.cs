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
    class PluginManager
    {
        private static List<IPlugin> plugins = null;
        private static volatile PluginManager Instance;

        private PluginManager() { }

        public static PluginManager getInstance()
        {
            if (Instance == null)
            {
                Instance = new PluginManager();
                plugins = new List<IPlugin>();
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
    }
}
