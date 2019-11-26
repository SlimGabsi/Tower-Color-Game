using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public int ColorID;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int colorId,Material mat)
    {
        ColorID = colorId;
        GetComponent<Renderer>().material = mat;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //If the projectile touch a tower element with the same color
        if (collision.gameObject.GetComponent<TowerElement>() != null && !collision.gameObject.GetComponent<TowerElement>().isLocked)
        {
            //If the ball is the super ball
            if(ColorID == TowerColorGameManager.Instance.materials.Length - 1) //If the color id equal to the last material id which is the material of the super ball
            {
                //Add Explosion force
                collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(12000000, collision.gameObject.transform.position,500);

                //Play super ball explosion
                SoundManager.Instance.PlaySuperBallExplosion();

                //Instantiate the explosion particle system
                GameObject explosionParticleSystem = Instantiate(TowerColorGameManager.Instance.SuperExplosionPartcileSystem, collision.transform.position, Quaternion.identity);

                //Destroy the particle system after 1s
                Destroy(explosionParticleSystem, 1);

                gameObject.SetActive(false);
            }
            //If the ball is simple ball
            else if(collision.gameObject.GetComponent<TowerElement>().ColorID == ColorID)
            {
                collision.gameObject.GetComponent<TowerElement>().ObjectTouched();
                
                //Play ball explosion
                SoundManager.Instance.PlayBallExplosion();
                
                gameObject.SetActive(false);
            }
            else
            {
                SoundManager.Instance.PlayCollisionSound();
            }
        }
        
    }
}
