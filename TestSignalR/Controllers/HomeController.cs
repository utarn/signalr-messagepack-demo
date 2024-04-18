using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using TestSignalR.Hubs;
using TestSignalR.Models;

namespace TestSignalR.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<DataHub> _hub;

    public HomeController(ILogger<HomeController> logger, IHubContext<DataHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Privacy()
    {
        await _hub.Clients.Groups("ActiveUser").SendAsync("notification", "Privacy page visited!");
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}