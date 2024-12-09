using System.IO.Ports;
using System.Text;

namespace NetTool.Unit;

public class SerialPortTest
{
    [Test]
    public async Task Receive()
    {
        var serialPort = new SerialPort();
        serialPort.PortName = "COM2";
        serialPort.BaudRate = 9600;
        serialPort.DataBits = 8;
        serialPort.Parity = Parity.None;
        serialPort.StopBits = StopBits.One;

        serialPort.DataReceived += HandleDataReceive;
        serialPort.Open();
        // var builder = new StringBuilder();
        for (int i = 0; i < 100_000; i++)
        {
            serialPort.Write(Guid.NewGuid().ToString());
            Thread.Sleep(2);
           
        }
        // serialPort.Write(builder.ToString());
    }

    private void HandleDataReceive(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender is SerialPort serialPort)
        {
            var serialPortReadBufferSize = serialPort.ReadBufferSize;
            int length = serialPort.BytesToRead;
            Console.WriteLine($"串口接收到{length}字节数据");
            if (length == 0)
            {
                return;
            }

            byte[] buffer = new byte[15535];
            var read = serialPort.Read(buffer, 0, 15535);
            
        }
    }
}