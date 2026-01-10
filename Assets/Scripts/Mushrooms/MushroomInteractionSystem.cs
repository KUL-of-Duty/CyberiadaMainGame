using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class MushroomInteractionSystem : NetworkBehaviour
{
    [SerializeField]
    Mushroom mushroomType;
    [SerializeField]
    GameObject interactionbutton;
    void Awake() 
    {
        if(!IsOwner){
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShowInteraction();
    }

    private void OnTriggerExit(Collider other)
    {
        HideInteraction();
    }

    private void OnTriggerStay(Collider other)
    {
        interactionbutton.transform.LookAt(other.transform.position);

        if (!Input.GetKeyDown(KeyCode.E)) return;

        Destroy(transform.gameObject);
    }

    void ShowInteraction()
    {
        interactionbutton.SetActive(true);
    }

    void HideInteraction()
    {
        interactionbutton.SetActive(false);
    }
}
