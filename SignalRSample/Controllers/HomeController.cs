using Microsoft.AspNetCore.Mvc;
using SignalRSample.Hubs;
using SignalRSample.Models;

namespace SignalRSample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<DeathlyHallowsHub> _deathlyHub;
    private readonly IHubContext<OrderHub> _orderHub;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger,IHubContext<DeathlyHallowsHub> deathlyHub,ApplicationDbContext context,IHubContext<OrderHub> orderHub)
    {
        _logger = logger;
        _deathlyHub = deathlyHub;
        _orderHub = orderHub;
        _context = context;
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

    public IActionResult Order()
    {
        string[] name = ["Taqi","Mohammad","Sara","Ali","Reyha"];
        string[] itemName = ["Food1","Food2","Food3","Food4","Food5"];

        Random rand = new Random();
        int index = rand.Next(name.Length);

        Order order = new Order()
        {
            Name = name[index],
            ItemName = itemName[index],
            Count = index
        };

        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Order(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        await _orderHub.Clients.All.SendAsync("newOrder");
        return RedirectToAction(nameof(Order));
    }

    public IActionResult OrderList() => View();

    [HttpGet]
    public IActionResult GetAllOrder()
    {
        var productList = _context.Orders.ToList();
        return Json(new { data = productList });
    }
}