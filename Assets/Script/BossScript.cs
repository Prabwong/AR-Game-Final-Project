using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public int Id;
    private int NewScore;
    public GameScript gameScript;
    public GameObject characterObject;
    private Transform bossObject;

    public float BossSpeed, n=0,b=0, DirectionNumber;
    private int a;
    private bool CalculatedState;
    private List<Vector3> vectorDirection= new List<Vector3>() { new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f) };
    private Vector3 characterPosition,targetPosition;

    public SoundManager SoundManager;

    // Start is called before the first frame update
    void Start()
    {
        bossObject = this.transform.Find("BossObject");
        CalculatedState = true;

        SoundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossObject.gameObject.GetComponent<BossObjectScript>().HealthPoint < 1)
        {
            // SoundManager.PlayEnemyDeath();
            StartCoroutine(Death());
        }

        if (CalculatedState)
        {
            CalculatedState=false;

            characterPosition = characterObject.transform.position;
            targetPosition = characterPosition-this.transform.position;
            a = Random.Range(0,2);

            b = targetPosition[a];
            if (b > 1f)
            {
                b = b / 2;
            }
            while (b == 0 && targetPosition != Vector3.zero)
            {
                a = (a + 1) % 3;
                b = targetPosition[a];
            }
            if (b < 0)
            {
                DirectionNumber = -1* gameScript.NumEater;
            }
            else if (b > 0)
            {
                DirectionNumber = 1* gameScript.NumEater;
            }
            if (gameScript.NumEater==-1)
            {
                b = (BossSpeed * DirectionNumber * gameScript.NumEater) * 20f;
            }
        }
        else
        {
            if (b > BossSpeed || b < -BossSpeed)
            {
                this.transform.Translate(vectorDirection[a] * BossSpeed * DirectionNumber);
                b = b-(BossSpeed*DirectionNumber*gameScript.NumEater);
            }
            else
            {
                this.transform.Translate(vectorDirection[a] * b);
                CalculatedState=true;
            }
        }
    }

    IEnumerator Death()
    {

        bossObject.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        gameScript.SpawnEnemy();

        NewScore = gameScript.NumScore + 4;
        gameScript.StoreEnemySpawn.Add(Id);
        gameScript.UpdateScoreText(NewScore);
        bossObject.gameObject.SetActive(true);
        bossObject.gameObject.GetComponent<BossObjectScript>().HealthPoint = Random.Range(4, 5);
        this.gameObject.SetActive(false);
    }

}
