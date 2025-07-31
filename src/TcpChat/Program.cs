using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Shannels;

var (ip, port) = Helper.ParseArgs(args);

using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var ipEndPoint = new IPEndPoint(ip, port);
listener.Bind(ipEndPoint);
listener.Listen(100);

Console.CancelKeyPress += (_, _) =>
{

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\nShutdowing...");
    Console.ResetColor();
};

Console.WriteLine($"Server starts accepting clients on {ip}:{port}. To close connection press Ctrl+C");
var socketCollection = new ConcurrentDictionary<StreamWriter, byte>(); //must be something like ConcurrentHashSet

while (true)
{
    var clientSocket = await listener.AcceptAsync();
    _ = ReceiveAsync(clientSocket).ContinueWith(async errorTask =>
    {
        var (isError, errorMessage) = await errorTask;
        if (isError)
        {

            Console.WriteLine("Server crashed.\n  - " + errorMessage!);
            await SendAll("Server crashed");
        }
    });
}

async Task<(bool isError, string? errorMessage)> ReceiveAsync(Socket client)
{
    await using var socketStream = new NetworkStream(client);
    await using var socketWriter = new StreamWriter(socketStream) {AutoFlush = true};
    
    if(!socketCollection.TryAdd(socketWriter, 0))
        return (true, "Unexpected behavior on adding socket to the collection");

    using var streamReader = new StreamReader(socketStream);

    await socketWriter.WriteLineAsync("Enter your username kiddo");
    var username = await streamReader.ReadLineAsync();
    await socketWriter.WriteLineAsync($"So your username is {{{username}}}. Be carefull.");
    while (client.Connected)
    {
        var message = await streamReader.ReadLineAsync();
        if(message is null || message.Trim() == string.Empty) continue;

        _ = SendAll($"{username}: {message}");
    }
    if(!socketCollection.TryRemove(socketWriter, out _))
        return (true, "Unexpected behavior on removing socket from the collection");

    return (false, null);
}

async Task SendAll(string message)
{
    foreach(var socket in socketCollection.Keys)
    {
        await socket.WriteLineAsync(message);
    }
}

async Task SetName(StreamWriter writer, StreamReader reader)
{
    await writer.WriteLineAsync("Enter your name:");
    var username = await reader.ReadLineAsync();

    if(username is null) return;

    if(username.Length is not > 2 and < 16 || username.Contains("!@#$%^&*()+={[\\|\"\':;?/.>,<]}~` "))
    {
        await writer.WriteLineAsync("You've entered wrong name");
    }
}
