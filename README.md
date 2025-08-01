# TCP Chat Server

A simple asynchronous TCP chat server written in C# to demonstrate asynchronous programming concepts. This project serves as a testbed for exploring async/await patterns and concurrent programming in .NET.

## Features

- Asynchronous TCP server implementation
- Multiple client support
- Concurrent message broadcasting
- Username validation
- Graceful shutdown handling

## Getting Started

### Prerequisites

- .NET 6.0 or later
- A terminal client like `telnet` or `nc` (netcat)

### Running the Server

You can run the server in several ways:

1. **Direct execution with dotnet run:**
   ```bash
   dotnet run <IP_ADDRESS> <PORT>
   ```

2. **Publish and run:**
   ```bash
   dotnet publish -c Release
   ./bin/Release/net6.0/publish/TcpChat.exe <IP_ADDRESS> <PORT>
   ```

3. **Run published DLL:**
   ```bash
   dotnet TcpChat.dll <IP_ADDRESS> <PORT>
   ```

### Examples

