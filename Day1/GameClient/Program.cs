using System.Net.Sockets;
using System.Text;

// 1. TCP 클라이언트 생성
using TcpClient client = new TcpClient();

try
{
    // 2. 서버에 연결
    client.Connect("127.0.0.1", 5000);

    Console.WriteLine("Connected to server!");

    // 3. 통신 스트림 생성
    using NetworkStream stream = client.GetStream();

    using StreamReader reader = new StreamReader(
        stream, Encoding.UTF8, leaveOpen: true);

    using StreamWriter writer = new StreamWriter(
        stream, new UTF8Encoding(false),
        leaveOpen: true)
    {
        AutoFlush = true
    };

    // 4. 사용자 입력 반복
    while (true)
    {
        Console.Write("Message (/quit to exit): ");

        string? message = Console.ReadLine();

        if (message == null || message == "/quit")
            break;

        // 5. 서버로 메시지 전송
        writer.WriteLine(message);

        // 6. 서버 응답 수신
        string? response = reader.ReadLine();

        if (response == null)
        {
            Console.WriteLine("Server disconnected.");
            break;
        }

        Console.WriteLine($"Server: {response}");
    }
}
catch (SocketException ex)
{
    Console.WriteLine($"Connection error: {ex.Message}");
}
catch (IOException ex)
{
    Console.WriteLine($"Network I/O error: {ex.Message}");
}

Console.WriteLine("Client terminated.");
