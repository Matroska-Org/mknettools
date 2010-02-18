using System;

namespace mkNETtools.PluginHelper
{
  /// <summary>
  /// The base plugin interface
  /// </summary>
  public interface IBasePlugin
  {
    /// <summary>
    /// Load the plugin, alloc any resources needed
    /// </summary>
    //void Load();
    /// <summary>
    /// Unload the plugin, freeing any resources
    /// </summary>
    //void Unload();
    /// <summary>
    /// The plugin name
    /// </summary>
    string PluginName { get; }
  }
   
  /// <summary>
  /// Basic file plugin interface
  /// </summary>
  public interface IBaseFilePlugin : IBasePlugin
  {
    /// <summary>
    /// Open a file for reading
    /// </summary>
    /// <param name="filename">The file to open</param>
    void Open(string filename);
    /// <summary>
    /// Close the currently open file
    /// </summary>
    void Close(); 
    /// <summary>
    /// Detect if the passed file is supported
    /// </summary>
    /// <param name="filename">The file to check</param>
    /// <returns>Returns true is the file is supported</returns>
    bool IsSupported(string filename);
    /// <summary>
    /// The default file extension(s)
    /// </summary>
    /// <remarks>If there are more than one use |'s to separate them</remarks>
    string DefaultExt { get; }
  }

  /// <summary>
  /// Track infomation interface
  /// </summary>
  public interface ITrackInfo
  {
    int Number { get; set; }
    string Name { get; set; }
    string Language { get; set; }
    string CodecID { get; set; }
    byte [] CodecPrivate { get; set; }
    double Duration { get; set; }
  }
  
  /// <summary>
  /// Audio Track infomation interface
  /// </summary>
  public interface IAudioTrackInfo : ITrackInfo
  {
    int Channels { get; set; }
    int Bitdepth { get; set; }
    double SamplingRate { get; set; }
    double OutputSamplingRate { get; set; }
  }
  
  /// <summary>
  /// Video Track infomation interface
  /// </summary>
  public interface IVideoTrackInfo : ITrackInfo
  {
    int Width { get; set; }
    int Height { get; set; }
    int DisplayWidth { get; set; }
    int DisplayHeight { get; set; }
    double AvgFramerate { get; set; }
  }
  
  /// <summary>
  /// Frame struct
  /// </summary>
  public class Frame
  {
    public ITrackInfo Track;
    public double Timecode;
    public double Duration;
    public byte [] Data;
    public double [] References;
  }

  /// <summary>
  /// Basic input plugin interface
  /// </summary>
  public interface IBaseInputPlugin : IBaseFilePlugin
  {
    /// <summary>
    /// The source tracks for the currently open input file.
    /// </summary>
    /// <remarks>
    /// The array is read-only. Trying to set a property 
    /// will result in a MemberAccessException being thrown.
    /// </remarks>
    ITrackInfo [] Tracks { get; }
    /// <summary>
    /// Get the next frame for a track
    /// </summary>
    /// <param name="Track">The track to get the frame from.</param>
    /// <returns>Returns the next frame. null on EOS.</returns>
    Frame GetNextFrame(ITrackInfo Track);
  }

  /// <summary>
  /// Basic output plugin interface
  /// </summary>
  public interface IBaseOutputPlugin : IBaseFilePlugin
  {      
    ITrackInfo [] Tracks { set; }
    void WriteFrame(ref Frame frame);
  }

  /// <summary>
  /// Attribute to mark a exported class to treated as a plugin
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class PluginAttribute : Attribute{}
}
