using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XomracLabs;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioBanks audioBank;

    public void PlayRandomClip()
    {
        AudioManager.instance.PlayClip(audioBank.clips[Random.Range(0,audioBank.clips.Count)]);
    }

    
}
