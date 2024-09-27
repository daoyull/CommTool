using LanguageExt;
using NetTool.Lib.Interface;

namespace NetTool.Models;

public class GlobalOption : IGlobalOption
{
    public int BufferSize { get; set; } = 1021;
    public string ScriptPath { get; set; } = AppContext.BaseDirectory;
}