using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    private int playerSpeed = 3;

    private GameObject gameController;
    public GameObject transparentObjs;


    public bool objectReplaced = false;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        // allows the player to control their movement movement
        //PlayerMovement();
       // CloseToTransObj();
    }

    void PlayerMovement()
    {
        float moveHor = Input.GetAxis("Horizontal");
        float moveVer = Input.GetAxis("Vertical");

        Vector3 position = transform.position;
        position.x += moveHor * playerSpeed * Time.deltaTime;
        position.z += moveVer * playerSpeed * Time.deltaTime;
        transform.position = position;
    }

    public void CloseToTransObj()
    {
      gameController.GetComponent<GameManager>().pickedUpObject.GetComponent<MovableObjectData>().PickedUpObject();
    }

    

}


