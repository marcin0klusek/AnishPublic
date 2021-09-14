"use strict";

var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("resultsHub")
    .build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("UpdateScore", function (matchId, scoreTeam1, scoreTeam2) {
    document.getElementById(matchId + 'scoreteam1').innerText = scoreTeam1;
    document.getElementById(matchId + 'scoreteam2').innerText = scoreTeam2;
    //@(match.MatchID + "scoreteam1")
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    //li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    //when connected
    //document.getElementById("sendButton").disabled = false;
    /*var rereshIntervalId = setInterval(function () {
        var score1 = document.getElementById('7scoreteam1').textContent == '16' ? true : false;
        var score2 = document.getElementById('7scoreteam2').textContent == '16' ? true : false;
        if (score1 || score2) {
            alert('Zatrzymano interval!');
            clearInterval(rereshIntervalId);
        } else {
            connection.invoke("GetUpdateOnScore", 7).catch(function (err) {
                return console.error(err.toString());
            });
        }

    }, 4000);*/
}).catch(function (err) {
    return console.error(err.toString());
});

