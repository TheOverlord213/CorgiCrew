using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBarkEvent : MonoBehaviour
{
    public AK.Wwise.Event WwiseEvent;
    // Start is called before the first frame update
    public void PlayBarkSound()
    {
        AkSoundEngine.PostEvent("bark", gameObject);
        WwiseEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
