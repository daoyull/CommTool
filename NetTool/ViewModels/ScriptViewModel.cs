using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.ScriptManager.Interface;
using NetTool.ScriptManager.Service;

namespace NetTool.ViewModels;

public partial class ScriptViewModel : ObservableObject
{
    
    private readonly IScriptManager _scriptManager;

    public string Type { get; set; }
    

    [ObservableProperty] private string _selectScriptName;


    partial void OnSelectScriptNameChanged(string value)
    {
        _ = HandleSelectScriptNameChanged(value);
    }

    public Action<string>? ContentChanged { get; set; }

    private async Task HandleSelectScriptNameChanged(string value)
    {
        var content = await _scriptManager.GetScriptContent(Type, value);
        ContentChanged?.Invoke(content);
    }

    public Task Save(string content)
    {
        return Task.CompletedTask;
    }

    public ScriptViewModel()
    {
        //
    }

    public ScriptViewModel(IScriptManager scriptManager)
    {
        _scriptManager = scriptManager;
    }

    [ObservableProperty]
    private List<string> _scriptNames;
}