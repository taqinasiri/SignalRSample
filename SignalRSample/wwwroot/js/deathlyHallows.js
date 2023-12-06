var cloakSpan = document.getElementById("cloakCounter");
var stoneSpan = document.getElementById("stoneCounter");
var wandSpan = document.getElementById("wandCounter");

//Create connection

var connectionDeathlyHallows = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/deathlyHallows")
    .build();

//connect to methods that hub invoked aka receive notification for the hub

connectionDeathlyHallows.on("updateDeathlyHallowsCount", (cloak, stone, wand) => {
    cloakSpan.innerText = cloak;
    stoneSpan.innerText = stone;
    wandSpan.innerText = wand;
});
//invoke hub methods aka send notification to hub

//start connection

function fulfilled() {
    console.log("Connection to user hub successful");
    connectionDeathlyHallows.invoke("GerRaceStatus").then((raceCounter) => {
        cloakSpan.innerText = raceCounter.cloak;
        stoneSpan.innerText = raceCounter.stone;
        wandSpan.innerText = raceCounter.wand;
    });
}
function rejected() {
    console.log("Connection to user hub rejected");
}

connectionDeathlyHallows.start().then(fulfilled, rejected);