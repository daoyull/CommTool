const server = {

    /*
    * 已链接的客户端列表
    * ip:port
    * @type {Array<string>}
    * */
    connectList: [],

    /*
    * 已选择的客户端列表
    * ip:port
    * @type {Array<string>}
    * */
    selectList: [],
    /**
     * 发送数据
     * @param {string} address - 目标地址 ip:port
     * @param {string} message - 发送信息
     */
    send(address, message) {
    },

    /**
     * 发送数据
     * @param {string} address - 目标地址 ip:port
     * @param {Uint8Array} array - 发送信息
     */
    sendBuffer(address, array) {
    },

}