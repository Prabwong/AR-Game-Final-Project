using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using Core;
using TMPro;
using System.Linq;

public class GameScript : MonoBehaviour
{ 
    public GameObject targetIndicator;
    public GameObject objectToPlace; // The object you want to place
    public ARRaycastManager ray;
    public Camera xrCamera;
    private Pose PlacementPose;
    private bool canPlace = false;

    public GameState StateValue;

    public Control actionMap;
    
    public GameObject ScanningStateObject, GameplayStateObject, EndedStateObject;
    public TextMeshProUGUI NotifyUI, TimeUI, ScoreUI;
    public float CurrentTime;
    public int NumScore,NumEater;
    private int PreviousScore;


    public GameObject CharacterPrefab, CharacterObject;
    // public GameObject NormalEnemyPrefab, BossPrefab, PointPrefab;
    public GameObject BossPrefab, PointPrefab;
    public GameObject NormalEnemyPrefab1, NormalEnemyPrefab2, NormalEnemyPrefab3, NormalEnemyPrefab4;
    private GameObject[] EnemylistObject,PointObject;
    public List<int> StoreEnemySpawn = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    private float k = 10f;
    private int j,i;

    public SoundManager SoundManager;


    void Start()
    {
        StateValue = GameState.Scanning;

        NumEater = 1;

        NotifyUI.text = "";
        TimeUI.text = "Time : 00";
        ScoreUI.text = "Score : 0";
        NumScore = 0;

        actionMap = new Control();
        actionMap.Enable();
        CharacterObject = Instantiate(CharacterPrefab, new Vector3(0f, 0f, 0f),Quaternion.Euler(0f,0f,0f));
        CharacterObject.SetActive(false);

        EnemylistObject = new GameObject[10];
        PointObject = new GameObject[10];
        for(i = 0; i <10; i++)
        {
            PointObject[i]=Instantiate(PointPrefab,new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            PointObject[i].SetActive(false);
        }

        for (i = 0; i <3; i++)
        {
            EnemylistObject[i] = Instantiate(BossPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            EnemylistObject[i].GetComponent<BossScript>().Id = i;
            EnemylistObject[i].GetComponent<BossScript>().gameScript = this.gameObject.GetComponent<GameScript>();
            EnemylistObject[i].GetComponent<BossScript>().characterObject = CharacterObject;
            EnemylistObject[i].SetActive(false);
        }
        for (i = 3; i <10; i++)
        {
            int j = UnityEngine.Random.Range(0, 3);

            if (j == 0)
            {
                EnemylistObject[i] = Instantiate(NormalEnemyPrefab1, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            }
            else if (j == 1)
            {
                EnemylistObject[i] = Instantiate(NormalEnemyPrefab2, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            }
            else if (j == 2)
            {
                EnemylistObject[i] = Instantiate(NormalEnemyPrefab3, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            }
            else if (j == 3)
            {
                EnemylistObject[i] = Instantiate(NormalEnemyPrefab4, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            }

            // EnemylistObject[i] = Instantiate(NormalEnemyPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            EnemylistObject[i].GetComponent<NormalEnemyScript>().Id = i;
            EnemylistObject[i].GetComponent<NormalEnemyScript>().gameScript = this.gameObject.GetComponent<GameScript>();
            EnemylistObject[i].GetComponent<NormalEnemyScript>().characterObject = CharacterObject;
            EnemylistObject[i].SetActive(false);
        }
        
        ScanningStateObject.SetActive(true);
        GameplayStateObject.SetActive(false);
        EndedStateObject.SetActive(false);

        SoundManager = FindObjectOfType<SoundManager>();



    }

    void Update()
    {
        if (StateValue == GameState.Scanning)
        {

            UpdatePlacementPose();
            UpdateTargetIndicator();

            if (actionMap.Player.Action.IsPressed() && canPlace)
            {
                MoveToGameplayState();
            }
        }else if (StateValue == GameState.Gameplay)
        {

            CurrentTime -= Time.deltaTime;
            if (CurrentTime > 0)
            {
                if (NumScore - PreviousScore > 4)
                {
                    StartCoroutine(EaterTime());
                }
                UpdateTimerText();
            }
            else
            {
                MoveToEndedState();
            }

        }else if (StateValue == GameState.Ended) 
        {
            
            if (actionMap.Player.Action.IsPressed())
            {
               MoveToGameplayState();
            }
            if (actionMap.Player.ExitAction.IsPressed())
            {
               MoveToScanningState();
            }
            
        }
        

        
    }

    void UpdatePlacementPose()
    {
        Vector3 screenCenter = xrCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        ray.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        canPlace = hits.Count > 0;

        if (canPlace)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void UpdateTargetIndicator()
    {
        if (canPlace)
        {
            targetIndicator.SetActive(true);
            targetIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            targetIndicator.SetActive(false);
        }
    }

    void MoveToScanningState()
    {
        for (int i = 0; i < 10; i++)
        {
            EnemylistObject[i].SetActive(false);
        }

        CharacterObject.SetActive(false);

        ScanningStateObject.SetActive(true);
        GameplayStateObject.SetActive(false);
        EndedStateObject.SetActive(false);

        NotifyUI.text = "";
        TimeUI.text = "Time : 00";
        ScoreUI.text = "Score : 0";
        NumScore = 0;

        StateValue = GameState.Scanning;

    }

    void MoveToGameplayState()
    {
        SoundManager.PlayStartGame();

        for (int i = 0; i < 10; i++)
        {
            EnemylistObject[i].SetActive(false);
            PointObject[i].SetActive(false);
        }

        StoreEnemySpawn = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        StartCoroutine(StartCountdown());

        CharacterObject.SetActive(true);
        CharacterObject.transform.position = new Vector3 (PlacementPose.position.x, PlacementPose.position.y+1f, PlacementPose.position.z);
        CharacterObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy();
        }

        for(int i = 0;i < 10; i++)
        {
            SpawnPoint(PointObject[i]);
        }

        CurrentTime = 90f;

        ScanningStateObject.SetActive(false);
        GameplayStateObject.SetActive(true);
        EndedStateObject.SetActive(false);

        NotifyUI.text = "";
        TimeUI.text = "Time : 00";
        ScoreUI.text = "Score : 0";
        NumScore = 0;

        StateValue = GameState.Gameplay;
    }

    public void MoveToEndedState()
    {
        SoundManager.PlayGameOver();

        ScanningStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        EndedStateObject.SetActive(true);
        NotifyUI.text = "Finish!!!";

        StateValue = GameState.Ended;
    }
    IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            NotifyUI.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        NotifyUI.text = "Go!"; // Or start your game logic here
        yield return new WaitForSeconds(1.0f);
        NotifyUI.text = "";


    }

    IEnumerator EaterTime()
    {
        NumEater = -1;
        yield return new WaitForSeconds(5.0f);
        NumEater = 1;
    }
    public void SpawnEnemy()
    {
        j = Random.Range(0,StoreEnemySpawn.Count);
        EnemylistObject[StoreEnemySpawn[j]].SetActive(true);
        EnemylistObject[StoreEnemySpawn[j]].transform.position = PlacementPose.position + new Vector3(Random.Range(-k,k), 0f, Random.Range(-k, k));
        EnemylistObject[StoreEnemySpawn[j]].transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        StoreEnemySpawn.RemoveAt(j);
       
    }

    public void SpawnPoint(GameObject PointTarget)
    {
        PointTarget.SetActive(true);
        PointTarget.transform.position = PlacementPose.position + new Vector3(Random.Range(-k, k), 0f, Random.Range(-k, k));
        PointTarget.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    public void UpdateScoreText(int newscore)
    {
        NumScore = newscore;
        ScoreUI.text = "Score :" + NumScore;
    }
    void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(CurrentTime % 90f);
        TimeUI.text = "Time : " + string.Format("{0:00}", seconds);
    }

 
}
