using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int timer = 75;
    public bool hasFood = false;

    void Start()
    {
        StartCoroutine(Leaves());
    }

    private IEnumerator Leaves()
    {
        yield return new WaitForSeconds(timer);

        if (!hasFood)
        {
            Debug.Log("CUSTOMER GONE");
        }
    }

    void Update()
    {
        
    }
}
