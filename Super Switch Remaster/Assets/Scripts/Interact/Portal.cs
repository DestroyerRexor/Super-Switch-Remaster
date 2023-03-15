using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Portal : MonoBehaviour
{
    [SerializeField] private Loader.Scene scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (scene == Loader.Scene.Main_Menu_Scene)
                PlayerConfigurationManager.Instance.DestroyObject();
            Loader.Load(scene);
        }
    }
}
