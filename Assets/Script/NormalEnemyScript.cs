using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyScript : MonoBehaviour
{
    public int Id;
    private int NewScore;
    public GameScript gameScript;
    public GameObject characterObject;
    private Transform normalEnemyObject;

    public float NormalEnemySpeed, n = 0, b = 0, DirectionNumber;
    private int a;
    private bool CalculatedState;
    private List<Vector3> vectorDirection = new List<Vector3>() { new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f) };
    private Vector3 characterPosition, targetPosition;

    public SoundManager SoundManager;

    // Start is called before the first frame update
    void Start()
    {
        normalEnemyObject = this.transform.Find("NormalEnemyObject");
        CalculatedState = true;

        SoundManager = FindObjectOfType<SoundManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (normalEnemyObject.gameObject.GetComponent<NormalEenemyObjectScript>().HealthPoint < 1)
        {
            // SoundManager.PlayEnemyDeath();
            StartCoroutine(Death());
        }

        if (CalculatedState)
        {
            CalculatedState = false;

            characterPosition = characterObject.transform.position;
            targetPosition = characterPosition - this.transform.position;
            a = Random.Range(0, 2);

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
                DirectionNumber = -1 * gameScript.NumEater;
            }
            else if (b > 0)
            {
                DirectionNumber = 1 * gameScript.NumEater;
            }
        }
        else
        {
            if (b > NormalEnemySpeed || b < -NormalEnemySpeed)
            {
                this.transform.Translate(vectorDirection[a] * NormalEnemySpeed * DirectionNumber);
                b = b - (NormalEnemySpeed * DirectionNumber * gameScript.NumEater);
            }
            else
            {
                this.transform.Translate(vectorDirection[a] * b);
                CalculatedState = true;
            }
        }

    }

    IEnumerator Death()
    {

        normalEnemyObject.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        gameScript.SpawnEnemy();

        NewScore = gameScript.NumScore + 2;
        gameScript.StoreEnemySpawn.Add(Id);
        gameScript.UpdateScoreText(NewScore);
        normalEnemyObject.gameObject.SetActive(true);
        normalEnemyObject.gameObject.GetComponent<NormalEenemyObjectScript>().HealthPoint = Random.Range(2, 3);
        this.gameObject.SetActive(false);    
    }
}
