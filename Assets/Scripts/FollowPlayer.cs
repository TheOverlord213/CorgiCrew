using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;//public becouse there has to be a camera for each player

    private Vector3 distVector;
    private Vector3 rightPos;
    private float lerpSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        distVector = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rightPos = player.transform.position + distVector;
        if(rightPos!=transform.position)
        {
            //move camera
            Vector3 currentPos = transform.position;

            currentPos.y = Mathf.Lerp(this.transform.position.y, rightPos.y, lerpSpeed);
            currentPos.x = Mathf.Lerp(this.transform.position.x, rightPos.x, lerpSpeed);
            currentPos.z = Mathf.Lerp(this.transform.position.z, rightPos.z, lerpSpeed);

            this.transform.position = currentPos;
        }
    }
}
