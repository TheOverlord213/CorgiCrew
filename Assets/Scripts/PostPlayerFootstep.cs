using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostPlayerFootstep : MonoBehaviour
{
    public AK.Wwise.Event WwiseEvent;
    // Start is called before the first frame update
    public void PlayFootstepSound()
    {
        AkSoundEngine.PostEvent("Foot_Player", gameObject);
        WwiseEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
