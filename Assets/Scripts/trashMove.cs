using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashMove : MonoBehaviour
{
    public float moveDistance = 2.8f;
    public float maxLeft = -13.0f, maxRight = -0.7f, maxUp = 10.0f , maxDown = -6.3f ;
    Rigidbody trashRb;
    private Vector3 previousPos, newPos;
    private checkSpace checkingSpaceScript;
    public GameObject checker;
    sceneScript scriptSc;

    void Start()
    {
        trashRb = gameObject.GetComponent<Rigidbody>();
        checkingSpaceScript = checker.GetComponent<checkSpace>();
        scriptSc = GameObject.Find("SceneManager").GetComponent<sceneScript>();
    }

    // void FixedUpdate()
    // {
    //     if (transform.position.x < maxLeft || transform.position.x > maxRight  || transform.position.y < maxDown || transform.position.y > maxUp)
    //     {
    //          trashRb.position = previousPos;
    //     }
    // }

    public void moveFd()
    {
        if (scriptSc.canMove && checkingSpaceScript.freeSpace)
        {
            newPos = trashRb.position + transform.up * moveDistance;
            if (newPos.x < maxLeft || newPos.x > maxRight  || newPos.y < maxDown || newPos.y > maxUp || !checkingSpaceScript.freeSpace)
            {
                //
            }
            else
            {
                //previousPos = trashRb.position;
                trashRb.MovePosition(newPos);
                checkingSpaceScript.freeSpace = true;
            }
        }
        
    }

    public void rightRotate()
    {
        if (scriptSc.canMove)
        {
            transform.Rotate(Vector3.back * 90);
            checkingSpaceScript.freeSpace = true;
        }
    }

    public void leftRotate()
    {
        if (scriptSc.canMove)
        {
            transform.Rotate(Vector3.forward * 90);
            checkingSpaceScript.freeSpace = true;
        }
    }

    // void OnCollisionEnter(Collision other)
    // {
    //     trashRb.position = previousPos;
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Odpad")
        {
            print("juhu");
            Destroy(other.gameObject);
        }
        else
        {
            scriptSc.jeVCieliZelenom = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Odpad")
        {
            scriptSc.jeVCieliZelenom = false;
        }
    }
}
