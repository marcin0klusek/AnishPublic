"use strict";

var isLiveMatchesEmpty = false;
if (document.getElementById('nomatches')) {
    isLiveMatchesEmpty = true;
}

var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl("resultsHub")
    .build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true; 

connection.on("MatchLive", function (matchId, mapName) {
    var nomatches = document.getElementById('nomatches')
    if (nomatches) {
        nomatches.remove();
    }
    //Find match to move and move to live matches table
    var matchToMove = document.getElementById('match' + matchId);
    document.getElementById('liveMatches').appendChild(matchToMove);

    //Set score to 0-0
    document.getElementById('match' + matchId + 'scoreteam1').innerText = 0;
    document.getElementById('match' + matchId + 'scoreteam2').innerText = 0;

    //Set map name to valid
    document.getElementById('match' + matchId + 'MapName').innerText = mapName;

    var audio = document.getElementById('matchWentLive');
    audio.play();
});

connection.on("UpdateScore", function (matchId, scoreTeam1, scoreTeam2) {
    //Find elements responsible for score and replace their text
    document.getElementById('match' + matchId + 'scoreteam1').innerText = scoreTeam1;
    document.getElementById('match' + matchId + 'scoreteam2').innerText = scoreTeam2;
});

connection.on("MatchEnded", function (matchId) {
    //Find match to move and move to finished matches table
    //TODO: add style to score
    var matchToMove = document.getElementById('match' + matchId);
    var finishedMatches = document.getElementById('finishedMatches');
    var firstresult = document.getElementById('finished1');
    finishedMatches.insertBefore(matchToMove, firstresult);

    //Checks if there is no matches left in Live div, if there is no matches add text
    var liveMatches = document.getElementById('liveMatches');
    var liveMatchesCount = document.getElementById('liveMatches').children.length;
    if (liveMatchesCount == 1) {
        liveMatches.innerHTML += '<h3 style="color: yellow" id="nomatches">Nie ma akutalnie spotkań LIVE.</h3>';
        isLiveMatchesEmpty = true;
    }

    var audio = document.getElementById('matchEnds');
    audio.play();
});

connection.start().then(function () {
   //start connection with resultsHub
    //put commands to do after connected to the hub
}).catch(function (err) {
    return console.error(err.toString());
});

