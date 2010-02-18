using System;
using mkNETtools.PluginHelper;

namespace mkNETtools.MuxEngine
{
	/// <summary>
	/// Summary description for InputTrackInfo.
	/// </summary>
	public class InputTrackInfo
	{
    protected bool m_Enabled = true;
    protected ITrackInfo m_Track = null;

		public InputTrackInfo(ITrackInfo track)
		{
      m_Track = track;
		}

    public bool Enabled 
    {
      get 
      {
        return m_Enabled;
      }
      set
      {
        m_Enabled = value;
      }
    }

    public ITrackInfo Track 
    {
      get 
      {
        return m_Track;
      }
    }
	}
}
