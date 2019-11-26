using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMB<SoundManager>
{
    // Start is called before the first frame update
    public AudioSource ThrowBallSound;
    public AudioSource BallExplosion;
    public AudioSource SuperBallExplosion;
    public AudioSource CollisionSound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayThrowBall()
    {
        ThrowBallSound.Stop();
        ThrowBallSound.Play();
    }

    public void PlayBallExplosion()
    {
        BallExplosion.Stop();
        BallExplosion.Play();
    }

    public void PlaySuperBallExplosion()
    {
        SuperBallExplosion.Stop();
        SuperBallExplosion.Play();
    }

    public void PlayCollisionSound()
    {
        CollisionSound.Stop();
        CollisionSound.Play();
    }
}
