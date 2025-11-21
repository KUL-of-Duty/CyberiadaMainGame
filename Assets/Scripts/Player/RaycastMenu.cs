using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class RaycastMenu : NetworkBehaviour
{

    [SerializeField] LayerMask hitMask;
    [SerializeField] GameObject playerCamera;
    void Start()
    {
        if(!IsServer) this.enabled = false;
        if(IsOwner)
        {
            playerCamera = GetComponentInChildren<CinemachineCamera>().gameObject;
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKey(KeyCode.E))
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 1000f, hitMask))
            {
                Debug.DrawRay(transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.gameObject.GetComponentInParent<Button>())
                {
                    hit.collider.gameObject.GetComponentInParent<Button>().onClick.Invoke();
                }
            }
        }
    }
}
