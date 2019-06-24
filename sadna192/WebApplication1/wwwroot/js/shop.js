"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ShopHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;
var numberOfNotifications = 0;
connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "- System says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("notifications_body").appendChild(li);
    numberOfNotifications += 1;

    document.getElementById("notifi_number_font").innerHTML = numberOfNotifications;

});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
  //  var user = document.getElementById("userInput").value;
    //var message = document.getElementById("messageInput").value;
    //var reason = "I Said So";
    //sendNotification(message);
    //event.preventDefault();
//});

function sendNotification(message) {
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
}

function sendNotification2(message) {
    connection.invoke("SendMessage2", message).catch(function (err) {
        return console.error(err.toString());
    });
}
