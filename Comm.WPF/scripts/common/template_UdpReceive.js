/**
 * 处理接收到的消息。
 * Udp Receive Template
 * 自动回发
 *
 * @param {Object} message - 接收的消息对象。
 * @param {Date} message.time - 消息的时间戳。
 * @param {Uint8Array} message.data - 消息的数据体。
 * @param {String} message.ip - 对方ip
 * @param {Number} message.port - 对方端口
 * @returns {UdpReceiveResult}
 */
function receive(message) {

    debugger;
    // 接收日志输出到界面
    var remoteIp = `${message.ip}:${message.port}`;
    ui.logInfo("-------Script Start-------")
    ui.logInfo(`[${formatDate(message.time)}] [Receive <-- [{remoteIp}] `);
    ui.logSuccess(util.arrayToString(message.data));

    // 发送日志输出到界面
    ui.logInfo(`[${formatDate(new Date())}] [Send --> ${remoteIp}]`);
    ui.logPrimary(util.arrayToString(message.data));
    ui.addSendFrame(1);
    ui.addSendBytes(message.data.length);

    // 发送收到的数据
    udp.sendBuffer(remoteIp,message.data);

    ui.logInfo("-------Script End-------");
    ui.logEmptyLine();

    let result = new UdpReceiveResult();
    result.logHandle = true;
    return result;
}

/**
 * 接收脚本返回结果
 */
class UdpReceiveResult {

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