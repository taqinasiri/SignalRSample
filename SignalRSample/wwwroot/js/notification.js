var connectionNotification = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notification")
    .build();

document.getElementById("sendButton").disabled = true;
document.getElementById("sendButton").addEventListener("click", (event) => {
    var message = document.getElementById("notificationInput").value;
    connectionNotification.send("SendMessage", message).then(() => {
        document.getElementById("notificationInput").value = "";
    });
    event.preventDefault();
});

connectionNotification.on("loadNotification", (messages, counter) => {
    document.getElementById("messageList").innerHTML = "";
    document.getElementById("notificationCounter").innerHTML = `<span>(${counter})</span>`;
    for (let i in messages) {
        var li = document.createElement("li");
        li.textContent = "Notification - " + messages[i];
        document.getElementById("messageList").appendChild(li);
    }
});

//start connection

function fulfilled() {
    console.log("Connection to user hub successful");
    connectionNotification.invoke("LoadMessages");
    document.getElementById("sendButton").disabled = false;
}
function rejected() {
    console.log("Connection to user hub rejected");
}

connectionNotification.start().then(fulfilled, rejected);