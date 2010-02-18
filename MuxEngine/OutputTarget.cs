using System;
using mkNETtools.PluginManager;
using mkNETtools.PluginHelper;

namespace mkNETtools.MuxEngine
{
	/// <summary>
	/// Summary description for OutputTarget.
	/// </summary>
	public class OutputTarget
	{
    protected string m_Filename = "";
    protected IBaseOutputPlugin m_Plugin = null;

    private IBaseOutputPlugin SearchForSupportedPlugin(PluginManager.PluginManager manager, string filename)
    {
      foreach (IBaseOutputPlugin plugin in manager.OutputPlugins) 
      {
        if (plugin.IsSupported(filename)) 
        {          
          return plugin;
        }
      }
      return null;
    }

    public OutputTarget(PluginManager.PluginManager manager, string filename)
    {
      m_Plugin = SearchForSupportedPlugin(manager, filename);
      if (m_Plugin == null)
        throw new NotSupportedException("Output target: " + filename + " is not supported by any of the loaded output plugins.");

      m_Filename = filename;
      m_Plugin.Open(m_Filename);
    }

    public string Filename
    {
      get 
      {        
        return m_Filename;
      }
    }

    public IBaseOutputPlugin Plugin
    {
      get 
      {        
        return m_Plugin;
      }
    }
	}
}
