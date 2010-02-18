using System;

namespace mkNETtools.PluginHelper
{
	/// <summary>
	/// Summary description for TrackInfo.
	/// </summary>
	public class TrackInfo : ITrackInfo
	{
    private bool m_ReadOnly = false;
    private int m_Number = 0;
    private string m_Name = "";
    private string m_Language = "";
    private string m_CodecID = "";
    private byte [] m_CodecPrivate = new byte[0];

    private double m_Duration = 0.0;

		public TrackInfo()
		{
			
    }

    /// <summary>
    /// Create a copy of the ITrackInfo
    /// </summary>
    /// <param name="Track">The ITrackInfo to copy the data from</param>
    /// <param name="ReadOnly">If true the copy with be read-only</param>
    public TrackInfo(ITrackInfo Track, bool ReadOnly)
    {
      m_ReadOnly = ReadOnly;
      this.Number = Track.Number;
      this.Name = Track.Name;
      this.Language = Track.Language;
      this.CodecID = Track.CodecID;
      this.CodecPrivate = Track.CodecPrivate;
      this.Duration = Track.Duration;      
    }

    protected void CheckReadOnly()
    {
      if (m_ReadOnly) 
      {
        throw new MemberAccessException("Property in Read-Only mode");
      }
    }
    #region ITrackInfo Members

    public int Number
    {
      get
      {
        return m_Number;
      }
      set
      {
        CheckReadOnly();        
        m_Number = value;
      }
    }

    public string Name
    {
      get
      {
        return m_Name;
      }
      set
      {
        CheckReadOnly();
        if (value == null)
          throw new ArgumentNullException("Name", "Name cannot be a null reference. Set a 0 length string instead.");
        m_Name = value;
      }
    }

    public string Language
    {
      get
      {
        return m_Language;
      }
      set
      {
        CheckReadOnly();
        if (value == null)
          throw new ArgumentNullException("Name", "Name cannot be a null reference. Set a 0 length string instead.");
        m_Language = value;
      }
    }

    public string CodecID 
    { 
      get
      {
        return m_CodecID;
      }
      set
      {
        CheckReadOnly();
        if (value == null)
          throw new ArgumentNullException("CodecID", "CodecID cannot be a null reference. Set a 0 length string instead.");
        m_CodecID = value;
      }
    }

    public byte [] CodecPrivate 
    { 
      get
      {
        return m_CodecPrivate;
      }
      set
      {
        CheckReadOnly();
        if (value == null)
          throw new ArgumentNullException("CodecPrivate", "CodecPrivate cannot be a null reference. Set a 0 sized byte array instead.");
        m_CodecPrivate = value;
      }
    }

    public double Duration
    {
      get
      {
        return m_Duration;
      }
      set
      {
        CheckReadOnly();
        m_Duration = value;
      }
    }

    #endregion
  }
}
