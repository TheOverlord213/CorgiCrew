﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectData : MonoBehaviour
{
    public Vector3 originPosition;
    public Quaternion originRotation;
    private float tempWaitChecker2 = 0.0f;

    public bool objectInitialized = false;
    private bool objectMoved = false;
    private bool ObjectSpawned = false;
    private bool resetPosition = false;
    private bool objectResetPos = false;

    private GameObject gameController;
    private GameObject player;
    public GameObject instantiatedObject;

    public Material transparentMat;



    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        player = GameObject.FindGameObjectWithTag("Player2");

    }

    private void Update()
    {
        if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player2");
        }

        if (objectInitialized)
        {
            CheckPos();
        }

        if (resetPosition)
        {
            ResetPos();
        }

        if (player.GetComponent<TestPlayerMovement>().objectReplaced)
        {
            CheckTransObj();
        }

        if (objectResetPos)
        {
            ReenableObject();
        }

    }

    public void SetOrigin()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    private void CheckPos()
    {
        float dist = Vector3.Distance(originPosition, transform.position);
        if (dist >= 0.5 && !objectMoved)
        {
            objectMoved = true;
            Debug.Log("object moved");
            if (!ObjectSpawned)
            {
                ObjectSpawned = true;
                SpawnObjectClone();
            }
            gameController.GetComponent<GameManager>().SliderValueChange(true);
        }
    }

    private void SpawnObjectClone()
    {
        instantiatedObject = Instantiate(this.gameObject, originPosition, originRotation);
        Destroy(instantiatedObject.GetComponent<Rigidbody>());
        Destroy(instantiatedObject.GetComponent<MeshCollider>());
        Destroy(instantiatedObject.GetComponent<MovableObjectData>());
        foreach (Collider c in instantiatedObject.GetComponents<Collider>())
        {
            c.enabled = false;
        }
        instantiatedObject.GetComponent<MeshRenderer>().material = transparentMat;
        instantiatedObject.tag = "SpawnedObject";

    }

    private void OnTriggerStay(Collider other)
    {
       
        if ((other.tag == "Player2" && objectMoved) && gameController.GetComponent<GameManager>().objectPickedUp == false)
        {
            gameController.GetComponent<GameManager>().TestDistance(this.gameObject);
        }
    }

    void ResetPos()
    {
        gameController.GetComponent<GameManager>().SliderValueChange(false);

        this.transform.position = originPosition;
        this.transform.rotation = originRotation;
        Destroy(instantiatedObject);


        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = true;
        }

        objectMoved = false;
        ObjectSpawned = false;
        gameController.GetComponent<GameManager>().objectPickedUp = false;
        resetPosition = false;
        objectResetPos = true;

        Debug.Log("item sucesfully placed");
    }

    public void CheckTransObj()
    {
        if (player.GetComponent<TestPlayerMovement>().transparentObjs == instantiatedObject)
        {
            resetPosition = true;
        }
        else
        {
            player.GetComponent<TestPlayerMovement>().objectReplaced = false;
        }
    }

    public void PickedUpObject()
    {
        if (Vector3.Distance(transform.position,instantiatedObject.transform.position)<=1.5f)
        {
            ResetPos();
            gameController.GetComponent<GameManager>().closeObj = null;
        }
    }

    private void ReenableObject()
    {
        tempWaitChecker2 += Time.deltaTime;
        int seconds = Mathf.RoundToInt(tempWaitChecker2 % 60.0f);
        if (seconds >= 4)
        {
            Debug.Log("Timer finshed");
            this.GetComponent<Rigidbody>().isKinematic = false;
            objectResetPos = false;
            tempWaitChecker2 = 0;
        }
    }

}
