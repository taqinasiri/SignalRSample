using Microsoft.AspNetCore.Mvc;
using SignalRSample.Hubs;
using SignalRSample.Models;
using System.Diagnostics;

namespace SignalRSample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<DeathlyHallowsHub> _deathlyHub;

    public HomeController(ILogger<HomeController> logger,IHubContext<DeathlyHallowsHub> deathlyHub)
    {
        _logger = logger;
        _deathlyHub = deathlyHub;
    }

    public IActionResult Index() => View();

    public IActionResult Notification() => View();

    public IActionResult HouseGroup() => View();

    public IActionResult UserCount() => View();

    public IActionResult BasicChat() => View();

    public async Task<IActionResult> DeathlyHallows(string? type)
    {
        if(string.IsNullOrEmpty(type))
            return View();

        if(SD.DeathlyHallowRace.ContainsKey(type))
        {
            SD.DeathlyHallowRace[type]++;
        }
        await _deathlyHub.Clients.All.SendAsync("updateDeathlyHallowsCount",
            SD.DeathlyHallowRace[SD.Cloak],
            SD.DeathlyHallowRace[SD.Stone],
            SD.DeathlyHallowRace[SD.Wand]);
        return View();
    }
}