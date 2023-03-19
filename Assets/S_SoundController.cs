using UnityEngine;

public class S_SoundController : MonoBehaviour
{
    public AudioSource SoundSource;

    public AudioClip clip1, clip2, clip3, clip4, clip5;

    public void AttackSound()
    {
        SoundSource.clip = clip1;
        SoundSource.PlayOneShot(clip1);
    }

    public void AttackHit()
    {
        SoundSource.clip = clip2;
        SoundSource.PlayOneShot(clip2);
    }

    public void WoodMine()
    {
        SoundSource.clip = clip3;
        SoundSource.PlayOneShot(clip3);
    }

    public void Dash()
    {
        SoundSource.clip = clip4;
        SoundSource.PlayOneShot(clip4);
    }

    public void HurtMonster()
    {
        SoundSource.clip = clip5;
        SoundSource.PlayOneShot(clip5);
    }
}
