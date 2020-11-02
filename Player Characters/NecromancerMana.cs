using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerMana : MonoBehaviour
{
   public Slider slider;

   public void SetMana(float mana)
   {
       slider.value = mana;
   }

}