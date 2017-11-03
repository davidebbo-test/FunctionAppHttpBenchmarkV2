using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

public static async Task<IActionResult> Run(HttpRequest req)
{
    //log.Info("C# HTTP trigger function processed a request.");

    long memory = GetLongQueryStringParam(req, "AllocInMB", 0);
    int sleep = (int)GetLongQueryStringParam(req, "SleepInMS", 0);
    long iterations = GetLongQueryStringParam(req, "LoopSpins", 0);
    var rnd = new Random();

    // Allocate memory and fill it with random bytes
    byte[] bytes = new byte[memory * 1024 * 1024];
    rnd.NextBytes(bytes);

    // Do an async sleep
    await Task.Delay(sleep);

    // Spin the CPU
    for (long i = 0; i < iterations; i++) { }

    HttpResponse response = req.HttpContext.Response;
    response.Headers.Add("X-server", Environment.GetEnvironmentVariable("COMPUTERNAME"));

    return new OkObjectResult("Done");
}

internal static long GetLongQueryStringParam(HttpRequest req, string paramName, int defaultValue)
{
    string paramValue = req.Query[paramName];

    if (paramValue == null)
    {
        return defaultValue;
    }

    return long.Parse(paramValue);
}
