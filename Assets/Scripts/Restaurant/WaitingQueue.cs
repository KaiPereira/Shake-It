using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class WaitingQueue : MonoBehaviour
{
	public static WaitingQueue Instance;

	public List<Customer> customerList = new List<Customer>();
	private List<Vector3> positionList;
	private Vector3 entrancePosition;
	private SeatManager seatManager;

	public void Initialize(List<Vector3> positionList) {
		this.positionList = positionList;
		entrancePosition = positionList[positionList.Count - 1];

		seatManager = FindObjectOfType<SeatManager>();
	}

	public bool CanAddCustomer() {
		return customerList.Count < positionList.Count;
	}

	public void AddCustomer(Customer customer) {
		if (!CanAddCustomer()) return;

		customerList.Add(customer);

		int customerIndex = customerList.Count - 1;
		Vector3 targetPosition = positionList[customerIndex];

		customer.MoveTo(targetPosition, () => {
				customer.ShowOrder();
		});
	}

	public void RemoveCustomerAndShift(Customer customer) {
		//if (!customerList.Contains(customer)) return;

		customerList.Remove(customer);

		for (int i = 0; i < customerList.Count; i++) {
			Vector3 newPosition = positionList[i];
			customerList[i].MoveTo(newPosition);
			customerList[i].index = i;
			customerList[i].spriteRenderer.sortingOrder = i;
		}
	}

	public void ClearQueue()
	{

	}
}
