/*
* 发送数据
* return 源数据是否继续发送 true: 继续发送 false: 不发送
* */
function doSend(sendBuffer) {
    let buffer = Array.from(sendBuffer);
    buffer.append(12);
    return false;
}

function received(receiveBuffer) {

}


class SendConfig {

    // 字段
    sourceSend = false;

}

class ReceiveConfig {
    
}