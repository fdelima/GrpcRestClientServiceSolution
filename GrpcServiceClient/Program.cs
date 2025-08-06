using System.Diagnostics;
using Grpc.Net.Client;
using GrpcGreeterClient;

const string port = "8080"; // "7246";

Console.WriteLine($"Starting gRPC client listen port {port}...");

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress($"http://grpcserviceserver:{port}");
//using var channel = GrpcChannel.ForAddress($"http://localhost:{port}");
var client = new Greeter.GreeterClient(channel);

var myTasks = new List<Task>();
var stopwatch = Stopwatch.StartNew();
for (int i = 0; i < 250000; i++)
{
    if (i > 0 && i % 50000 == 0)
        Console.WriteLine($"Send 50.000 sayHello: {DateTime.Now:HHmmss}");

    myTasks.Add(sayHello(client, i));
}
Task.WaitAll(myTasks);

stopwatch.Stop();
Console.WriteLine($"Tempo gasto para execução de 250.000: {stopwatch.Elapsed}");

static async Task sayHello(Greeter.GreeterClient client, int i)
{
    var reply = await client.SayHelloAsync(
        new HelloRequest { Name = $"GreeterClient::{i}", Count = i.ToString() });
    //Console.WriteLine("Server response: " + reply.Message);
}