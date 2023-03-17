using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawns;
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        if (FindObjectOfType<PlayerController>())
        {
            FindObjectOfType<PlayerController>().transform.position = playerSpawns[0].position;
        }
        else
        {
            GetPlayerConfig();
        }

    }

    private void GetPlayerConfig()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            var player = Instantiate(playerPrefab, playerSpawns[0].position, Quaternion.identity);
            player.GetComponentInChildren<GameInput>().InitializePlayer(PlayerConfigurationManager.Instance.GetPlayerConfig());
        }
        else
        {
            StartCoroutine(RepeatGetPlayerConfig());
        }
    }

    IEnumerator RepeatGetPlayerConfig()
    {
        yield return new WaitForSeconds(1);
        GetPlayerConfig();
    }

}
