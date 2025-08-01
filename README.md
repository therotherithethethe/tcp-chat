
```md
# TCP Chat Server

A simple asynchronous TCP chat server written in C# to demonstrate asynchronous programming concepts. This project serves as a testbed for exploring `async/await` patterns and concurrent programming in .NET.

## Features

- Asynchronous TCP server implementation
- Multiple client support
- Concurrent message broadcasting
- Username validation with specific requirements
- Graceful shutdown handling with `Ctrl+C`

## Getting Started

### Prerequisites

- .NET 9.0. Probably will work on previous or next dotnet runtimes.
- A terminal client like `telnet` or `nc` (netcat)

### Running the Server

You can run the server using `dotnet run`, providing an IP address and port as arguments.

```bash
dotnet run <IP_ADDRESS> <PORT>
```

## Examples

This example shows how to start the server and have two users connect and chat.

### 1. Start the Server

In your first terminal, start the server and have it listen on `127.0.0.1` port `8080`.

```bash
dotnet run 127.0.0.1 8080
```

The server will output the following and wait for connections:

```
Server starts accepting clients on 127.0.0.1:8080. To close connection press Ctrl+C
```

### 2. User1 (Alice) Connects

In a **new terminal window**, User1 connects using `telnet`.

```bash
telnet 127.0.0.1 8080
```

The server immediately prompts for a username with the validation rules from your code:

```
Trying 127.0.0.1...
Connected to 127.0.0.1.
Escape character is '^]'.
Username requirements:
- Must be between 3 and 20 characters long
- Can only contain letters, numbers, underscores, and hyphens
- Cannot start or end with a hyphen
- Cannot contain double hyphens (--)
Enter your name:
```

Alice types her username `Alice` and presses Enter. The server confirms her username.

```
Alice
Nice, Alice, be carefull :)
```

### 3. User2 (Bob) Connects

In a **third terminal window**, User2 connects.

```bash
telnet 127.0.0.1 8080
```

Bob sees the same username prompt. He enters `Bob` and is also welcomed.

```
Trying 127.0.0.1...
Connected to 127.0.0.1.
Escape character is '^]'.
Username requirements:
... (same as above)
Enter your name:
Bob
Nice, Bob, be carefull :)
```

### 4. Chatting

Now, the chat can begin.

**In Alice's terminal**, she types a message and presses Enter.

```
Hello everyone!
```

**In Bob's terminal**, he instantly sees Alice's message broadcast by the server.

```
Alice: Hello everyone!
```

Now **Bob replies in his terminal**.

```
Hi Alice, welcome to the chat!
```

**Alice's terminal** is updated with Bob's message.

```
Bob: Hi Alice, welcome to the chat!
```

### 5. Disconnecting

If Alice closes her `telnet` session (e.g., with `Ctrl+]` then `quit`), she will be disconnected. If Bob sends another message, only he will see it, as Alice is no longer in the chat.
```
