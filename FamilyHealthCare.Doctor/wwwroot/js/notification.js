"use strict";
let accessToken = localStorage.getItem("accessToken");

var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:52409/notification-hub", {
        accessTokenFactory: () => accessToken
    }).build();

connection.on("SendNotification", function (message) {
    console.log(message)
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