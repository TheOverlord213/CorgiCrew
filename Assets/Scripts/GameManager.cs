using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    List<GameObject> objectData = new  List<GameObject>();

    private bool objectWaitBool = false;
    public bool objectPickedUp = false;

    private float tempWaitChecker = 0.0f;

    public Material transMat;

    private GameObject player;
    public GameObject pickedUpObject;

    private Vector3 playerItemAnchorPoint = new Vector3 (0,0,0);

    private void Awake()
    {
        // sets up the objects data
        foreach (GameObject x in GameObject.FindGameObjectsWithTag("MovableObjects"))
        {
            objectData.Add(x);
            x.AddComponent<MovableObjectData>();
            x.AddComponent<Rigidbody>();
            x.AddComponent<SphereCollider>();

            x.GetComponent<SphereCollider>().isTrigger = true;
            float tempRadius = x.GetComponent<SphereCollider>().radius;
            x.GetComponent<SphereCollider>().radius = tempRadius * 1.5f;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        // sets up the anchor point for the player
        CreateAnchorPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!objectWaitBool)
        {
            ObjectWait();
        }

        if (objectPickedUp)
        {
            SetObject();
        }

    }

    void CreateAnchorPoint()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = playerPos.y + 2.0f;
        Vector3 playerDir = player.transform.forward;
        Quaternion playerRot = player.transform.rotation;
        float spawnDistance = 1.0f;

        playerItemAnchorPoint = playerPos + playerDir * spawnDistance - new Vector3(0, 1, 0);

    }

    void ObjectWait()
    {
        tempWaitChecker += Time.deltaTime;
        int seconds = Mathf.RoundToInt(tempWaitChecker % 60.0f);
        if (seconds == 2)
        {
            foreach (GameObject x in objectData)
            {       
                x.GetComponent<MovableObjectData>().objectInitialized = true;
                x.GetComponent<MovableObjectData>().transparentMat = transMat;
                x.GetComponent<MovableObjectData>().SetOrigin();
            }
            objectWaitBool = true;
        }

    }

    public void PlayerPickUpDetected(GameObject tempPickedUpObject)
    {
        Debug.Log("item picked up");
        objectPickedUp = true;
        pickedUpObject = tempPickedUpObject;

    }

    private void SetObject()
    {
        CreateAnchorPoint();
        pickedUpObject.transform.position = playerItemAnchorPoint;
        pickedUpObject.transform.rotation = Quaternion.LookRotation(pickedUpObject.transform.position - player.transform.position);
        pickedUpObject.GetComponent<SphereCollider>().enabled = false;
        pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
    }



}
