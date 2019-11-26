using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMB<InputManager>
{
    // Start is called before the first frame update
    [Header("Camera Rotation Variables")]
    public GameObject TowerCenter;
    public GameObject cam;
    public float TouchSensitivity;

    [Header("Throw Projectile Variables")]
    public GameObject Projectile;
    public GameObject ScreenProjectile;
    public float ProjectileForcePower;
    int CurrentColorId;
    public bool isThrowProjectile = false; 

    //Private Variables
    RaycastHit hit;
    int layer = 1 << 4;
    float TouchTime;

    void Start()
    {
        InitScreenProjectile();
    }

    public void InitScreenProjectile()
    {
        //Init Screen Projectile
        CurrentColorId = Random.Range(0, TowerColorGameManager.Instance.materials.Length);
        ScreenProjectile.GetComponent<Projectile>().Init(CurrentColorId, TowerColorGameManager.Instance.materials[CurrentColorId]);
    }

    // Update is called once per frame
    private void Update()
    {
        TouchScreen();
    }

    public void TouchScreen()
    {
        if (Input.touchCount == 1 && isThrowProjectile)
        {
            //Get the touch
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                //Rotate the Camera if there is a touch movement
                cam.transform.RotateAround(TowerCenter.transform.position, TowerCenter.transform.up, Time.deltaTime * touch.deltaPosition.x * TouchSensitivity);

                //Increment the touch time
                TouchTime += touch.deltaTime;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 5000, layer)  && TouchTime <0.2f) // If there is no touch movement
                {
                    //Init Projectile component
                    Projectile.GetComponent<Projectile>().Init(CurrentColorId, TowerColorGameManager.Instance.materials[CurrentColorId]);
                    //Get the Force direction
                    Vector3 ForceDirection = hit.point - ScreenProjectile.transform.position;

                    //Reset the Projectile
                    Projectile.SetActive(true);
                    Projectile.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    Projectile.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                    Projectile.transform.position = ScreenProjectile.transform.position;
                    Projectile.transform.rotation = Quaternion.identity;

                    //Play Throw Ball sound
                    SoundManager.Instance.PlayThrowBall();

                    //Throw the projectile
                    Projectile.GetComponent<Rigidbody>().AddForce(ForceDirection * ProjectileForcePower, ForceMode.VelocityChange);

                    //Init another projectile
                    CurrentColorId = Random.Range(0, TowerColorGameManager.Instance.materials.Length);
                    ScreenProjectile.SetActive(false);
                    ScreenProjectile.GetComponent<Projectile>().Init(CurrentColorId, TowerColorGameManager.Instance.materials[CurrentColorId]);
                    
                    //Activate the screenProjectile Animation
                    ScreenProjectile.SetActive(true);

                    //Decrement the Projectile Number
                    TowerColorGameManager.Instance.ProjectileNumber--;

                    //Update the Projectile Number Text in the screen
                    MainController.Instance.ChangeProjectileNumber();

                    //if Projectile number finished GameOver
                    if (TowerColorGameManager.Instance.ProjectileNumber == 0)
                    {
                        isThrowProjectile = false;
                        TowerColorGameManager.Instance.GameOver();
                    }
                }

                //Reset the touch time
                TouchTime = 0;
            }
        }
    }
    
}
