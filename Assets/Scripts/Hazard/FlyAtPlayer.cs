using Unity.VisualScripting;
using UnityEngine;

public class FlyAtPlayer : MonoBehaviour
{
    [SerializeField] Transform Player;
    Vector3 PlayerPosition;
    [SerializeField] float speed = 1f;
    [SerializeField] public int damage = 20;
    void Start()
    {
       
    }
    void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        PlayerPosition = Player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, Time.deltaTime * speed);
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag!="Hazard")
            Destroy(gameObject);
    }

}
