"use strict";

//setup connection
var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("/ticketHub")
    .build();

connection.on("UpdateStatus", function (ticketId, status) {
    document.getElementById('ticket' + ticketId + 'status').innerText = status;
    var icon = document.getElementById('ticket' + ticketId + 'icon');
    switch (status) {
        case 'Closed':
            icon.innerText = 'task_alt';
            break;
        case 'New':
            icon.innerText = 'lock_open';
            MoveTicketToTable(ticketId, 'tickets-new');
            break;
        case 'Taken':
            icon.innerText = 'lock';
            MoveTicketToTable(ticketId, 'tickets-todo');
            break;
        case 'Responded':
            icon.innerText = 'lock_open';
            MoveTicketToTable(ticketId, 'tickets-todo');
            break;
        case 'Needs_Attention':
            icon.innerText = 'priority_high';
            MoveTicketToTable(ticketId, 'tickets-todo');
            break;
        default:
            icon.innerText = 'lock_open';
            break;
    }

    if (status != 'Closed') {
        PlaySound('beep');
    }
});

connection.on("NewTicket", function (ticketId, subject, status) {
    var destinationTable = document.getElementById('tickets-new');
    AppendTicket(ticketId, subject, status, destinationTable);

    PlaySound('ticket_new');
});

connection.on("CompleteTicket", function (ticketId, status) {
    //complete ticket and move to finished
    MoveTicketToTable(ticketId, 'tickets-completed');
    PlaySound('ticket_ended');
});

function MoveTicketToTable(ticketId, destinationId) {
    var ticket = document.getElementById('ticket' + ticketId + 'row');
    var ticketsCompleted = document.getElementById(destinationId);

    ticketsCompleted.appendChild(ticket);
}

function PlaySound(name) {
    var audio = document.getElementById(name);
    audio.play();
}


connection.start().then(function () {
    //start connection with ticketHub
    //put commands to do after connected to the hub
}).catch(function (err) {
    return console.error(err.toString());
});

function AppendTicket(ticketId, subjectText, status, destinationEl) {
    var ticket = document.createElement("a");
    ticket.href = "/ticket/" + ticketId;
    ticket.id = "ticket" + ticketId + "row";

    var row = document.createElement("div");
    row.classList.add("tickets--row");

    var id = document.createElement("div");
    id.classList.add("ticket-id");
    id.id = "ticket" + ticket.Id + "id";
    id.innerText = "#" + ticketId;

    var icon = document.createElement("div");
    icon.classList.add("ticket--icon");
    var iconspan = document.createElement("span");
    iconspan.classList.add("material-icons-outlined");
    iconspan.id = "ticket" + ticket.Id + "icon";
    iconspan.innerText = "lock_open";
    icon.appendChild(iconspan);

    var subject = document.createElement("div");
    subject.classList.add("ticket--title");
    subject.innerText = subjectText;

    var statusDiv = document.createElement("div");
    statusDiv.classList.add("ticket-status");
    statusDiv.innerText = status;
    statusDiv.id = "ticket" + ticket.Id + "status";

    row.appendChild(id);
    row.appendChild(icon);
    row.appendChild(subject);
    row.appendChild(statusDiv);
    ticket.appendChild(row);

    destinationEl.appendChild(ticket);
}