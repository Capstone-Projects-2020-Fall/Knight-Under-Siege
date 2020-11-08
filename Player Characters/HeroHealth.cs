using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHealth : MonoBehaviour
{
   public Slider slider;

   public void SetHealth(float health)
   {
       slider.value = health;
   }
   
   public void SetStartingHealth(float health)
   {
       slider.value = health;
       slider.maxValue = health;
   }

}