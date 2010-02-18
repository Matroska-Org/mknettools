using System;
using System.Collections;
using mkNETtools.PluginManager;

namespace mkNETtools.MuxEngine
{
	/// <summary>
	/// Summary description for MuxEngine.
	/// </summary>
	public class MuxEngine
	{
    PluginManager.PluginManager m_Manager = null;
    protected ArrayList m_Input = new ArrayList();
    protected string m_OutputFilename = "";
    protected OutputTarget m_Output = null;
    protected MuxThread m_Thread = new MuxThread();

		public MuxEngine(PluginManager.PluginManager manager)
		{
			m_Manager = manager;
		}

    public void AddInput(string filename) 
    {
      InputSource input = new InputSource(m_Manager, filename);
      AddInput(input);
    }

    public void AddInput(InputSource input) 
    {
      m_Input.Add(input);
    }

    public string OutputFilename
    {
      get
      {
        return m_OutputFilename;
      }
      set
      { 
        m_Output = new OutputTarget(m_Manager, value);
        m_OutputFilename = value;        
      }
    }

    public long FramesMuxed
    {
      get
      {
        return m_Thread.FramesMuxed;
      }
    }

    public double TimeElapsed
    {
      get
      {
        return m_Thread.TimeElapsed;
      }
    }

    public bool Complete
    {
      get 
      {
        return m_Thread.Complete;
      }
    }

    public void Start()
    {
      m_Thread.Load(m_Input, m_Output);
      m_Thread.Start();
    }

    public void Stop()
    {
      m_Thread.Stop();
    }
	}
}
