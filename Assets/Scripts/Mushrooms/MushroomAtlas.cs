using Unity.Netcode;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
public class MushroomAtlas:NetworkBehaviour{
    private List<MushroomType> mushroomAtlas = new List<MushroomType>();
    private NetworkVariable<int> mushroomCounter = new NetworkVariable<int>(0);
    private DisplayMushroomAmountUI displayMushroomAmountUI;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        displayMushroomAmountUI = GetComponentInChildren<DisplayMushroomAmountUI>();
        mushroomCounter.OnValueChanged += OnMushroomCounterChanged;
    }



    [ServerRpc]
    public void addToAtlasServerRpc(MushroomType type){
            mushroomAtlas.Add(type);
            mushroomCounter.Value++;
    }
    public void addToAtlas(MushroomType type){
        if (IsOwner){
            mushroomAtlas.Add(type);
            //mushroomCounter.Value++;
            addToAtlasServerRpc(type);
        }
    }
    public void removeFromAtlas(MushroomType type){
        if(IsOwner){
            mushroomAtlas.Remove(type);
            mushroomCounter.Value--;
        }
    }

    public void consumeRandomMushroom(){
        if(IsOwner && mushroomAtlas.Count > 0){
            int randomIndex = UnityEngine.Random.Range(0, mushroomAtlas.Count);
            MushroomType consumedMushroom = mushroomAtlas[randomIndex];
            mushroomAtlas.RemoveAt(randomIndex);
            mushroomCounter.Value--;
            // Implement the effect of consuming the mushroom here
        }
    }

    public string showMushroomList(){
        StringBuilder sb = new StringBuilder("Mushroom list ("+mushroomCounter.Value+"):\n");
        foreach(MushroomType m in mushroomAtlas){
            Debug.Log(m.ToString());
            sb.Append(m.ToString()+"\n");
        }
        Debug.Log(sb.ToString());
        return sb.ToString();
    }

    public int getMushroomCounter(){
        return mushroomCounter.Value;
    }
    public NetworkVariable<int> getMushroomCounterVariable(){
        return mushroomCounter;
    }
    private void OnMushroomCounterChanged(int previousValue, int newValue)
    {
        displayMushroomAmountUI.OnMushroomAmountChange(newValue);
    }
}