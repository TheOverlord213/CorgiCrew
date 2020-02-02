using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class AudioSurfaceSwitch : MonoBehaviour
{
    public string SwitchGroup = "SurfaceMaterial";
    public string Switch = "Grass";
    public string ExitSwitch = "Grass";
    public GameObject player;


    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Switch surface Wwise footsteps");
            AkSoundEngine.SetSwitch(SwitchGroup, Switch, player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AkSoundEngine.SetSwitch(SwitchGroup, ExitSwitch, player);
        }
    }
}
