using System.Diagnostics;
using Grpc.Net.Client;
using GrpcGreeterClient;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7246");
var client = new Greeter.GreeterClient(channel);

var myTasks = new List<Task>();
var stopwatch = Stopwatch.StartNew();
for (int i = 0; i < 10000; i++)
{
    myTasks.Add(sayHello(client, i));
}
Task.WaitAll(myTasks);

stopwatch.Stop();
Console.WriteLine($"Tempo gasto para execução do for: {stopwatch.Elapsed}");
//Tempo gasto para execução do for: 00:01:03.4855090
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static async Task sayHello(Greeter.GreeterClient client, int i)
{
    var reply = await client.SayHelloAsync(
        new HelloRequest { Name = $"GreeterClient::{i}" });
    Console.WriteLine("Server response: " + reply.Message);
}