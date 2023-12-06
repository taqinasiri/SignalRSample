//Create connection

var connectionUserCount = new signalR.HubConnectionBuilder()
    .withUrl("hubs/userCount")
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
    connectionUserCount.send("NewWindowLoaded");
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