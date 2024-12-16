using Comm.Lib.Interface;
using Comm.WPF.Abstracts;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Entity;

public class JsUtils<T> where T : IMessage
{
    public AbstractCommViewModel<T> ViewModel { get; }
    public V8ScriptEngine Engine { get; }

    public JsUtils(AbstractCommViewModel<T> viewModel, V8ScriptEngine engine)
    {
        ViewModel = viewModel;
        Engine = engine;
    }

    public string bufferToString(ITypedArray<byte> data) => ViewModel.BufferToString(data.ToArray());
}