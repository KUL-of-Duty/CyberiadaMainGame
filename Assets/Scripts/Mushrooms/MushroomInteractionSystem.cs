using Unity.VisualScripting;
using UnityEngine;

public class MushroomInteractionSystem : MonoBehaviour
{
    [SerializeField]
    Mushroom mushroomType;
    [SerializeField]
    GameObject interactionbutton;
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
