using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip StartGame;
    public AudioClip Shoot;
    public AudioClip GameOver;
    public AudioClip EatPoint;
    /*public AudioClip EnemyDeath;*/
    public AudioClip EnemyIntermission;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayStartGame()
    {
        StartCoroutine(StartPlayStartGame());
        
    }

    IEnumerator StartPlayStartGame()
    {
        AudioSource audioSourceStartGame = GetComponent<AudioSource>();
        audioSourceStartGame.volume = 1.0f;
        // audioSourceStartGame.clip = StartGame;
        audioSourceStartGame.PlayOneShot(StartGame);
        yield return null;
    }

    public void PlayGameOver()
    {
        AudioSource audioSourceGameOver = GetComponent<AudioSource>();
        audioSourceGameOver.volume = 1.5f;
        // audioSourceGameOver.clip = GameOver;
        audioSourceGameOver.PlayOneShot(GameOver);
    }

    public void PlayShoot()
    {
        StartCoroutine(StartPlayShoot());
    }

    IEnumerator StartPlayShoot()
    {
        AudioSource audioSourceShoot = GetComponent<AudioSource>();
        audioSourceShoot.volume = 1.0f;
        // audioSourceShoot.clip = Shoot;
        audioSourceShoot.PlayOneShot(Shoot);
        yield return null;
    }

    public void PlayEatPoint()
    {
        AudioSource audioSourceEatPoint = GetComponent<AudioSource>();
        audioSourceEatPoint.volume = 1.0f;
        // audioSourceEatPoint.clip = EatPoint;
        audioSourceEatPoint.PlayOneShot(EatPoint);
    }

    public void PlayEnemyIntermission()
    {
        AudioSource audioSourceEnemyIntermission = GetComponent<AudioSource>();
        audioSourceEnemyIntermission.volume = 0.5f;
        audioSourceEnemyIntermission.clip = EnemyIntermission;
        audioSourceEnemyIntermission.Play();
    }

    /*public void PlayEnemyDeath()
    {
        StartCoroutine(StartPlayEnemyDeath());

        *//*AudioSource audioSourceEnemyDeath = GetComponent<AudioSource>();
        audioSourceEnemyDeath.volume = 0.3f;
        *//*audioSourceEnemyDeath.clip = EnemyDeath;
        audioSourceEnemyDeath.Play();*//*
        // audioSourceEnemyDeath.PlayOneShot(EnemyDeath);*//*
    }

    IEnumerator StartPlayEnemyDeath()

    {
        AudioSource audioSourceEnemyDeath = GetComponent<AudioSource>();
        audioSourceEnemyDeath.volume = 0.2f;
        *//*audioSourceEnemyDeath.clip = EnemyDeath;
        audioSourceEnemyDeath.Play();*//*
        yield return new WaitForSeconds(0.5f);
        audioSourceEnemyDeath.PlayOneShot(EnemyDeath);

        // yield return new WaitForSeconds(0.5f);
        // yield return null;
    }*/
}
