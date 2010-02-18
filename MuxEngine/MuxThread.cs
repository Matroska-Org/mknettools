using System;
using System.Collections;
using System.Threading;
using mkNETtools.PluginHelper;

namespace mkNETtools.MuxEngine
{
	/// <summary>
	/// Summary description for MuxThread.
	/// </summary>
	public class MuxThread
	{
    protected Thread m_Thread = null;
    protected bool m_bExit = false;
    protected ArrayList m_Input = null;
    protected OutputTarget m_Output = null;
    protected bool m_Complete = false;
    protected long m_FramesMuxed = 0;
    protected double m_StartTime = 0.0;
    protected double m_EndTime = 0.0;

		public MuxThread()
		{
			m_Thread = new Thread(new ThreadStart(this.ThreadProc));
      m_Thread.Name = "Muxing Thread";
		}

    public void Load(ArrayList input, OutputTarget output)
    {
      m_Input = input;
      m_Output = output;
    }

    public long FramesMuxed
    {
      get
      {
        return m_FramesMuxed;
      }
    }

    public double TimeElapsed
    {
      get
      {
        if (m_EndTime != 0.0) 
        {
          return m_EndTime - m_StartTime;
        } 
        else 
        {
          return 0.0 - m_StartTime;
        }
      }
    }

    public bool Complete
    {
      get 
      {
        return m_Complete;
      }
    }

    public void Start()
    {
      // Reset counters
      m_FramesMuxed = 0;
      m_StartTime = 0.0;
      m_EndTime = 0.0;

      // Setup the output plugin
      ArrayList outputTracks = new ArrayList();
      foreach (InputSource input in m_Input)
      {
        foreach (InputTrackInfo track in input.Tracks)
        {
          if (track.Enabled) 
          {
            outputTracks.Add(track.Track);
          }
        }
      }
      m_Output.Plugin.Tracks = (ITrackInfo [])outputTracks.ToArray(typeof(ITrackInfo));

      m_Thread.Start();
    }

    public void Stop()
    {
      m_bExit = true;
      if (m_Thread.Join(1000 * 30) == false)
        throw new ThreadInterruptedException("Muxing Thread failed to exit after 30 seconds");
    }

    private double LongestTrackDuration
    {
      get 
      {
        double duration = 0.0;
        foreach (InputSource input in m_Input)
        {
          foreach (InputTrackInfo track in input.Tracks)
          {
            if (track.Enabled) 
            {
              if (track.Track.Duration > duration)
                duration = track.Track.Duration;
            }
          }
        }
        return duration;
      }
    }
    /// <summary>
    /// Main processing loop
    /// </summary>
    protected void ThreadProc()
    {      
      m_Complete = false;
      try 
      {
        double totalDuration = this.LongestTrackDuration;
        double lastTimecode = 0.0;
        int lastPercent = -1;

        Console.Out.WriteLine("Muxing " + totalDuration + " seconds...");
        while (!m_bExit) 
        {
          bool bEOF = true;
          foreach (InputSource input in m_Input)
          {
            foreach (InputTrackInfo track in input.Tracks)
            {
              if (track.Enabled) 
              {
                Frame frame = input.Plugin.GetNextFrame(track.Track);
                if (frame != null) 
                {
                  m_Output.Plugin.WriteFrame(ref frame);
                  lastTimecode = frame.Timecode;
                  bEOF = false;
                  m_FramesMuxed++;
                }
              }
            }
          }
          int percent = (int)((100.0 / totalDuration) * lastTimecode);
          if (percent != lastPercent) 
          {
            Console.Out.Write(percent + "% ");
            lastPercent = percent;
          }
          //Thread.Sleep(10);
          if (bEOF)
            break;
        }
        Console.Out.WriteLine("");
        Console.Out.WriteLine("Muxed " + lastTimecode + " seconds...");
        // Close the output file
        m_Output.Plugin.Close();
      } 
      catch (Exception ex) 
      {
        Console.Error.WriteLine(ex);
        throw;
      }
      finally
      {
        m_Complete = true;
      }
    }
	}
}
