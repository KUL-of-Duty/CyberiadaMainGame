using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    // Przeciągnij tutaj komponent Slider z UI
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}