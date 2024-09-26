function registerTip() {
    monaco.languages.registerCompletionItemProvider('javascript', {
        triggerCharacters: ['engine', '.'],
        provideCompletionItems: function (model, position) {
            return {
                suggestions: [
                    {
                        label: 'UiLogger',
                        kind: monaco.languages.CompletionItemKind.Function,
                        documentation: '操作界面元素',
                        detail:'操作界面元素',
                        insertText: 'UiLogger',
                        insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    },
                    {
                        label: 'UiLogger.Message',
                        kind: monaco.languages.CompletionItemKind.Method,
                        documentation: '输出信息到界面(文本,颜色)',
                        detail: '输出信息到界面(文本,颜色)',
                        insertText: 'UiLogger.Message(\'${1:Hello World}\',\'${2:#202424}\');',
                        insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    },
                ]
            }
        }
    });
}