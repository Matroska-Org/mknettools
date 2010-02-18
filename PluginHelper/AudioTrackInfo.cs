using System;

namespace mkNETtools.PluginHelper
{
	/// <summary>
	/// Summary description for AudioTrackInfo.
	/// </summary>
	public class AudioTrackInfo : TrackInfo, IAudioTrackInfo
	{
    private int m_Channels = 0;
    private int m_Bitdepth = 0;
    private double m_SamplingRate = 0.0;
    private double m_OutputSamplingRate = 0.0;

		public AudioTrackInfo()
		{
			//
			// TODO: Add constructor logic here
			//
    }

    /// <summary>
    /// Create a copy of the IAudioTrackInfo
    /// </summary>
    /// <param name="Track">The IAudioTrackInfo to copy the data from</param>
    /// <param name="ReadOnly">If true the copy with be read-only</param>
    public AudioTrackInfo(IAudioTrackInfo Track, bool ReadOnly)
      : base(Track, ReadOnly) 
    {
      this.Channels = Track.Channels;
      this.Bitdepth = Track.Bitdepth;
      this.SamplingRate = Track.SamplingRate;
      this.OutputSamplingRate = Track.OutputSamplingRate;
    }

    #region IAudioTrackInfo Members

    public int Channels
    {
      get
      {
        return m_Channels;
      }
      set
      {
        CheckReadOnly();        
        m_Channels = value;
      }
    }

    public int Bitdepth
    {
      get
      {
        return m_Bitdepth;
      }
      set
      {
        CheckReadOnly();        
        m_Bitdepth = value;
      }
    }

    public double SamplingRate
    {
      get
      {
        return m_SamplingRate;
      }
      set
      {
        CheckReadOnly();        
        m_SamplingRate = value;
      }
    }

    public double OutputSamplingRate
    {
      get
      {
        return m_OutputSamplingRate;
      }
      set
      {
        CheckReadOnly();        
        m_OutputSamplingRate = value;
      }
    }

    #endregion
  }
}
