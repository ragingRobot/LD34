var express = require('express');
var app = express();

var _ = require("underscore");
var http = require('http').Server(app);
var io = require('socket.io')(http);

var players = [];           // ALL PLAYERS
var lobby_players = [];     // PLAYERS WAITING FOR GAME
var active_games = [];      // ACTIVE GAMES

io.on('connection', function(socket){
    console.log('a user connected');

    players.push(socket);

    //try to pair players for a game, or add them to the lobby
    if(lobby_players.length === 1){
        try_to_pair_players(lobby_players[0], socket);
    } else {
        lobby_players.push(socket);
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

http.listen(3000, function(){
    console.log('listening on *:3000');
});

//HELPER METHODS
function remove_from(array, item){
    var index = array.indexOf(item);
    if(index > -1){
        array.splice(index, 1);
    }
}

function generate_game(players_array, game_uuid){
    Game = {};
    Game.Player_1 = players_array[0];
    Game.Player_2 = players_array[1];
    Game.Id = game_uuid;
    active_games.push(Game);
    io.emit('new_game', {Game}); //Not sure if the clients will need to be aware of the games. We can remove if not.
}

function try_to_pair_players(player_1, player_2){
    if(player_1 && player_2){
        //Remove players form the lobby, if they're there.
        remove_from(lobby_players, player_1);
        remove_from(lobby_players, player_2);
        //Make a game using these players
        var game_uuid = player_1.id + "_" + player_2.id // unique id for the game
        generate_game([player1, player_2], game_uuid);
    } else {
        console.log("Issue pairing players.");
    }
}