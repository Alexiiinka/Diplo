using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashMove : MonoBehaviour
{
    private float moveDistance = 2.8f;
    private float maxLeft = -13.0f, maxRight = -0.7f, maxUp = 10.0f , maxDown = -6.3f ;
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
        moveDistance = scriptSc.stepTiles;
        maxLeft = scriptSc.leftDownTileX - 0.5f;
        maxDown = scriptSc.leftDownTileY - 0.5f;
        maxRight = scriptSc.leftDownTileX + (scriptSc.muchTilesX - 1)*moveDistance + 0.5f;
        maxUp = scriptSc.leftDownTileY + (scriptSc.muchTilesY - 1)*moveDistance + 0.5f;

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
        print("Vykonaj dopredu");
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
            Destroy(other.gameObject);
        }
        else
        {   if (other.tag != "Stena")
            {
                scriptSc.jeVCieliZelenom = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Odpad" && other.tag != "Stena")
        {
            scriptSc.jeVCieliZelenom = false;
        }
    }
}
