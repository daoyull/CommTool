/**
 * .net数组转Uint8Array
 * @param {.net byte[]} array - .net数组
 * @returns {Uint8Array} - Uint8Array
 */
function arrayToUint8Array(array) {
    return new Uint8Array(array)
}

/**
 * 格式化时间
 * @param {Date} date - 时间
 * @param {string} format - 格式化的格式 yyyy-MM-dd HH:mm:ss.S
 * @returns {string} - 格式化后的时间
 */
function formatDate(date, format = "yyyy-MM-dd HH:mm:ss:S") {
    const o = {
        "M+": date.getMonth() + 1, // 月份
        "d+": date.getDate(), // 日
        "h+": date.getHours() % 12 === 0 ? 12 : date.getHours() % 12, // 小时
        "H+": date.getHours(), // 小时
        "m+": date.getMinutes(), // 分
        "s+": date.getSeconds(), // 秒
        "q+": Math.floor((date.getMonth() + 3) / 3), // 季度
        S: date.getMilliseconds(), // 毫秒
        a: date.getHours() < 12 ? "上午" : "下午", // 上午/下午
        A: date.getHours() < 12 ? "AM" : "PM", // AM/PM
    };
    if (/(y+)/.test(format)) {
        format = format.replace(
            RegExp.$1,
            (date.getFullYear() + "").substr(4 - RegExp.$1.length)
        );
    }
    for (let k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(
                RegExp.$1,
                RegExp.$1.length === 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length)
            );
        }
    }
    return format;
}

/**
 * 计算并附加Modbus CRC16校验码到原始字节数组上。
 * @param {Uint8Array} source - 原始字节数组
 * @returns {Uint8Array} - 包含CRC16校验码的新字节数组
 */
function modbusCrc16(source) {
    // 创建一个比原数组多两个元素的数组来存储CRC值
    const newBuffer = new Uint8Array(source.length + 2);

    // 复制原始数据到新数组
    newBuffer.set(source);

    // 初始化CRC寄存器
    let crc = 0xFFFF;

    // 计算CRC16
    for (let i = 0; i < source.length; i++) {
        crc ^= source[i];
        for (let j = 0; j < 8; j++) {
            if (crc & 0x01) {
                crc = (crc >> 1) ^ 0xA001;
            } else {
                crc >>= 1;
            }
        }
    }

    // 将计算出的CRC添加到新数组末尾，注意字节顺序
    newBuffer[source.length] = crc & 0xFF;     // Low byte
    newBuffer[source.length + 1] = (crc >> 8); // High byte

    return newBuffer;
}