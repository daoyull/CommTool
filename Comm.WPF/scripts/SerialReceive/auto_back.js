/*
* 自动回发
* */
function receive(message, serial) {
    let timeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    // 接收日志输出到界面
    ui.logInfo(`[${utils.formatDate(message.time, timeFormat)}] Receive`);
    ui.logSuccess(serial.bufferToString(message.data));

    // 增加收到的帧/字节
    ui.addReceiveFrame(1)
    ui.addReceiveBytes(message.data.length)

    // 发送收到的数据
    serial.send(message.data)

    // 发送日志输出到界面
    ui.logInfo(`[${utils.formatDate(new Date(), timeFormat)}] Send`);
    ui.logPrimary(serial.bufferToString(message.data))

    // 增加收到的帧/字节
    ui.addSendFrame(1)
    ui.addSendBytes(message.data.length)

    return {
        logHandle: true,
        frameHandle: true
    }
}