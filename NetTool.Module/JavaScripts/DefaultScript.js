// socket.Send(byte[]);
// ui.Logger("","#ffffff")

function doSend(sendBuffer) {
    let buffer = Array.from(sendBuffer);
    buffer.append(12);
    return {};
}

function onReceive(receiveBuffer) {
    Console.WriteLine(ByteHelper.ToUtf8Str(receiveBuffer));
    // 自动回发
    socket.Send(receiveBuffer);
    return {}
}


class SendConfig {

    // 字段
    sourceSend = false;

}

class ReceiveConfig {
    
}