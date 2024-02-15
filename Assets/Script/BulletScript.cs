using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float Speed=0.1f;
    private int CriticalRate,Damage;
    public GameObject characterObject;
    public GameScript gameScript;
    public NormalEnemyScript normalEnemyScript;
    public BossScript bossScript;
    public int Id;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0f, 0f, Speed));
    }

    private void OnTriggerEnter(Collider other)
    {

        CriticalRate = Random.Range(0, 3);
        if (CriticalRate == 3)
        {
            Damage = 2;
        }
        else
        {
            Damage = 1;
        }


        if (other.tag == "NormalEnemyTag")
        {
            other.GetComponent<NormalEenemyObjectScript>().HealthPoint -= Damage;
            ReloadBullet();
            
        }
        else if (other.tag == "BossTag")
        {
            other.GetComponent<BossObjectScript>().HealthPoint -= Damage;
            ReloadBullet();

        }
    }

    public void ReloadBullet()
    {
        characterObject.GetComponent<CharacterScript>().Maxazine.Add(Id);
        this.gameObject.SetActive(false);
    }

}
