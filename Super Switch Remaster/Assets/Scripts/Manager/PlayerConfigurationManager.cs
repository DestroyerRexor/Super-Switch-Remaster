using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    [SerializeField] private PlayerConfiguration playerConfig;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance of singleton!!");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public PlayerConfiguration GetPlayerConfig()
    {
        return playerConfig;
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player Joined " + pi.playerIndex);

        pi.transform.SetParent(transform);
        playerConfig = new PlayerConfiguration(pi);

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DestroyChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
