using System;
using System.IO;
using mkNETtools.PluginHelper;
using org.ebml.matroska;
using org.ebml.io;

namespace mkNETtools.Plugins.MatroskaInput
{
	/// <summary>
	/// Summary description for MatroskaInput Plugin.
	/// </summary>
	[PluginAttribute]
	public class MatroskaInput : IBaseInputPlugin
	{
    protected MatroskaFile m_File = null;
    protected DataSource m_DataSource = null;

		public MatroskaInput()
		{
			//
			// TODO: Add constructor logic here
			//
    }
    #region IBaseInputPlugin Members

    public Frame GetNextFrame(ITrackInfo Track)
    {
      MatroskaFileFrame mFrame = m_File.getNextFrame(Track.Number);
      // Check for EOS
      if (mFrame == null)
        return null;

      Frame frame = new Frame();
      frame.Track = Track;
      frame.Timecode = (mFrame.Timecode / 1000.0);
      frame.Duration = (mFrame.Duration / 1000.0);
      
      int referenceCount = 1;
      if (mFrame.References != null)
        referenceCount += mFrame.References.Length;

      frame.References = new double[referenceCount];
      frame.References[0] = (mFrame.Reference / 1000.0);
      for (int i = 1; i < referenceCount; i++)
        frame.References[i] = (mFrame.References[i] / 1000.0);

      frame.Data = ArrayCopy.SByteToByte(mFrame.Data);

      return frame;
    }

    public ITrackInfo [] Tracks
    {
      get 
      {
        MatroskaFileTrack [] mTrackList = m_File.getTrackList();
        ITrackInfo [] tracks = new ITrackInfo[mTrackList.Length];
        for (int i = 0; i < mTrackList.Length; i++)
        {
          MatroskaFileTrack mTrack = mTrackList[i];
          if (mTrack.TrackType == MatroskaDocType.track_audio) 
          {
            AudioTrackInfo aTrack = new AudioTrackInfo();
          
            aTrack.Channels = mTrack.Audio_Channels;
            aTrack.Bitdepth = mTrack.Audio_BitDepth;
            aTrack.SamplingRate = mTrack.Audio_SamplingFrequency;
            aTrack.OutputSamplingRate = mTrack.Audio_OutputSamplingFrequency;

            tracks[i] = aTrack;
          }
          else if (mTrack.TrackType == MatroskaDocType.track_video) 
          {
            VideoTrackInfo vTrack = new VideoTrackInfo();
          
            vTrack.DisplayHeight = mTrack.Video_DisplayHeight;
            vTrack.DisplayWidth = mTrack.Video_DisplayWidth;
            vTrack.Height = mTrack.Video_PixelHeight;
            vTrack.Width = mTrack.Video_PixelWidth;

            tracks[i] = vTrack;
          }
          else 
          {
            tracks[i] = new TrackInfo();        
          }
          ITrackInfo track = tracks[i];        
          track.Number = mTrack.TrackNo;
          
          // Copy the track meta-data
          if (mTrack.Name != null)
            track.Name = mTrack.Name;
          if (mTrack.Language != null)
            track.Language = mTrack.Language;

          track.CodecID = mTrack.CodecID;
          if (mTrack.CodecPrivate != null) 
          {
            track.CodecPrivate = ArrayCopy.SByteToByte(mTrack.CodecPrivate);
          }
          // Currently all Matroska tracks share the file duration
          track.Duration = (m_File.getDuration() / 1000.0);
        }
        return tracks;
      }
    }

    #endregion

    #region IBaseFilePlugin Members

    public string DefaultExt
    {
      get
      {
        return "mkv";
      }
    }

    public void Close()
    {
      m_File = null;
      m_DataSource = null;
    }

    public void Open(string filename)
    {
      m_DataSource = new FileDataSource(filename, "r");
      m_File = new MatroskaFile(m_DataSource);
      m_File.readFile();
    }

    public bool IsSupported(string filename)
    {
      string ext = Path.GetExtension(filename).ToLower();

      if (ext.CompareTo(".mkv") == 0)
        return true;
      if (ext.CompareTo(".mka") == 0)
        return true;
      if (ext.CompareTo(".mks") == 0)
        return true;
      
      return false;
    }

    #endregion

    #region IBasePlugin Members

    public string PluginName
    {
      get 
      {
        return "Matroska Input Plugin v1.0";
      }
    }

    #endregion
  }
}
