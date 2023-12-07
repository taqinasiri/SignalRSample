var btn_sendMessage = document.getElementById("sendMessage");
var ul_messagesList = document.getElementById("messagesList");

var connectionBasicChat = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/basicChat")
    .build();

btn_sendMessage.disabled = true;

connectionBasicChat.on("messageReceived", (user, message) => {
    var li = document.createElement("li");
    li.textContent = `${user} - ${message}`;
    ul_messagesList.appendChild(li);
});

btn_sendMessage.addEventListener("click", (event) => {
    var sender = document.getElementById("senderEmail").value;
    var message = document.getElementById("chatMessage").value;
    var receiver = document.getElementById("receiverEmail").value;

    if (receiver.length > 0) {
        connectionBasicChat.send("SendMessageToReceiver", sender, receiver, message).catch((err) => console.error(err));
    }
    else {
        connectionBasicChat.send("SendMessageToAll", sender, message).catch((err) => console.error(err));
    }

    event.preventDefault();
});

//start connection
function fulfilled() {
    console.log("Connection to user hub successful");
    btn_sendMessage.disabled = false;
}
function rejected() {
    console.log("Connection to user hub rejected");
}

connectionBasicChat.start().then(fulfilled, rejected);