﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkLobbyHook : LobbyHook {
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager,
                                                           GameObject lobbyPlayer, GameObject gamePlayer){
        
        gamePlayer.GetComponent<myPlayerController>().pname = lobbyPlayer.GetComponent<LobbyPlayer>().playerName;
        gamePlayer.GetComponent<myPlayerController>().pcolor = lobbyPlayer.GetComponent<LobbyPlayer>().playerColor;
    }

}
