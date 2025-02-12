using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
/*转为UTF-8*/

/// <summary>
/// alembic动画控制器
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class AlembicDirectorControl : MonoBehaviour
{
    private PlayableDirector director;
    public PlayableDirector Director
    {
        get
        {
            if (director == null)
            {
                director = GetComponent<PlayableDirector>();
            }
            return director;
        }
    }


    private void Awake()
    {
        Director.Pause();
    }

    public void Play(float speed) 
    {
        Director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        Director.Play();
    }

}
