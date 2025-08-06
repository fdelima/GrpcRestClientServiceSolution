using System.Diagnostics;

using var httpClient = new HttpClient();

const string port = "8082"; // "7262";

Console.WriteLine($"Starting REST client listen port {port}...");

var myTasks = new List<Task>();
var stopwatch = Stopwatch.StartNew();
for (int i = 0; i < 250000; i++)
{
    if (i > 0 && i % 50000 == 0)
        Console.WriteLine($"Send 50.000 requests: {DateTime.Now:HH:mm:ss}");

    myTasks.Add(RequestRest(httpClient, i));
}
Task.WaitAll(myTasks);

stopwatch.Stop();
Console.WriteLine($"Tempo gasto para execução de 250.000: {stopwatch.Elapsed}");

async Task RequestRest(HttpClient httpClient, int i)
{
    var response = await httpClient.GetAsync($"http://restserviceserver:{port}/weatherforecast/{i}");
    //var response = await httpClient.GetAsync($"http://localhost:{port}/weatherforecast");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    //Console.WriteLine($"Resposta da API weatherforecast:{i}");
}
