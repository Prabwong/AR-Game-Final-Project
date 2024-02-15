using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEenemyObjectScript : MonoBehaviour
{
    public int HealthPoint;
    // Start is called before the first frame update
    void Start()
    {
        HealthPoint = Random.Range(1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
