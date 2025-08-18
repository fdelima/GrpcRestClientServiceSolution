using System.Diagnostics;

const string _port = "8082";
const int _totalRequests = 500000;
const int _totalBatch = 125000;
const int _waitingTime = 1000;

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
        //await RequestRest(httpClient, i);
        myTasks.Add(RequestRest(httpClient, i));

        if (i > 1 && i % _totalBatch == 0)
        {
            await Task.WhenAll(myTasks);
            Console.WriteLine($"Send {_totalBatch:#,##0,000} requests: {DateTime.Now:HH:mm:ss}");
            myTasks.Clear();
        }
    }

    stopwatch.Stop();
    Console.WriteLine($"Tempo gasto para execução de {_totalRequests:#,##0,000}: {stopwatch.Elapsed}");
  
    stopwatch = Stopwatch.StartNew();
}