using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostDogFootstep : MonoBehaviour
{
    public AK.Wwise.Event WwiseEvent;
    // Start is called before the first frame update
    public void PlayDogFootstepSound()
    {
        AkSoundEngine.PostEvent("dog_foot", gameObject);
        WwiseEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
