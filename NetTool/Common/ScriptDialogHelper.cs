using NetTool.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Views;

namespace NetTool.Common;

public class ScriptDialogHelper
{
    public static void ShowDialog<T>(string scriptType, string initContent, AbstractNetViewModel<T> viewModel)
        where T : IMessage
    {
        var scriptManagerView = new ScriptManagerView();
        scriptManagerView.ShowDialog(scriptType, initContent);
        viewModel.RefreshScriptSource();
    }
}