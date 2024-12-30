/**
 * 处理发送的消息。
 * TcpServer Send Template
 * Add Modbus Crc16
 * @param {Object} message - 发送的消息对象。
 * @param {Date} message.time - 消息时间。
 * @param {Uint8Array} message.data - 消息的数据体。
 * @returns {TcpServerSendResult}
 */
function send(message) {
    debugger;

    ui.logInfo("-------Script Start-------")
    // 要发送的数据
    let sendBuffer = modbusCrc16(message.data)
    for (let address of server.selectList) {
        // 输出界面信息
        ui.addSendFrame(1);
        ui.addSendBytes(sendBuffer.length);
        ui.logInfo(`[${formatDate(new Date())}] [Send --> ${address}]`)
        ui.logPrimary(util.arrayToString(sendBuffer, true))
        // 发送数据
        server.sendBuffer(address, sendBuffer)
    }
    ui.logInfo("-------Script End-------")
    ui.logEmptyLine();

    // 返回结果
    let result = new TcpServerSendResult();
    result.logHandle = true;
    result.frameHandle = true;
    result.sendHandle = true;
    return result;
}

/**
 * 接收脚本返回结果
 */
class TcpServerSendResult {

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


    /**
     * 是否处理了发送数据
     * 为true程序中不再处理发送数据
     */
    sendHandle = false
}