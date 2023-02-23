using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoundController : MonoBehaviour
{
    public AudioSource SoundSource;

    public AudioClip clip1, clip2, clip3;

    public void AttackSound()
    {
        SoundSource.clip = clip1;
        SoundSource.Play();
    }

    public void AttackHit()
    {
        SoundSource.clip = clip2;
        SoundSource.Play();
    }

    public void WoodMine()
    {
        SoundSource.clip = clip3;
        SoundSource.Play();
    }
}
