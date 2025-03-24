using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int timer = 150;

    public Customer customer;

    void Start()
    {
        StartCoroutine(Leaves());
    }

    // whole timer object gets deleted when the customer gets their food
    private IEnumerator Leaves()
    {
        yield return new WaitForSeconds(timer);

        customer.LeaveRestaurant();
    }
}
