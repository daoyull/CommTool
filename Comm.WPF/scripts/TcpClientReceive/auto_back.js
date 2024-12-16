/*
* @function 
* @param message {SocketMessage} 
*   message.time --> js.Date 
*   message.data --> js.Uint8Array 
*   message.Socket --> .net Socket 
* @param client {TcpClient} 客户端 
*   client.send(string message)
*   client.sendBuffer(Uint8Array array)
* */
function receive(message, client) {
    // debugger
    // client.sendBuffer(message.data)
    // ui.logInfo(`脚本自动回发: ${utils.bufferToString(message.data)}`)
    
    // return {
    //     logHandle : true,
    //     frameHandle:true,
    // }
}


