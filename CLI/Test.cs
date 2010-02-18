/*
* Test class: main testing class
*
* Authors:		R. LOPES
* Contributors:	R. LOPES
* Created:		28 October 2002
* Modified:		28 October 2002
*
* Version:		1.0
*/

using System;
using System.Threading;
using mkNETtools.PluginManager;
using mkNETtools.MuxEngine;

namespace mkNETtools.CLI 
{
  /// <summary>
  /// Testing class
  /// </summary>
  class Test 
  {
    /// <summary>
    /// Main loop
    /// </summary>
    [STAThread]
    static void Main(string[] Args)
    {
      // Command line parsing
      ArgumentProcessor CommandLine = new ArgumentProcessor(Args);

      string inputFilename;
      string outputFilename;
      
      // Look for specific arguments values and display them if they exist (return null if they don't)
      inputFilename = CommandLine["i"];
      if (CommandLine["i"] != null)
        Console.WriteLine("i value: " + CommandLine["i"]);
      else 
        Console.WriteLine("i not defined !");
			
      outputFilename = CommandLine["o"];
      if (CommandLine["o"] != null) 
        Console.WriteLine("o value: " + CommandLine["o"]);
      else 
        Console.WriteLine("o not defined !");

      PluginManager.PluginManager manager = new PluginManager.PluginManager();
      foreach (string name in manager.PluginNames) 
      {
        Console.Out.WriteLine("Loaded Plugin " + name);
      }
      MuxEngine.MuxEngine engine = new MuxEngine.MuxEngine(manager);
      engine.AddInput(inputFilename);
      engine.OutputFilename = outputFilename;
      engine.Start();

      while (engine.Complete == false) 
      {
        Thread.Sleep(100);
      }

      Console.Out.WriteLine("Muxing is complete.");
      Console.Out.WriteLine("Frames Muxed: " + engine.FramesMuxed);
      Console.Out.WriteLine("Time Elapsed: " + engine.TimeElapsed);

      // Wait for key
      Console.Out.WriteLine("Press any key...");
      Console.Read();
    }
  }
}
