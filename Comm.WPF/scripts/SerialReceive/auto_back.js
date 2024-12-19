/**
 * 处理接收到的消息。
 *
 * @param {Object} message - 接收的消息对象。
 * @param {Date} message.time - 消息的时间戳。
 * @param {Uint8Array} message.data - 消息的数据体。
 * @returns {SerialReceiveResult}
 */
function receive(message) {

    // 接收日志输出到界面
    ui.logInfo("-------Script Start-------")
    ui.logInfo(`[${formatDate(message.time)}] Receive`);
    ui.logSuccess(util.arrayToString(message.data));
    
    // 发送收到的数据
    serial.sendBuffer(message.data);

    // 发送日志输出到界面
    ui.logInfo(`[${formatDate(new Date())}] Send`);
    ui.logPrimary(util.arrayToString(message.data));

    ui.logInfo("-------Script End-------");
    ui.logEmptyLine();
    
    let result = new SerialReceiveResult();
    result.logHandle = true;
    return result;
}

/**
 * 接收脚本返回结果
 */
class SerialReceiveResult {

    /**
     * 是否处理了界面日志
     * 为true程序中不再处理界面日志
     */
    logHandle = false

    /**
     * 是否处理了帧/字节增加
     * 为true程序中不再处理帧/字节增加逻辑
     */
    frameHandle = false
}