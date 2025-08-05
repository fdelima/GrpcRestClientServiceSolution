// Chamada HTTP para https://localhost:7262/weatherforecast
using System.Diagnostics;

using var httpClient = new HttpClient();

var myTasks = new List<Task>();
var stopwatch = Stopwatch.StartNew();
for (int i = 0; i < 100000; i++)
{
    myTasks.Add(RequestRest(httpClient, i));
}
Task.WaitAll(myTasks);

stopwatch.Stop();
Console.WriteLine($"Tempo gasto para execução do for: {stopwatch.Elapsed}");

static async Task RequestRest(HttpClient httpClient, int i)
{
    var response = await httpClient.GetAsync("https://localhost:7262/weatherforecast");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Resposta da API weatherforecast:{i}");
}
//Tempo gasto para execução do for: 00:00:03.8973082
