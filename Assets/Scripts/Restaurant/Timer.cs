using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int timer = 75;
    public bool hasFood = false;

    private OrderManager orderManager;
    public Customer customer;

    void Start()
    {
        StartCoroutine(Leaves());
        orderManager = FindObjectOfType<OrderManager>();
    }

    private IEnumerator Leaves()
    {
        yield return new WaitForSeconds(timer);

        if (!hasFood)
        {
            customer.LeaveRestaurant();
        }
    }

    void Update()
    {
        
    }
}
