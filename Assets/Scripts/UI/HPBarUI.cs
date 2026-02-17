using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : NetworkBehaviour
{
    [Header("Start fields")]
    public float Width=492, Height = 42;
    public PlayerHpSystem playerHpSystem;
    public RectTransform HpBar;
    public Image image;

    float healthRatio;

    [Header("HP bar colors")]
    public Color firstColor = new Color(32,197,21);
    public Color secondColor = new Color(153,163,5);
    public Color thirdColor = new Color(194,67,25);
    public Color fourthColor = new Color(23,23,23);

    [Header("HP bar ratios")]
    public float firstRatio = 0.5f;
    public float secondRatio = 0.25f;
    public float thirdRatio = 0.05f;



    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;
        healthRatio = playerHpSystem.targetHp.health.Value/playerHpSystem.targetHp.maxHealth;
    }
    public void OnSetHealth(float newValue)
    {
        healthRatio = newValue /playerHpSystem.targetHp.maxHealth;
        float newWidth = healthRatio*Width;
        if(healthRatio>=firstRatio) image.color = firstColor;
        else if(healthRatio>=secondRatio) image.color = secondColor;
        else if(healthRatio>=thirdRatio) image.color = thirdColor;
        else image.color = fourthColor;
        HpBar.sizeDelta = new Vector2(newWidth,Height);
    }
}
