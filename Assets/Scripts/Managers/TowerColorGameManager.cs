using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColorGameManager : SingletonMB<TowerColorGameManager>
{
    // Start is called before the first frame update
    [Header("Instantiation Parameters")]
    public GameObject TowerElementPrefab;
    public GameObject TowerParent;
    public GameObject ExplosionParticleSystem;
    public GameObject SuperExplosionPartcileSystem;
    public GameObject cam;
    public float radius;
    public int CylindersNumber;
    public int FloorNumber;
    public Material[] materials;
    public Material BlackMaterial;
    public List<GameObject> TowerElements;
    public Vector3 CameraFirstPosition;
    public Vector3 TEVerifierFirstPosition;

    [Header("Game Parameters")]
    public int ProjectileNumber;
    public int CurrentLastFloor;
    public GameObject towerElementVerifierObject;

    void Start()
    {
        //Instantiate Tower
        InstantiateTower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateTower()
    {
        
        for (int j = 0; j < FloorNumber; j++)
        {
            for (int i = 0; i < CylindersNumber; i++)
            {
                float angle;

                //If the floor Number is pair so we get the normal Angle else we add half angle (2*pi / 2* cylinderNumber) to the normal Angle to have an balanced tower
                if (j % 2 == 0)
                {
                    angle = i * Mathf.PI * 2f / CylindersNumber;
                }
                else
                {
                    angle = i * Mathf.PI * 2f / CylindersNumber + Mathf.PI / CylindersNumber;
                }
                //Calculate the Tower Element position according to its angle and floor number
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, j+1.5f, Mathf.Sin(angle) * radius);

                //Instantiate the Tower Element
                GameObject towerElement = Instantiate(TowerElementPrefab, newPos, Quaternion.identity,TowerParent.transform);
                int colorId = Random.Range(0, materials.Length-1);

                //Init the Tower Element Parameters
                towerElement.GetComponent<TowerElement>().Init(j, colorId, materials[colorId]);
                
                //Add the tower element to the elements list
                TowerElements.Add(towerElement);
            }
        } 
    }

    public void VerifyFloorNumber()
    {
        if(TowerElementVerifier.Instance.LastFloorTowerElementsNumber == 0)
        {
            CurrentLastFloor--;

            //Set the new position of the Tower Element Verifier Plane
            towerElementVerifierObject.transform.position = new Vector3(0, CurrentLastFloor + 0.5f, 0);

            if (CurrentLastFloor >= 8)
            {
                //Move the camera to the new Tower Center
                Vector3 To = new Vector3(cam.transform.position.x, CurrentLastFloor - 3, cam.transform.position.z);
                StopAllCoroutines();
                StartCoroutine(TranslateToDestination(cam, cam.transform.position, To, 0.5f));
            }
            
            LockTowerElements();

            //Game Win
            if (CurrentLastFloor <= 3)
            {
                GameWin();
            }
        }
    }

    public void BeginGame(int projectileNumber)
    {
        //Set the projectile Number
        ProjectileNumber = projectileNumber;

        //Set the floor Number
        CurrentLastFloor = FloorNumber;

        //Start the Begin Game Animation
        StartCoroutine(_BeginGame());
    }

    public IEnumerator _BeginGame()
    {
        //Move the camera to the top of the tower
        Vector3 To = new Vector3(cam.transform.position.x, CurrentLastFloor - 3, cam.transform.position.z);
        yield return StartCoroutine(MoveCamToTop(cam,To));

        //Lock the tower Elements
        LockTowerElements();

        //Move the tower element verifier object to the last floor
        towerElementVerifierObject.transform.position = new Vector3(0, CurrentLastFloor+0.5f, 0);

        //Activate the Screen Projectile
        InputManager.Instance.ScreenProjectile.SetActive(true);
        
        //Authorize to throw projectile
        InputManager.Instance.isThrowProjectile = true;
    }

    //Animate the camera translation to the top of the tower
    private IEnumerator MoveCamToTop(GameObject obj,Vector3 to)
    {
        while (obj.transform.position.y<to.y)
        {
            obj.transform.RotateAround(TowerParent.transform.position, -TowerParent.transform.up, Time.deltaTime * 200);
            obj.transform.Translate(obj.transform.up * Time.deltaTime * 5);
            yield return null;
        }
    }

    //Move an object to a specified destination
    private IEnumerator MoveToDestination(GameObject obj, Vector3 from, Vector3 to, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime<time)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(from, to, elapsedTime / time);
            yield return null;
        }
    }

    //Translate an object to a specified destination
    private IEnumerator TranslateToDestination(GameObject obj, Vector3 from, Vector3 to, float time)
    {
        
        while (obj.transform.position.y > to.y)
        {
            obj.transform.Translate(-obj.transform.up * Time.deltaTime * 5);
            yield return null;
        }
    }

    public void LockTowerElements()
    {
        for(int i = 0; i < TowerElements.Count; i++)
        {
            if (TowerElements[i].GetComponent<TowerElement>().FloorID < CurrentLastFloor - 8)
            {
                TowerElements[i].GetComponent<TowerElement>().LockTowerElement();
            }
            else
            {
                TowerElements[i].GetComponent<TowerElement>().UnlockTowerElement();
            }
        }
    }

    public void GameOver()
    {
        //Activate the game over text
        MainController.Instance.GameOverController();

        //Desactivate the  Screen Projectile
        InputManager.Instance.ScreenProjectile.SetActive(false);
    }

    public void GameWin()
    {
        //Disable the projectile throw
        InputManager.Instance.isThrowProjectile = false;

        //Activate the game Win text
        MainController.Instance.GameWinController();

        //Desactivate the  Screen Projectile
        InputManager.Instance.ScreenProjectile.SetActive(false);

        //Animate the camera
        Vector3 to = cam.transform.position - cam.transform.forward * 20 + cam.transform.up * 5;
        StartCoroutine(MoveToDestination(cam, cam.transform.position, to, 0.5f));
    }

    public void ResetGame()
    {
        StopAllCoroutines();

        //Destroy All Objects
        for(int i = 0; i < TowerElements.Count; i++)
        {
            Destroy(TowerElements[i]);
        }

        //Clear the list
        TowerElements.Clear();

        //Reset the Tower Element Verifier
        TowerElementVerifier.Instance.LastFloorTowerElementsNumber = 0;
        towerElementVerifierObject.transform.position = TEVerifierFirstPosition;

        //Instantiate the tower
        InstantiateTower();

        //Reset the camera position
        cam.transform.position = CameraFirstPosition;
        cam.transform.rotation = Quaternion.identity;

    }

    

}
