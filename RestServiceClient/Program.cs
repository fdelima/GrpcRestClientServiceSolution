using System.Diagnostics;

const string _port = "8082";
const int _totalRequests = 150000;
const int _totalBatch = 50000;
const int _waitingTime = 10000;

using var httpClient = new HttpClient();

Console.WriteLine($"Starting REST client listen port {_port}...");

async Task RequestRest(HttpClient httpClient, int i)
{
    var response = await httpClient.GetAsync($"http://restserviceserver:{_port}/weatherforecast/{i}");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
}

var myTasks = new List<Task>();
var stopwatch = Stopwatch.StartNew();
while (true)
{
    for (int i = 1; i <= _totalRequests; i++)
    {
        if (i > 1 && i % _totalBatch == 0)
            Console.WriteLine($"Send {_totalBatch:#,##0,000} requests: {DateTime.Now:HH:mm:ss}");

        myTasks.Add(RequestRest(httpClient, i));
    }
    Task.WaitAll(myTasks);

    stopwatch.Stop();
    Console.WriteLine($"Tempo gasto para execução de {_totalRequests:#,##0,000}: {stopwatch.Elapsed}");

    myTasks.Clear();
    Thread.Sleep(_waitingTime);
    stopwatch = Stopwatch.StartNew();
}