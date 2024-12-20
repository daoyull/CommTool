/**
 * 界面操作
 */
const ui = {
    /**
     * 打印普通信息。
     * @param {string} message - 要打印的信息内容。
     */
    logInfo(message) {
    },

    /**
     * 打印主要信息。
     * @param {string} message - 要打印的信息内容。
     */
    logPrimary(message) {
    },

    /**
     * 打印成功信息。
     * @param {string} message - 要打印的信息内容。
     */
    logSuccess(message) {
    },

    /**
     * 打印警告信息。
     * @param {string} message - 要打印的信息内容。
     */
    logWaring(message) {
    },

    /**
     * 打印错误信息。
     * @param {string} message - 要打印的信息内容。
     */
    logError(message) {
    },

    /**
     * 打印信息。
     * @param {string} message - 要打印的信息内容。
     * @param {string} color - 要打印的信息颜色 '#000000'。
     */
    log(message, color) {
    },

    /**
     * 打印空行。
     */
    logEmptyLine() {
    },

    /**
     * 清除界面消息。
     */
    clearLog() {
    },

    /**
     * 增加发送帧计数
     * @param {Number} num - 增加数
     */
    addSendFrame(num) {
    },

    /**
     * 增加接收帧计数
     * @param {Number} num - 增加数
     */
    addReceiveFrame(num) {
    },

    /**
     * 增加发送字节计数
     * @param {Number} num - 增加数
     */
    addSendBytes(num) {
    },

    /**
     * 增加接收字节计数
     * @param {Number} num - 增加数
     */
    addReceiveBytes(num) {
    },

    /**
     * 重置帧/字节计数
     */
    resetNumber() {
    },

    /**
     * 滚动到底部
     */
    scrollToEnd() {
    },
}

/**
 * 消息提示
 */
const notify = {
    /**
     * 普通信息提示。
     * @param {string} message - 要提示的信息内容。
     */
    info(message) {
    },

    /**
     * 成功信息提示。
     * @param {string} message - 要提示的信息内容。
     */
    success(message) {
    },

    /**
     * 警告信息提示。
     * @param {string} message - 要提示的信息内容。
     */
    warning(message) {
    },

    /**
     * 错误信息提示。
     * @param {string} message - 要提示的信息内容。
     */
    error(message) {
    },
}

/**
 * 通讯
 */
const comm = {
    /**
     * 发送数据
     * @param {string} message - 发送信息
     */
    send(message) {
    }
}

/**
 * 工具
 */
const util = {

    /**
     * 把 Uint8Array 转为字符串
     * @param {Uint8Array} array - byte数组
     * @param {Boolean} isHex - 是否转为16进制字符串
     * @returns {string} - byte数组对应的字符串
     */
    arrayToString(array, isHex = false) {
    },

    /**
     * 把 字符串 转为 Uint8Array
     * @param {string} message - 信息
     * @param {Boolean} isHex - 是否16进制字符串
     * @returns {Uint8Array} - 字符串对应的byte数组
     */
    stringToArray(message, isHex = false) {
    }
}


/**
 * .net数组转Uint8Array
 * @param {.net byte[]} array - .net数组
 * @returns {Uint8Array} - Uint8Array
 */
function arrayToUint8Array(array) {
}

/**
 * 格式化时间
 * @param {Date} date - 时间
 * @param {string} format - 格式化的格式 yyyy-MM-dd HH:mm:ss.S
 * @returns {string} - 格式化后的时间
 */
function formatDate(date, format = "yyyy-MM-dd HH:mm:ss:S") {
}

/**
 * 计算并附加Modbus CRC16校验码到原始字节数组上。
 * @param {Uint8Array} source - 原始字节数组
 * @returns {Uint8Array} - 包含CRC16校验码的新字节数组
 */
function modbusCrc16(source) {
}


