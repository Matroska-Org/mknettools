using System;
using System.IO;

using mkNETtools.PluginHelper;

namespace mkNETtools.Plugins.MpaPlugin
{
	/// <summary>
	/// Summary description for MpaOutput.
	/// </summary>
	[PluginAttribute]
	public class MpaOutput : IBaseOutputPlugin
	{
    protected FileStream m_Output = null;

		public MpaOutput()
		{
			//
			// TODO: Add constructor logic here
			//
		}

    #region IBaseOutputPlugin Members

    public ITrackInfo [] Tracks
    {
      set 
      {
        ITrackInfo [] tracks = value;
        if (tracks.Length != 1)
          throw new ArgumentException("Mpa only supports one track!");
            
        try 
        {
          IAudioTrackInfo track = (IAudioTrackInfo)tracks[0];
        
          if (track.CodecID == CodecIDs.AUDIO_MPEG_LAYER3) 
          {

          }
          else if (track.CodecID == CodecIDs.AUDIO_MPEG_LAYER2 || track.CodecID == CodecIDs.AUDIO_MPEG_LAYER1) 
          {
            
          }
          else
          {
            throw new ArgumentException("CodecID: " + track.CodecID + " is not supported for mpa output.");
          }
        } 
        catch (InvalidCastException ex) 
        {
          throw new ArgumentException("Mpa only supports an audio track.", ex);
        }     
      }
    }

    public void WriteFrame(ref Frame frame)
    {
      // Right now we just ignore timecodes and write the sample data out directly
      // Prehaps later we could add some padding code if some samples are dropped
      // If samples are overlapping that's just too bad.
      m_Output.Write(frame.Data, 0, frame.Data.Length);
    }

    #endregion

    #region IBaseFilePlugin Members

    public string DefaultExt
    {
      get
      {
        return "mpa|mp3|mp2|mp1";
      }
    }

    public void Close()
    {
      m_Output.Close();
      m_Output = null;
    }

    public void Open(string filename)
    {
      m_Output = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
      // Tuncate the file
      m_Output.SetLength(0);
    }

    public bool IsSupported(string filename)
    {
      string ext = Path.GetExtension(filename).ToLower();

      if (ext.CompareTo(".mp3") == 0)
        return true;

      if (ext.CompareTo(".mp2") == 0)
        return true;

      if (ext.CompareTo(".mp1") == 0)
        return true;
      
      if (ext.CompareTo(".mpa") == 0)
        return true;

      return false;
    }

    #endregion

    #region IBasePlugin Members

    public string PluginName
    {
      get 
      {
        return "Mpa Output Plugin v1.0";
      }
    }

    #endregion
	}
}
