using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Shannels;

var (ip, port) = Helper.ParseArgs(args);

using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var ipEndPoint = new IPEndPoint(ip, port);
listener.Bind(ipEndPoint);
listener.Listen(100);

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, args) =>
{
    args.Cancel = true; 
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\nShutdowing...");
    Console.ResetColor();
    cts.Cancel();
};

Console.WriteLine($"Server starts accepting clients on {ip}:{port}. To close connection press Ctrl+C");
var socketCollection = new ConcurrentDictionary<string, StreamWriter>(); 

while (!cts.Token.IsCancellationRequested)
{
    try
    {
        var clientSocket = await listener.AcceptAsync(cts.Token);

        // Does it affect perfomance? idk
        _ = Task.Run(async () =>
        {
            try
            {
                await ReceiveAsync(clientSocket, cts.Token);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error processing client: {ex.Message}");
            }
        }, cts.Token);
    }
    catch (OperationCanceledException)
    {
        
    }
    
}
async Task ReceiveAsync(Socket client, CancellationToken token)
{
    await using var socketStream = new NetworkStream(client);
    await using var socketWriter = new StreamWriter(socketStream) {AutoFlush = true};
    
    using var streamReader = new StreamReader(socketStream);

    var username = await SetName(socketWriter, streamReader, token);
    while (client.Connected)
    {
        var message = await streamReader.ReadLineAsync(token);
        if(message is null || message.Trim() == string.Empty) continue;

        _ = SendAll($"{username}: {message}", token);
    }
    /* if(!socketCollection.TryRemove(username!, out _))
        return (true, "Unexpected behavior on removing socket from the collection"); */

    if(!socketCollection.TryRemove(username!, out _))
        throw new InvalidOperationException("Unexpected behavior on removing socket from the collection");
    
}

async Task SendAll(string message, CancellationToken token)
{
    foreach(var socket in socketCollection.Values)
    {
        await socket.WriteLineAsync(message.AsMemory(), token);
    }
}

async Task<string?> SetName(StreamWriter writer, StreamReader reader, CancellationToken token)
{
    var usernameRequirements = @"Username requirements:
- Must be between 3 and 20 characters long
- Can only contain letters, numbers, underscores, and hyphens
- Cannot start or end with a hyphen
- Cannot contain double hyphens (--)
Enter your name:";


    await writer.WriteLineAsync(usernameRequirements);
    var username = await reader.ReadLineAsync(token);

    if(username is null) return null;

    while(!isUserNameValid(username))
    {
        await writer.WriteLineAsync("Wrong username. Try again.");
        username = await reader.ReadLineAsync(token);
        if (username is null) return null;
    }
    if(!socketCollection.TryAdd(username, writer))
        throw new InvalidOperationException("Unexpected behavior on adding socket from the collection");

    await writer.WriteLineAsync($"Nice, {username}, be carefull :)");
    return username;
}

bool isUserNameValid(string username) => Helper.ValidUsernameRegex().IsMatch(username);
