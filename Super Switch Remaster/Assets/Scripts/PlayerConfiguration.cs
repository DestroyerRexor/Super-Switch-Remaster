using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Configuration")]
public class PlayerConfiguration : ScriptableObject
{
    public PlayerInput input;
    public int playerIndex;

    public PlayerConfiguration(PlayerInput pi)
    {
        playerIndex = pi.playerIndex;
        input = pi;
    }

}
