using System.IO;
using System.Windows;
using Comm.Lib.Interface;
using Comm.WPF.Abstracts;
using Comm.WPF.Views;

namespace Comm.WPF.Common;

public class ScriptDialogHelper
{
    public static void ShowDialog<T>(string scriptType, string initContent, AbstractCommViewModel<T> viewModel)
        where T : IMessage
    {
        var extendTipName = scriptType + "Tip.js";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "scripts", "common", extendTipName);
        string extendTip = File.Exists(filePath) ? File.ReadAllText(filePath) : "";
        var scriptManagerView = new ScriptManagerView();
        scriptManagerView.ShowDialog(scriptType, initContent, extendTip);
        viewModel.RefreshScriptSource();
    }
}