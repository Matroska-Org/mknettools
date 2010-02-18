using System;

namespace mkNETtools.PluginHelper
{
	/// <summary>
	/// Summary description for VideoTrackInfo.
	/// </summary>
	public class VideoTrackInfo : TrackInfo, IVideoTrackInfo
	{
    private int m_Width = 0;
    private int m_Height = 0;
    private int m_DisplayWidth = 0;
    private int m_DisplayHeight = 0;
    private double m_AvgFramerate = 0.0;

		public VideoTrackInfo()
		{
			//
			// TODO: Add constructor logic here
			//
    }

    /// <summary>
    /// Create a copy of the IVideoTrackInfo
    /// </summary>
    /// <param name="Track">The IVideoTrackInfo to copy the data from</param>
    /// <param name="ReadOnly">If true the copy with be read-only</param>
    public VideoTrackInfo(IVideoTrackInfo Track, bool ReadOnly)
      : base(Track, ReadOnly) 
    {      
      this.Width = Track.Width;
      this.Height = Track.Height;
      this.DisplayWidth = Track.DisplayWidth;
      this.DisplayHeight = Track.DisplayHeight;
      this.AvgFramerate = Track.AvgFramerate;
    }

    #region IVideoTrackInfo Members

    public int Width
    {
      get
      {
        return m_Width;
      }
      set
      {
        CheckReadOnly();        
        m_Width = value;
      }
    }

    public int Height
    {
      get
      {
        return m_Height;
      }
      set
      {
        CheckReadOnly();        
        m_Height = value;
      }
    }

    public int DisplayWidth
    {
      get
      {
        return m_DisplayWidth;
      }
      set
      {
        CheckReadOnly();        
        m_DisplayWidth = value;
      }
    }

    public int DisplayHeight
    {
      get
      {
        return m_DisplayHeight;
      }
      set
      {
        CheckReadOnly();        
        m_DisplayHeight = value;
      }
    }

    public double AvgFramerate
    {
      get
      {
        return m_AvgFramerate;
      }
      set
      {
        CheckReadOnly();        
        m_AvgFramerate = value;
      }
    }

    #endregion
  }
}
