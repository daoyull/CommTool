using Comm.Lib.Interface;
using Comm.WPF.Abstracts;
using Comm.WPF.Views;

namespace Comm.WPF.Common;

public class ScriptDialogHelper
{
    public static void ShowDialog<T>(string scriptType, string initContent, AbstractCommViewModel<T> viewModel)
        where T : IMessage
    {
        var scriptManagerView = new ScriptManagerView();
        scriptManagerView.ShowDialog(scriptType, initContent);
        viewModel.RefreshScriptSource();
    }
}