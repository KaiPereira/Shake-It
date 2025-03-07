using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int timer = 10;
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
        Debug.Log("STARTED MOVING");

        yield return new WaitForSeconds(timer);

        if (!hasFood)
        {
            Debug.Log("CUSTOMER LEAVING");
            customer.LeaveRestaurant();
        }
    }

    void Update()
    {
        
    }
}
