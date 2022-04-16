"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:52409/notification-hub").build();

connection.on("SendNotification", function (message) {
    var HandlerbarTemplate = $("#NotificationTemplate").html(); // GET TEMPLATE
    var CompileHtml = Handlebars.compile(HandlerbarTemplate);
    var NotificationHTML = CompileHtml({ Notification: message }); //SET RECORD LIST TO HANDLEBARS OBJECT
    $('#notification').append(NotificationHTML); // APPEND HTML TO TABLE BODY
    //var li = document.createElement("li");
    //document.getElementById("notification").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    //alert("new noti")
});

connection.start();

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});