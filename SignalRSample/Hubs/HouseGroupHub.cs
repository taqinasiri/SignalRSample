﻿namespace SignalRSample.Hubs;

public class HouseGroupHub : Hub
{
    public static List<string> GroupsJoined { get; set; } = new();

    public async Task JoinHouse(string houseName)
    {
        if(!GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
        {
            GroupsJoined.Add(Context.ConnectionId + ":" + houseName);

            string houseList = "";
            foreach(string house in GroupsJoined)
            {
                if(house.Contains(Context.ConnectionId))
                {
                    houseList += house.Split(':')[1] + " ";
                }
            }
            await Clients.Caller.SendAsync("subscriptionStatus",houseList,houseName.ToLower(),true);
            await Clients.Others.SendAsync("newMemeberAddedToHouse",houseName);

            await Groups.AddToGroupAsync(Context.ConnectionId,houseName);
        }
    }

    public async Task LeaveHouse(string houseName)
    {
        if(GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
        {
            GroupsJoined.Remove(Context.ConnectionId + ":" + houseName);

            string houseList = "";
            foreach(string house in GroupsJoined)
            {
                if(house.Contains(Context.ConnectionId))
                {
                    houseList += house.Split(':')[1] + " ";
                }
            }
            await Clients.Caller.SendAsync("subscriptionStatus",houseList,houseName.ToLower(),false);
            await Clients.Others.SendAsync("memeberRemovedFromHouse",houseName);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId,houseName);
        }
    }

    public async Task TriggerHouseNotify(string houseName) => await Clients.Group(houseName).SendAsync("triggerHouseNotification",houseName);
}