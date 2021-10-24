"use strict";

var roomName;
docReady();
function docReady() {
    var url = window.location.href;
    var matchid = url.split('/').pop();
    roomName = 'Match' + matchid;
    console.log('roomName: ' + roomName);
}

var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("matchesHub")
    .build();



window.onbeforeunload = DisconnectFromHub;
function DisconnectFromHub() {
    connection.invoke("LeaveRoom", roomName);
}

connection.on("PrintInConsole", function (Text) {
    console.log(Text);
    setTimeout(function () {
        confirmExit();
    }, 2500);
});

function confirmExit() {
    return "You have attempted to leave this page.  If you have made any changes to the fields without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page?";
}

connection.on("NewAction", function (text) {
    console.log('Dodawanie loga...');
    var actionEl = document.createElement('div');
    actionEl.classList.add('logs-action');
    actionEl.innerText = text;
    var logs = document.getElementById('logs');
    if (logs != undefined) {
        logs.insertBefore(actionEl, logs.firstChild);
        console.log('Dodano nowy log do kolekcji.');
    }
});

//Must be the last one
connection.start().then(function () {
    
    connection.invoke("JoinRoom", roomName);
}).catch(function (err) {
    return console.error(err.toString());
});
