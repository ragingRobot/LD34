var express = require('express');
var app = express();

var _ = require("underscore");
var http = require('http').Server(app);
var io = require('socket.io')(http);
var port = 3000;

var players = [];           // ALL PLAYERS
var lobby_players = [];     // PLAYERS WAITING FOR GAME
var active_games = [];      // ACTIVE GAMES

io.attach(port);

app.get('/', function(req, res){
    res.sendfile('example_client_assets/lobby.html');
});

io.on('connection', function(socket){
    console.log('User Connected! UUID: "' + socket.id + '"');

    players.push(socket);

    //try to pair players for a game, or add them to the lobby
    if(lobby_players.length === 1){
        console.log("Trying to Pairing Players..");
        try_to_pair_players(lobby_players[0], socket);
    } else {
        lobby_players.push(socket);
        console.log("Unable to match players, waiting on more to join..");
    }

    //Disconnect Event
    socket.on('disconnect', function(){
        console.log('user disconnected');
        remove_from(players, socket);
        remove_from(lobby_players, socket);
    });

    // -- EXAMPLE --
    var update_throttled = _.throttle(function(data) {
        io.emit('update', {player:socket.id});
    }, 100);

    socket.on('button_press', update_throttled);
    // -- /EXAMPLE -- 

});
/*
//using io.attach(port); instead. that works for some reason
http.listen(port, function(){
    console.log('listening on *:'+ port);
});*/

//HELPER METHODS
function remove_from(array, item){
    var index = array.indexOf(item);
    if(index > -1){
        array.splice(index, 1);
    }
}

function generate_game(players_array, game_uuid){
    console.log("Adding Players to room "+game_uuid);
    for(i in players_array){
        players_array[i].join(game_uuid);
    }
    active_games.push(game_uuid);
    io.to(game_uuid).emit("message", {message: "Joined Game "+game_uuid});

}

function try_to_pair_players(player_1, player_2){
    if(player_1 && player_2){
        //Remove players form the lobby, if they're there.
        remove_from(lobby_players, player_1);
        remove_from(lobby_players, player_2);
        //Make a game using these players
        var game_uuid = player_1.id + "_" + player_2.id // unique id for the game
        generate_game([player_1, player_2], game_uuid);
    } else {
        console.log("Issue pairing players.");
    }
}