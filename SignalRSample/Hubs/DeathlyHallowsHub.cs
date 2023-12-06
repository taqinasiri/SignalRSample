namespace SignalRSample.Hubs;

public class DeathlyHallowsHub : Hub
{
    public Dictionary<string,int> GerRaceStatus() => SD.DeathlyHallowRace;
}