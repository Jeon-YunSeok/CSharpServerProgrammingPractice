using System.Net;
using System.Net.Sockets;
using System.Text;

// 1. 서버가 사용할 IP 주소와 포트 설정
IPAddress ipAddress = IPAddress.Loopback;
int port = 5000;

// 2. TCP 서버 생성
TcpListener listener = new TcpListener(ipAddress, port);

// 3. 서버 시작
listener.Start();

Console.WriteLine($"Server started: {ipAddress}:{port}");

// 4. 클라이언트 접속을 반복해서 받음
while (true)
{
    Console.WriteLine("Waiting for client...");

    using TcpClient client = listener.AcceptTcpClient();

    Console.WriteLine("Client connected!");

    // 5. 네트워크 스트림 생성
    using NetworkStream stream = client.GetStream();

    using StreamReader reader = new StreamReader(
        stream, Encoding.UTF8, leaveOpen: true);

    using StreamWriter writer = new StreamWriter(
        stream, new UTF8Encoding(false),
        leaveOpen: true)
    {
        AutoFlush = true
    };

    // 6. 클라이언트 메시지 수신
    while (true)
    {
        string? message = reader.ReadLine();

        // 연결 종료
        if (message == null)
        {
            Console.WriteLine("Client disconnected.");
            break;
        }

        Console.WriteLine($"Received: {message}");

        // 7. 메시지 응답
        writer.WriteLine($"Echo: {message}");
    }
}
