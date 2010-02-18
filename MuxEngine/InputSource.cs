using System;
using System.Collections;
using mkNETtools.PluginManager;
using mkNETtools.PluginHelper;

namespace mkNETtools.MuxEngine
{
	/// <summary>
	/// Summary description for InputSource.
	/// </summary>
	public class InputSource
	{
    protected string m_Filename = "";
    protected IBaseInputPlugin m_Plugin = null;
    protected ArrayList m_Tracks = new ArrayList();

    private IBaseInputPlugin SearchForSupportedPlugin(PluginManager.PluginManager manager, string filename)
    {
      foreach (IBaseInputPlugin plugin in manager.InputPlugins) 
      {
        if (plugin.IsSupported(filename)) 
        {          
          return plugin;
        }
      }
      return null;
    }

    private void FillTrackArray()
    {
      // Make sure the array is empty
      m_Tracks.Clear();
      m_Plugin.Open(m_Filename);
      foreach (ITrackInfo track in m_Plugin.Tracks)
      {
        m_Tracks.Add(new InputTrackInfo(track));
      }
    }

		public InputSource(PluginManager.PluginManager manager, string filename)
		{
      m_Plugin = SearchForSupportedPlugin(manager, filename);
      if (m_Plugin == null)
        throw new NotSupportedException("Input source: " + filename + " is not supported by any of the loaded input plugins.");

      m_Filename = filename;
      FillTrackArray();
		}

    public string Filename
    {
      get 
      {        
        return m_Filename;
      }
    }

    public IBaseInputPlugin Plugin
    {
      get 
      {        
        return m_Plugin;
      }
    }

    public InputTrackInfo [] Tracks
    {
      get 
      {   
        return (InputTrackInfo [])m_Tracks.ToArray(typeof(InputTrackInfo));
      }
    }
	}
}
