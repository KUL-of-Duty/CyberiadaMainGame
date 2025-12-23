using Unity.Cinemachine;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class CameraRotate : NetworkBehaviour{
    
    CinemachineCamera CameraObject;
    public Vector3 offset;
    Rigidbody parentBody;
    [SerializeField] float Sensitivity = 1f;
    Vector2 mouseAxis;
    float validView=0;
    float deltaTime=0;
    PlayerInput input;
    InputAction lookAction;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        lookAction = input.actions["Look"];
        CameraObject = GetComponentInChildren<CinemachineCamera>();
        parentBody = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            CameraObject.Priority = 1;
        }
        else
        {
            CameraObject.Priority = 0;
        }
    }
    void Start()
    {
        Debug.Log($"[Camera Start] NetworkObjectId={NetworkObjectId} IsOwner={IsOwner} IsServer={IsServer} IsClient={IsClient} IsLocalPlayer={IsLocalPlayer}");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        deltaTime = Time.deltaTime;
        //Debug.Log("Sens: "+Sensitivity+" Vector: "+mouseAxis);
        Vector2 look = lookAction.ReadValue<Vector2>();
        mouseAxis = new Vector2(look.x,-look.y);
        if(IsServer&&IsOwner)
            cameraOnMouseReaction(mouseAxis,deltaTime);
        else if(IsClient && IsOwner)
        {
            CameraOnMouseReactionServerRpc(mouseAxis,deltaTime);
        }
        //Debug.Log(validView);
        //CameraObject.transform.Rotate(mouseAxis.y,0,0);
    }
    void cameraOnMouseReaction(Vector2 mouseAxis,float deltaTime, ServerRpcParams rpcParams = default)
    {
        //Debug.Log(mouseAxis);
        if(!IsServer) return;
        parentBody.transform.Rotate(0,mouseAxis.x*deltaTime*Sensitivity, 0);
        validView = CameraObject.transform.eulerAngles.x-180>0?CameraObject.transform.eulerAngles.x-180:
                                                                CameraObject.transform.eulerAngles.x+180; 
        
        mouseAxis.y=Mathf.Clamp(mouseAxis.y*deltaTime*Sensitivity+validView,93,267)-validView;
        CameraObject.transform.Rotate(mouseAxis.y,0,0);
        //CameraObject.transform.rotation.Set(CameraObject.transform.rotation.x,transform.rotation.y,0,0);
    }

[ServerRpc]
    void CameraOnMouseReactionServerRpc(Vector2 input,float deltaTime)
    {
        cameraOnMouseReaction(input,deltaTime);
    }
}