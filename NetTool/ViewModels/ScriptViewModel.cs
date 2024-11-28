using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetTool.ScriptManager.Interface;
using NetTool.Servcice;

namespace NetTool.ViewModels;

public partial class ScriptViewModel : ObservableObject
{
    private readonly IScriptManager _scriptManager;
    private readonly BlazorService _blazorService;

    public string Type { get; set; }
    public string? InitScriptContent { get; set; }


    [ObservableProperty] private string? _selectScriptName;
    [ObservableProperty] private string _addScriptName;


    partial void OnSelectScriptNameChanged(string? value)
    {
        _ = HandleSelectScriptNameChanged(value);
    }


    [ObservableProperty] private List<string> _scriptSource;

    private async Task HandleSelectScriptNameChanged(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _blazorService.Content = "";
            return;
        }

        var content = await _scriptManager.GetScriptContent(Type, value);
        _blazorService.Content = content;
    }

    [RelayCommand]
    public async Task AddScript()
    {
        if (string.IsNullOrEmpty(AddScriptName))
        {
            return;
        }

        var scriptNames = _scriptManager.GetScriptNames(Type);
        if (scriptNames.Contains(AddScriptName))
        {
            Refresh(AddScriptName);
            return;
        }

        await _scriptManager.EditScript(Type, AddScriptName, InitScriptContent ?? "");
        Refresh(AddScriptName);
    }

    [RelayCommand]
    public void DeleteScript()
    {
        if (string.IsNullOrEmpty(SelectScriptName))
        {
            return;
        }

        _scriptManager.RemoveScript(Type, SelectScriptName);
        Refresh();
    }

    private void SetScriptSource(List<string> source)
    {
        ScriptSource = source;
        SelectScriptName = null;
    }

    [RelayCommand]
    public void Refresh(string name = "")
    {
        SetScriptSource(_scriptManager.GetScriptNames(Type));
        if (string.IsNullOrEmpty(name))
        {
            if (ScriptSource.Count > 0)
            {
                SelectScriptName = ScriptSource[0];
            }
            else
            {
                _blazorService.Content = "";
            }
        }
        else
        {
            SelectScriptName = name;
        }
    }


    [RelayCommand]
    public async Task Save()
    {
        if (_blazorService.Editor != null && !string.IsNullOrEmpty(SelectScriptName))
        {
            var content = await _blazorService.Editor.GetValue();
            await _scriptManager.EditScript(Type, SelectScriptName, content);
            Refresh(SelectScriptName);
        }
    }


    public ScriptViewModel(IScriptManager scriptManager, BlazorService blazorService)
    {
        _scriptManager = scriptManager;
        _blazorService = blazorService;
    }
}