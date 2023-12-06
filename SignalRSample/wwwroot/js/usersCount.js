//Create connection

var connectionUserCount = new signalR.HubConnectionBuilder()
    //.configureLogging(signalR.LogLevel.Trace)
    .withUrl("hubs/userCount", signalR.HttpTransportType.WebSockets)
    //.withUrl("hubs/userCount", signalR.HttpTransportType.LongPolling)
    //.withUrl("hubs/userCount", signalR.HttpTransportType.ServerSentEvents)
    .build();

//connect to methods that hub invoked aka receive notification for the hub

connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerHTML = value;
});
connectionUserCount.on("updateTotalUsers", (value) => {
    var newCountSpan = document.getElementById("totalUsereCounter");
    newCountSpan.innerHTML = value;
});

//invoke hub methods aka send notification to hub
function NewWindowLoadedOnClinet() {
    // connectionUserCount.send("NewWindowLoaded");
    connectionUserCount.invoke("NewWindowLoaded").then((value) => console.log(value));
}
//start connection
function fulfilled() {
    console.log("Connection to user hub successful");
    NewWindowLoadedOnClinet();
}
function rejected() {
    console.log("Connection to user hub rejected");
}

connectionUserCount.start().then(fulfilled, rejected);