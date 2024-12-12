@REM 删除已发布的文件夹
if exist "publish" (
      rmdir /S /Q "publish"
)
@REM 打包Serial
cd SerialPort
dotnet publish -r win-x64 -c Release  -o "../publish"
cd ..

cd TcpClient
dotnet publish -r win-x64 -c Release  -o "../publish"
cd ..

cd TcpServer
dotnet publish -r win-x64 -c Release  -o "../publish"
cd ..

cd Udp
dotnet publish -r win-x64 -c Release  -o "../publish"
cd ..

