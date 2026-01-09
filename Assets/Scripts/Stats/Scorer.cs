using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Scorer : NetworkBehaviour{

    [SerializeField] Image[] hp = new Image[4];
    public NetworkVariable<float> score= new NetworkVariable<float>(100,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    private int ileHP=3;
    TextMeshProUGUI tmp;
    //Mover mover;
    CameraRotate cr;
    Vector3 end;
    void Start()
    {
        if(!IsOwner) return;
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        end = new Vector3(-tmp.transform.position.x/2,-tmp.transform.position.y/2,0);
        //mover = GetComponent<Mover>();
        cr = GetComponent<CameraRotate>();
    }
    void Update()
    {
        if(!IsOwner)return;
        healthChecker();
    }

    private void OnCollisionEnter(Collision other) {
            if (other.gameObject.tag == "Hazard")
            {
                ChangeScoreValueServerRpc(10);
            }
            else if(other.gameObject.tag == "Envrioment")
            {
                ChangeScoreValueServerRpc(1);
            }    
            tmp.text="HP: "+score.Value;
            
    }
    void healthChecker()
    {
        if(score.Value>0){
            if (score.Value / 25 < ileHP&&score.Value<100)
            {
                Destroy(hp[ileHP]);
                ileHP--;
            }
        }else{
                Destroy(hp[0]);
                ileHP--;
                ChangeScoreValueServerRpc(score.Value);
                tmp.transform.Translate(end);
                end=Vector3.zero;
                tmp.text="Koniec";

                GetComponent<Mover>().enabled =false;
                cr.enabled=false;
            }
        //Debug.Log("POsition: "+tmp.transform.position.x);
        Debug.Log(tmp.text);
    }

    [ServerRpc]
    void ChangeScoreValueServerRpc(float value)
    {
        score.Value -=value;
    }
}
