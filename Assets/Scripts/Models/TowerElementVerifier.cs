using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElementVerifier : SingletonMB<TowerElementVerifier>
{
    // Start is called before the first frame update
    public int LastFloorTowerElementsNumber=0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TowerElement")
        {
            //Notify that there is an object which enter on the trigger (Unity Bug Fix)
            ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

            //Increment the number of tower elements in the last Floor
            LastFloorTowerElementsNumber++;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Notify that there is an object which exits the trigger(Unity Bug Fix : unable to detect Disabled Object with Ontrigger Exit)
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        //Decrement the number of tower elements in the last Floor
        LastFloorTowerElementsNumber--;

        //Verify the floor Number
        TowerColorGameManager.Instance.VerifyFloorNumber();

        //Disable the Tower Element Component
        other.gameObject.GetComponent<TowerElement>().enabled = false;
    }

}
