using System;
using mkNETtools.PluginHelper;

namespace mkNETtools.Plugins.TestPlugin
{
	/// <summary>
	/// Summary description for TestPlugin.
	/// </summary>
	[PluginAttribute]
	public class TestPlugin : IBasePlugin
	{
		public TestPlugin()
		{

    }

    #region IBasePlugin Members

    public string PluginName
    {      
      get 
      {
        return "Test Plugin v1.0";
      }
    }

    #endregion
  }
}
