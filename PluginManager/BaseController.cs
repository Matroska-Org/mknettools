using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.IO;
using mkNETtools.PluginHelper;

namespace mkNETtools.PluginManager
{
  /// <summary>
  /// Summary description for BaseController.
  /// </summary>
  public abstract class BaseController
  {
    private PluginManager pm;
    private MarshalByRefObject[] objs;
    private AppDomain plugdom;
    private FileSystemWatcher fsw;

    protected BaseController(params MarshalByRefObject[] objs)
    {
      this.objs = objs;
      fsw = new FileSystemWatcher("./Plugins", "*.dll");
      fsw.NotifyFilter = NotifyFilters.LastWrite 
        | NotifyFilters.FileName | NotifyFilters.Size;

      fsw.Changed += new FileSystemEventHandler(FileHandling);
      fsw.Created += new FileSystemEventHandler(FileHandling);
      fsw.Deleted += new FileSystemEventHandler(FileHandling);
         
      Init();
    }

    public bool EnableFileSystemWatcher
    {
      get {return fsw.EnableRaisingEvents;}
      set 
      {
        if (fsw.EnableRaisingEvents != value)
        {
          fsw.EnableRaisingEvents = value;
        }
      }
    }

    private void FileHandling(object sender, FileSystemEventArgs e)
    {
      switch (e.ChangeType)
      {
        case WatcherChangeTypes.Changed:
        case WatcherChangeTypes.Created:
          Init();
          break;
        case WatcherChangeTypes.Deleted:
          Init();
          break;
        default:
          break;
      }
    }

    private void Init()
    {
      UnloadPluginManager();
         
      plugdom = AppDomain.CreateDomain("Plugins");
         
      string assname = typeof(PluginManager).Assembly.FullName;
      string typename = typeof(PluginManager).FullName;
      try 
      {           
        pm = plugdom.CreateInstanceAndUnwrap(assname, typename, 
          true, BindingFlags.Default, null, this.objs, null, null, null) as PluginManager;
      }
      catch (TargetInvocationException ex)
      {
        //fsw.EnableRaisingEvents = false;
        Console.WriteLine(ex.Message);
        UnloadPluginManager();
      }
    }

    public void LoadPlugin(string filename)
    {
      if (pm != null)
        pm.LoadPlugin(filename);
    }

    public void UnloadPlugin(string typename)
    {
      if (pm != null)
        pm.UnloadPlugin(typename);
    }

    public string[] PluginNames
    {
      get 
      {
        if (pm == null)
          return new string[0];
        else return pm.PluginNames;}
    }

    public void LoadPluginManager()
    {
      Init();
    }

    public void UnloadPluginManager()
    {
      if (pm != null)
      {
        try 
        {
          pm.Unload();
          if (plugdom != null)
            AppDomain.Unload(plugdom);
        }
        finally 
        {
          pm = null;
          plugdom = null;
        }
      }
    }
  }
}
