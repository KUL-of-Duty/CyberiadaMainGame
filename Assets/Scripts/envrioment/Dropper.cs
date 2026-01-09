using UnityEngine;
using Unity.Netcode;

public class Dropper : NetworkBehaviour{
    [SerializeField] float timeToWait = 2f;

    MeshRenderer myMeshRenderer;
    Rigidbody myRigidBody;
    private void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myRigidBody = GetComponent<Rigidbody>();

        myMeshRenderer.enabled = false;
        myRigidBody.useGravity = false;

    } 
    
    private void Update() {
        TimeElapsed();
    }

    private void TimeElapsed()
    {
        if (Time.time > timeToWait)
        {
            myRigidBody.useGravity = true;
            myMeshRenderer.enabled = true;
        }
    }
}
