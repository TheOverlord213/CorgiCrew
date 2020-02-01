using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayer : MonoBehaviour
{
    public GameObject corgi;
    public GameObject human;
    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player") && GameObject.FindGameObjectWithTag("Player")!=this.gameObject)
        {
            Destroy(corgi);
            human.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
