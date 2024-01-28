using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
   public AudioClip soundOnClick;
   public AudioSource source;

   public void PlayClip()
   {
      source.PlayOneShot(soundOnClick);
   }
}
