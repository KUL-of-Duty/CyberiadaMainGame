using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBarExtentionScript : NetworkBehaviour
{
    public PlayerInput playerInput;
    public Canvas PlayerBarUI;
    public static List<Canvas> Players = new List<Canvas>();
    InputAction extend;
    public Vector2 PlayerBarPosition = new Vector2(-510,-420);
    bool barsExtended = false;

    public override void OnNetworkSpawn(){
        
        if (!IsOwner)
        {   
            Players.Add(PlayerBarUI);
            playerInput.enabled = false;
            PlayerBarUI.enabled = false;
        }
        if (!IsOwner) return;

        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null){
            Debug.LogError("PlayerInput NOT FOUND");
            return;
        }else Debug.Log("PlayerInput FOUND successfuly");

        extend = playerInput.actions.FindAction("PlayerBarExtension", true);

        if (extend == null){
            Debug.LogError("Action NOT FOUND");
            return;
        }else Debug.Log("Action FOUND successfuly");

        Vector3 v3 = new Vector3(PlayerBarPosition.x,PlayerBarPosition.y,0);
        foreach(var player in NetworkManager.ConnectedClientsIds)
        {
            Debug.Log(player.ToString());
        }
    }
    void Start()
    {
        if(IsOwner){
            BarsPositioning();
        }
    }


    void Update()
    {
        if(!IsOwner) return;
        if (extend == null) return;
        BarsExtension();
    }

    void BarsPositioning(){
        Debug.Log(PlayerBarUI.GetComponent<RectTransform>().transform.localPosition);
        Vector3 startVector = PlayerBarUI.GetComponent<RectTransform>().localPosition;
        for(int i=0; i<Players.Count;i++)
            {
                RectTransform rt = Players[i].GetComponent<RectTransform>();
                Players[i].GetComponent<RectTransform>().transform.localPosition = new Vector3(
                                                        startVector.x,
                                                        startVector.y+rt.rect.height*(1+i),
                                                        0);
            }
    }

    void BarsExtension(){
        foreach(Canvas c in Players){
            if(c == null){
                Players.Remove(c);
                BarsPositioning();
            }
        }
        if (extend.IsPressed() && !barsExtended){
            foreach(Canvas c in Players){
                if(c == null) continue;
                c.enabled = true;
                barsExtended = true;
            }
        }
        else if (!extend.IsPressed() && barsExtended)
        {
            foreach(Canvas c in Players)
            {
                c.enabled=false;
                barsExtended=false;
            }
        }
    }
}
