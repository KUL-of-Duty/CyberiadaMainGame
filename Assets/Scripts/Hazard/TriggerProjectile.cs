using Unity.Netcode;
using UnityEngine;

public class NewMonoBehaviourScript : NetworkBehaviour
{
    [SerializeField] GameObject[] projectile;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (GameObject g in projectile){
                 NetworkObject no = g.GetComponent<NetworkObject>();
                if (!no.IsSpawned)
                    no.Spawn(); // rejestruje w SpawnManagerze
                g.GetComponent<SphereCollider>().enabled =true;
                g.GetComponent<MeshRenderer>().enabled =true;
            }
            Destroy(gameObject);
        }
    }
}

