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

- .NET 9.0. Probably will work on previous or next dotnet runtimes.
- A terminal client like `telnet` or `nc` (netcat)

### Running the Server

You can run the server in several ways:

1.  **Direct execution with dotnet run:**
    ```bash
    dotnet run <IP_ADDRESS> <PORT>
    ```

2.  **Publish and run:**
    ```bash
    dotnet publish -c Release
    ./bin/Release/netX.0/publish/TcpChat.exe <IP_ADDRESS> <PORT>
    ```

3.  **Run published DLL:**
    ```bash
    dotnet TcpChat.dll <IP_ADDRESS> <PORT>
    ```

### Examples

#### Starting the Server

To start the server on your local machine on port 8080:

```bash
dotnet run 127.0.0.1 8080
```

#### Connecting to the Server

You can connect to the running server using `telnet` or `nc`.

**Using `telnet`:**

```bash
telnet 127.0.0.1 8080
```

**Using `nc` (netcat):**

```bash
nc 127.0.0.1 8080
```

After connecting, you will be prompted to set a username.

## Commands

Once connected, you can use the following commands:

-   **Setting a username:**
    The first thing you must do after connecting is set your username. The server will prompt you with: `Welcome to the server! Please set your username:`
    Simply type your desired username and press Enter.

-   **Sending messages:**
    Once your username is set, anything you type will be broadcast to all other connected clients. For example: `Hello everyone!`

-   **Disconnecting:**
    To disconnect from the server, simply close your terminal client using `Ctrl+C` or `Ctrl+]` and then `quit` for telnet.
