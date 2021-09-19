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

connection.on("MatchLive", function (matchId, mapTag) {
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
    document.getElementById('match' + matchId + 'mapTag').innerText = mapTag;

    var audio = document.getElementById('matchWentLive');
    audio.play();
});

connection.on("UpdateScore", function (matchId, scoreTeam1, scoreTeam2) {
    //Find elements responsible for score and replace their text
    document.getElementById('match' + matchId + 'scoreteam1').innerText = scoreTeam1;
    document.getElementById('match' + matchId + 'scoreteam2').innerText = scoreTeam2;
});

connection.on("MatchEnded", function (matchId, team1result, team2result) {
    //Find match to move and move to finished matches table
    //TODO: add style to score
    var matchToMove = document.getElementById('match' + matchId);
    var finishedMatches = document.getElementById('finishedMatches');
    var firstresult = document.getElementById('finished1');

    //Set classes for scores
    document.getElementById('match' + matchId + 'scoreteam1').classList.add('score-' + team1result);
    document.getElementById('match' + matchId + 'scoreteam2').classList.add('score-' + team2result);

    finishedMatches.insertBefore(matchToMove, firstresult);
    //Checks if there is no matches left in Live div, if there is no matches add text
    var liveMatches = document.getElementById('liveMatches');
    var liveMatchesCount = document.getElementById('liveMatches').children.length;
    if (liveMatchesCount == 1) {
        var title = document.createElement("H5");
        title.id = 'nomatches';
        var t = document.createTextNode('Nie ma akutalnie spotkań LIVE.');
        title.appendChild(t);
        title.style.color = 'yellow';
        liveMatches.appendChild(title);
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

