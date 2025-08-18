using System.Diagnostics;
using Grpc.Net.Client;
using GrpcGreeterClient;

const string _port = "8080";
const int _totalRequests = 500000;
const int _totalBatch = 125000;
const int _waitingTime = 1000;

Console.WriteLine($"Starting gRPC client listen port {_port}...");

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress($"http://grpcserviceserver:{_port}");
var client = new Greeter.GreeterClient(channel);

async Task sayHello(Greeter.GreeterClient client, int i)
{
    var reply = await client.SayHelloAsync(
        new HelloRequest { Name = $"GreeterClient::{i}", Count = i });
}

var myTasks = new List<Task>();
var stopwatch = Stopwatch.StartNew();
while (true)
{
    for (int i = 1; i <= _totalRequests; i++)
    {
        //await sayHello(client, i);
        myTasks.Add(sayHello(client, i));

        if (i > 1 && i % _totalBatch == 0)
        {
            await Task.WhenAll(myTasks);
            Console.WriteLine($"Send {_totalBatch:#,##0,000} sayHello: {DateTime.Now:HH:mm:ss}");
            myTasks.Clear();
        }
    }

    stopwatch.Stop();
    Console.WriteLine($"Tempo gasto para execução de {_totalRequests:,##0,000}: {stopwatch.Elapsed}");

    stopwatch = Stopwatch.StartNew();
}