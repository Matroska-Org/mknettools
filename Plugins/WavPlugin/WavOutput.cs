using System;
using System.IO;
using mkNETtools.PluginHelper;
using WavLib;

namespace mkNETtools.Plugins.WavPlugin
{
	/// <summary>
	/// Summary description for WavInput.
	/// </summary>
	[PluginAttribute]
	public class WavOutput : IBaseOutputPlugin
	{
    protected WavWriter m_Writer = null;
    protected string m_Filename = "";
    protected WaveFormatEx m_Wfx = new WaveFormatEx();

		public WavOutput()
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
          throw new ArgumentException("Wav only supports one track!");
            
        try 
        {
          IAudioTrackInfo track = (IAudioTrackInfo)tracks[0];
        
          m_Wfx = new WaveFormatEx();
          m_Wfx.nChannels = (short)track.Channels;
          m_Wfx.wBitsPerSample = (short)track.Bitdepth;
          // Should I use the sampling rate or ouptut sampling rate?
          m_Wfx.nSamplesPerSec = (int)track.SamplingRate;
          //m_Wfx.nSamplesPerSec = (int)track.OutputSamplingRate;
          m_Wfx.cbSize = 0;

          if (track.CodecID == CodecIDs.AUDIO_ACM) 
          {
            MemoryStream memStream = new MemoryStream(track.CodecPrivate, 0, track.CodecPrivate.Length, false);
            BinaryReader binReader = new BinaryReader(memStream);
            m_Wfx.Read(binReader, track.CodecPrivate.Length);
          }
          else if (track.CodecID == CodecIDs.AUDIO_MPEG_LAYER3) 
          {
            m_Wfx.wFormatTag = (short)WaveFormatEx.WAVE_FORMAT_MPEG_LAYER3;
            if (track.CodecPrivate.Length > 0) 
            {
              int wfxSize = m_Wfx.GetSize();
              m_Wfx.cbSize = (short)(track.CodecPrivate.Length - wfxSize);
              m_Wfx.cbData = new byte[m_Wfx.cbSize];
              Array.Copy(track.CodecPrivate, wfxSize, m_Wfx.cbData, 0, m_Wfx.cbSize);
            }
          }
          else if (track.CodecID == CodecIDs.AUDIO_MPEG_LAYER2 || track.CodecID == CodecIDs.AUDIO_MPEG_LAYER1) 
          {
            m_Wfx.wFormatTag = (short)WaveFormatEx.WAVE_FORMAT_MPEG_LAYER12;
          }
          else
          {
            throw new ArgumentException("CodecID: " + track.CodecID + " is not supported for wav output.");
          }
        } 
        catch (InvalidCastException ex) 
        {
          throw new ArgumentException("Wav only supports an audio track.", ex);
        }
        // Now we can open the file
        m_Writer.Open(m_Filename, m_Wfx);        
      }
    }

    public void WriteFrame(ref Frame frame)
    {
      // Right now we just ignore timecodes and write the sample data out directly
      // Prehaps later we could add some padding code if some samples are dropped
      // If samples are overlapping that's just too bad.
      m_Writer.WriteSampleData(ref frame.Data, 0, frame.Data.Length);
    }

    #endregion

    #region IBaseFilePlugin Members

    public string DefaultExt
    {
      get
      {
        return "wav";
      }
    }

    public void Close()
    {
      m_Writer = null;
    }

    public void Open(string filename)
    {
      m_Writer = new WavWriter();  
      m_Writer.WriteFactChunk = false;
      m_Filename = filename;
    }

    public bool IsSupported(string filename)
    {
      string ext = Path.GetExtension(filename).ToLower();

      if (ext.CompareTo(".wav") == 0)
        return true;
      
      return false;
    }

    #endregion

    #region IBasePlugin Members

    public string PluginName
    {
      get 
      {
        return "Wav Output Plugin v1.0";
      }
    }

    #endregion
	}
}
