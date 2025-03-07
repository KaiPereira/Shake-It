using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public GameObject player;

	public Vector3 GetPlayerPosition()
	{
		return player.transform.position;
	}
}
