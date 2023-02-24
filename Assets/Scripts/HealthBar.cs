
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image healthBarImage; 
    public Creature creature; 
    public void UpdateHealthBar()
    {
        GetComponent<Slider>().maxValue = creature.MaxHP;
        GetComponent<Slider>().minValue = 0;
        GetComponent<Slider>().value = Mathf.Clamp(creature.currentHP, 0, creature.MaxHP);
    }

    private void Update()
    {
        if (creature != null)
        {
            UpdateHealthBar();
        }
    }
}