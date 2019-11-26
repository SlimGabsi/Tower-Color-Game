using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerElement : MonoBehaviour
{
    // Start is called before the first frame update
    public int FloorID;
    public int ColorID;
    public bool isLocked;
    public List<GameObject> SurroundingObjects;
    


    //Private variables
    private bool isTouched = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int floorId,int colorId,Material mat)
    {
        FloorID = floorId;
        ColorID = colorId;
        GetComponent<Renderer>().material = mat;
    }

    public void ObjectTouched()
    {
        if (!isTouched)
        {
            //This Boolean Variable (isTouched) to execute the function one time to avoid Stack overflow Bug
            isTouched = true;
            for (int i = 0; i < SurroundingObjects.Count; i++)
            {
                //Delete this GameObject to avoid its treatment another time
                TowerElement TE = SurroundingObjects[i].GetComponent<TowerElement>();
                TE.SurroundingObjects.Remove(gameObject);

                //Activate the ObjectTouched function of the surroundingObject if they have the same colorId
                if (TE.ColorID == ColorID && !TE.isLocked)
                {
                    TE.ObjectTouched();
                }
            }

            //Instantiate the particle system
            GameObject explosionParticleSystem = Instantiate(TowerColorGameManager.Instance.ExplosionParticleSystem, gameObject.transform.position, Quaternion.identity);
            explosionParticleSystem.GetComponent<ParticleSystemRenderer>().material = TowerColorGameManager.Instance.materials[ColorID];

            //Destroy the particle system after 1s
            Destroy(explosionParticleSystem, 1);

            //Disable this GameObject
            gameObject.SetActive(false);
        }
        else
        {
            return;
        }
        
    }

    //Lock the tower element with black material
    public void LockTowerElement()
    {
        isLocked = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Renderer>().material = TowerColorGameManager.Instance.BlackMaterial;
    }

    //Unlock the tower element with his color material
    public void UnlockTowerElement()
    {
        isLocked = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Renderer>().material = TowerColorGameManager.Instance.materials[ColorID];
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Add the collided Object to the surronding objects list
        if(collision.gameObject.tag == "TowerElement")
        {
            SurroundingObjects.Add(collision.gameObject);
        }
    }

    
    public void OnCollisionExit(Collision collision)
    {
        //Delete the collided Object to the surronding objects list
        if (collision.gameObject.tag == "TowerElement")
        {
            SurroundingObjects.Remove(collision.gameObject);
        }
    }
}
