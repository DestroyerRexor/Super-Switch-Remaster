using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private PlayerConfiguration playerConfig;

    private PlayerController playerController;
    private PlayerVisual playerVisual;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerVisual = transform.parent.GetComponentInChildren<PlayerVisual>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        playerConfig = pc;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == playerInputActions.Player.Move.name)
        {
            OnMove(obj);
        }
        
        if (obj.action.name == playerInputActions.Player.Jump.name)
        {
            OnJump(obj);
        }

        if (obj.action.name == playerInputActions.Player.Interact.name)
        {
            OnInteract(obj);
        }

        if (obj.action.name == playerInputActions.Player.Pause.name)
        {
            GameManager.Instance.SetPause(obj);
        }

        if (obj.action.name == playerInputActions.Player.Restart.name)
        {
            GameManager.Instance.SetRestart(obj);
        }
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        playerController?.SetInputVector(context.ReadValue<Vector2>());
        playerVisual?.SetInputVector(context.ReadValue<Vector2>());
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        playerController?.SetJumpInput(context);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        playerController?.SetInteract(context);
    }

    private void RetrieveJumpInput(InputAction.CallbackContext context)
    {
        playerController?.SetRetrieveJumpInput(context);
    }

    private void OnDestroy()
    {
        if (playerConfig != null)
        {
            playerConfig.Input.onActionTriggered -= Input_onActionTriggered;
        }

        playerInputActions.Dispose();
    }

}
