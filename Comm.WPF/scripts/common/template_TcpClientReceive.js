/**
 * 处理接收到的消息。
 * TcpClient Receive Template
 * 自动回发
 *
 * @param {Object} message - 接收的消息对象。
 * @param {Date} message.time - 消息的时间戳。
 * @param {Uint8Array} message.data - 消息的数据体。
 * @param {String} message.ip - 对方ip
 * @param {Number} message.port - 对方端口
 * @returns {TcpClientReceiveResult}
 */
function receive(message) {

    // 接收日志输出到界面
    ui.logInfo("-------Script Start-------")
    ui.logInfo(`[${formatDate(message.time)}] Receive`);
    ui.logSuccess(util.arrayToString(message.data));

    // 发送日志输出到界面
    ui.logInfo(`[${formatDate(new Date())}] Send`);
    ui.logPrimary(util.arrayToString(message.data));
    ui.addSendFrame(1);
    ui.addSendBytes(message.data.length);

    // 发送收到的数据
    client.sendBuffer(message.data);

    ui.logInfo("-------Script End-------");
    ui.logEmptyLine();

    let result = new TcpClientReceiveResult();
    result.logHandle = true;
    return result;
}

/**
 * 接收脚本返回结果
 */
class TcpClientReceiveResult {

    /**
     * 是否处理了接收界面日志
     * 为true程序中不再处理本次界面日志
     */
    logHandle = false

    /**
     * 是否处理了接收帧/字节计数
     * 为true程序中不再处理本次接收帧/字节计数增加逻辑
     */
    frameHandle = false
}