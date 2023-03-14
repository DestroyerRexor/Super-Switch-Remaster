using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawns;
    [SerializeField] private GameObject playerPrefab;

    private List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();

    private void Start()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs();
        }

        for (int i = 0; i < playerConfigs.Count; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, Quaternion.identity, transform);
            player.GetComponentInChildren<GameInput>().InitializePlayer(playerConfigs[i]);
        }

    }

}
