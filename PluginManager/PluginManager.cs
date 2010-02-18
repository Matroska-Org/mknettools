using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.IO;
using mkNETtools.PluginHelper;

namespace mkNETtools.PluginManager
{
  //[Serializable]
  public class PluginManager : MarshalByRefObject
  {
    public override object InitializeLifetimeService()
    {
      //why does it it disconnect?
      return null;
    }

    private ArrayList plugins = new ArrayList();

    public PluginManager()
    {         
      string [] files;
      try 
      {
        
        files = Directory.GetFiles("." + Path.DirectorySeparatorChar + "Plugins", "mkNETtools.Plugins.*.dll");        
        foreach (string file in files)
        {
          LoadPlugin(file);
        }
      } 
      catch (DirectoryNotFoundException ex) 
      {
        // Just ignore this exception
        ex = null;
      }
      // Look in the current folder
      files = Directory.GetFiles("." + Path.DirectorySeparatorChar, "mkNETtools.Plugins.*.dll");
      foreach (string file in files)
      {
        LoadPlugin(file);
      }
    }

    public void Unload()
    {
      foreach (IBasePlugin plug in plugins)
      {
        //plug.Unload();
      }
      plugins.Clear();
    }

    public int PluginCount
    {
      get {return plugins.Count;}
    }

    private bool PluginsContains(Type type)
    {
      foreach (IBasePlugin plug in plugins)
      {
        if (plug.GetType() == type)
        {
          return true;
        }
      }
      return false;
    }

    public string [] PluginNames
    {
      get 
      {
        string[] output = new string[plugins.Count];
        for (int i = 0; i < plugins.Count; i++)
        {
          IBasePlugin plugin = (IBasePlugin)plugins[i];
          output[i] = plugin.PluginName;// + " : " + plugin.GetType().FullName;
        }
        return output;
      }
    }

    public IBasePlugin [] Plugins
    {
      get 
      {
        return (IBasePlugin [])plugins.ToArray(typeof(IBasePlugin));
      }
    }

    public IBaseInputPlugin [] InputPlugins
    {
      get 
      {
        ArrayList inputPlugins = new ArrayList();

        foreach (IBasePlugin plugin in plugins)
        {
          Type [] types = plugin.GetType().GetInterfaces();
          foreach (Type type in types) 
          {
            if (type == typeof(IBaseInputPlugin)) 
            {
              inputPlugins.Add(plugin);
              break;
            }
          }
        }

        return (IBaseInputPlugin [])inputPlugins.ToArray(typeof(IBaseInputPlugin));
      }
    }

    public IBaseOutputPlugin [] OutputPlugins
    {
      get 
      {
        ArrayList outputPlugins = new ArrayList();

        foreach (IBasePlugin plugin in plugins)
        {
          Type [] types = plugin.GetType().GetInterfaces();
          foreach (Type type in types) 
          {
            if (type == typeof(IBaseOutputPlugin)) 
            {
              outputPlugins.Add(plugin);
            }
          }
        }

        return (IBaseOutputPlugin [])outputPlugins.ToArray(typeof(IBaseOutputPlugin));
      }
    }

    public void UnloadPlugin(string typename)
    {
      for (int i = 0; i < plugins.Count; i++)
      {
        IBasePlugin plugin = (IBasePlugin)plugins;
        if (plugin.GetType().Name == typename)
        {
          //plugin.Unload();
          plugins.RemoveAt(i);
          return;
        }
      }
    }
      
    public void LoadPlugin(string filename)
    {
      Assembly pluginAssembly = null;
      try 
      {
        // the following replaces Assembly.LoadFrom()
        FileStream stream = File.OpenRead(filename);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int)stream.Length);
        stream.Close();

        byte[] debugbuffer = null;
        try 
        {
          //this wont show in VS.NET, but symbols IS in fact loaded
          stream = File.OpenRead(filename.Replace(".dll", ".pdb"));
          debugbuffer = new byte[stream.Length];
          stream.Read(debugbuffer, 0, (int)stream.Length);
          stream.Close();
        } 
        catch (IOException ex) 
        {
          // Couldn't load debug symbols
        }
        if (debugbuffer != null) 
        {
          pluginAssembly = Assembly.Load(buffer, debugbuffer);
        } 
        else 
        {
          pluginAssembly = Assembly.Load(buffer);
        }
      } 
      catch (IOException ex) 
      {
        // Failed to load the plugin from disk
        Console.Error.WriteLine(ex);
      }

      if (pluginAssembly == null) 
      {
        // Unable to the load the plugin
        return;
      }

      try 
      {
        Type[] types = pluginAssembly.GetExportedTypes();
        foreach (Type type in types)
        {
          if (!PluginsContains(type) && !type.IsAbstract)
          {
            Type[] ifaces = type.GetInterfaces();
            foreach (Type iface in ifaces)
            {
              if (iface == typeof(IBasePlugin) 
                & type.IsDefined(typeof(PluginAttribute),false))
              {
                //create and load the type, add to collection
                IBasePlugin iplug = (IBasePlugin) Activator.CreateInstance(type);
                //iplug.Load();
                plugins.Add(iplug);
              }
            }
          }
        }
      } 
      catch (TypeLoadException ex) 
      {
        // Possibly the plugin was compiled with an older set of interfaces
        Console.Error.WriteLine("Plugin interface load error, Possibly the plugin was compiled with an older set of interfaces?");
        Console.Error.WriteLine(ex);
      }
    }
  }
}
