using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;


public class CharacterScript : MonoBehaviour
{
    public GameScript gameScript;

    public Control actionMap;
    public float MovementSpeed = 0.1f;
    [SerializeField] private Vector2 DirectionValue, FlyValue;
    private int NewScore;

    private int i;
    private bool a;
    public GameObject BulletPrefab;
    private GameObject[] BulletObject;
    public List<int> Maxazine = new List<int> { 0, 1, 2, 3 };

    public SoundManager SoundManager;

    void Start()
    {
        a= true;
        gameScript = GameObject.Find("GameMain").GetComponent<GameScript>();
        actionMap = new Control();
        actionMap.Enable();

        BulletObject = new GameObject[4];
        for (i = 0; i < 4; i++)
        {
            // BulletObject[i] = Instantiate(BulletPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            BulletObject[i] = Instantiate(BulletPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f));
            BulletObject[i].GetComponent<BulletScript>().Id = i;
            BulletObject[i].GetComponent<BulletScript>().characterObject = this.gameObject;
            BulletObject[i].GetComponent<BulletScript>().gameScript = gameScript;
            BulletObject[i].SetActive(false);

        }

        SoundManager = FindObjectOfType<SoundManager>();
    }
    void Update()
    {
        if (gameScript.StateValue == GameState.Gameplay)
        { 
            DirectionValue = actionMap.Player.Movement.ReadValue<Vector2>();
            FlyValue = actionMap.Player.Flying.ReadValue<Vector2>();

            this.transform.Translate(new Vector3(0f, 0f, DirectionValue.y * MovementSpeed));
            this.transform.Rotate(new Vector3(0f, DirectionValue.x, 0f));
            this.transform.Translate(new Vector3(0f, FlyValue.y * MovementSpeed, 0f));

            if (actionMap.Player.Action.IsPressed() && Maxazine.Count != 0 && a)
            {
                SoundManager.PlayShoot();
                StartCoroutine(BulletAction());

            }
        }

    }


    IEnumerator BulletAction()
    {
        a = false;
        BulletObject[Maxazine[0]].SetActive(true);
        BulletObject[Maxazine[0]].transform.rotation = this.transform.rotation;
        BulletObject[Maxazine[0]].transform.position = this.transform.position;
        Maxazine.RemoveAt(0);
        StartCoroutine(BulletTime(BulletObject[Maxazine[0]]));
        yield return new WaitForSeconds(1f);
        a = true;
    }
    IEnumerator BulletTime(GameObject target)
    {

        yield return new WaitForSeconds(5f);
        target.GetComponent<BulletScript>().ReloadBullet();
   
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NormalEnemyTag" || other.tag == "BossTag")
        {
            if (gameScript.NumEater == 1 )
            {
                gameScript.CurrentTime-=5f;
            }else if (gameScript.NumEater == -1)
            {
                if (other.tag == "NormalEnemyTag")
                {
                    other.GetComponent<NormalEenemyObjectScript>().HealthPoint = -1;
                }
                else if (other.tag == "BossTag")
                {
                    other.GetComponent<BossObjectScript>().HealthPoint = -1;
                }
            }
            
        }else if (other.tag == "PointTag")
        {
            StartCoroutine(GetPoint(other.gameObject));
        }
    }

    IEnumerator GetPoint(GameObject Target)
    {
        SoundManager.PlayEatPoint();
        SoundManager.PlayEnemyIntermission();
        Target.SetActive(false);
        NewScore = gameScript.NumScore + 1;
        gameScript.UpdateScoreText(NewScore);
        yield return new WaitForSeconds(2f);
        gameScript.SpawnPoint(Target);

    }

}
