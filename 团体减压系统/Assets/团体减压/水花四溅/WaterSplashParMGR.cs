using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*转为UTF-8*/
public class WaterSplashParMGR : MonoBehaviour
{

    public List<ParticleSystem> particleSystems;
    public void PlayParticle(int index)
    {
        particleSystems[index].Play();
    }
         
}
