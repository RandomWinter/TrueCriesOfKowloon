using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip bonkSound, emotionalDamageSound, enemyDeathSound, fireSound, footstepSound, glassBreakSound, knifeSound, knifeSlashSound, punchSound, rageSound;
    static AudioSource audioSrc;

    void Start()
    {
        bonkSound = Resources.Load<AudioClip>("Bonk");
        emotionalDamageSound = Resources.Load<AudioClip>("EmotionalDamage");//done
        enemyDeathSound = Resources.Load<AudioClip>("EnemyDeath");//done
        fireSound = Resources.Load<AudioClip>("Fire");
        footstepSound = Resources.Load<AudioClip>("Footstep");//done
        glassBreakSound = Resources.Load<AudioClip>("GlassBreak");
        knifeSound = Resources.Load<AudioClip>("Knife");
        knifeSlashSound = Resources.Load<AudioClip>("KnifeSlash");
        punchSound = Resources.Load<AudioClip>("Punch");//done
        rageSound = Resources.Load<AudioClip>("Rage");//done

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "Bonk":
            audioSrc.PlayOneShot(bonkSound);
            break;

            case "EmotionalDamage":
            audioSrc.PlayOneShot(emotionalDamageSound);
            break;

            case "EnemyDeath":
            audioSrc.PlayOneShot(enemyDeathSound);
            break;

            case "Fire":
            audioSrc.PlayOneShot(fireSound);
            break;

            case "Footstep":
            audioSrc.PlayOneShot(footstepSound);
            break;

            case "GlassBreak":
            audioSrc.PlayOneShot(glassBreakSound);
            break;

            case "Knife":
            audioSrc.PlayOneShot(knifeSound);
            break;

            case "KnifeSlash":
            audioSrc.PlayOneShot(knifeSlashSound);
            break;

            case "Punch":
            audioSrc.PlayOneShot(punchSound, 0.1f);
            break;

            case "Rage":
            audioSrc.PlayOneShot(rageSound);
            break;
        }
    }
}
