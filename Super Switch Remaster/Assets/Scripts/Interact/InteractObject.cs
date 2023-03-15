using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour, IInteract
{
    public virtual void Interact()
    {
        Debug.LogError("InteractObject.Interact()");
    }
}
