using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Scorer : NetworkBehaviour{

    [SerializeField] Image[] hp = new Image[4];
    private NetworkVariable<int> score= new NetworkVariable<int>(100,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);
    private int ileHP=3;
    [SerializeField] TextMeshProUGUI tmp;
    //Mover mover;
    CameraRotate cr;
    Vector3 end;
    void Start()
    {
        end = new Vector3(-tmp.transform.position.x/2,-tmp.transform.position.y/2,0);
        //mover = GetComponent<Mover>();
        cr = GetComponent<CameraRotate>();
    }
    void Update()
    {
        healthChecker();
    }

    private void OnCollisionEnter(Collision other) {
            if (other.gameObject.tag == "Hazard")
            {
                score.Value -= 10;
            }
            else if(other.gameObject.tag == "Envrioment")
            {
                score.Value--;
            }    
            tmp.text="HP: "+score;
            
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
                score.Value=0;
                tmp.transform.Translate(end);
                end=Vector3.zero;
                tmp.text="Koniec";

                GetComponent<Mover>().enabled =false;
                cr.enabled=false;
            }
        Debug.Log("pozycja: "+tmp.transform.position.x);
    }
}
