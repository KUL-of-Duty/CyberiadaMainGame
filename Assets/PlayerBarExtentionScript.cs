using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBarExtentionScript : NetworkBehaviour
{
    public Canvas PlayerBarUI;
    public Canvas ExtendedPlayerBarUI;
    InputAction extend;
    public PlayerInput playerInput;

    void Awake(){
        if (!IsOwner) return;

        playerInput = GetComponentInParent<PlayerInput>();

        if (playerInput == null){
            Debug.LogError("PlayerInput NOT FOUND");
            return;
        }

        extend = playerInput.actions.FindAction("PlayerBarExtension", true);

        if (extend == null){
            Debug.LogError("Action NOT FOUND");
            return;
        }
        ExtendedPlayerBarUI.enabled = false;
    }


    void Update()
    {
        if(!IsOwner) return;
        if (extend == null) return;
        if (extend.IsPressed() && !ExtendedPlayerBarUI.enabled)
        {
            PlayerBarUI.enabled=false;
            ExtendedPlayerBarUI.enabled=true;
        }
        else if (!extend.IsPressed()&& ExtendedPlayerBarUI.enabled)
        {
            PlayerBarUI.enabled=true;
            ExtendedPlayerBarUI.enabled=false;
        }
    }
}
