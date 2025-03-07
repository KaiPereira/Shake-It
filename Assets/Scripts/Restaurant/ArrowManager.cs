using UnityEngine;

public class ArrowManager : MonoBehaviour
{
	public GameObject arrowPrefab;
	private Arrow arrowComponent;

	public void SpawnArrow(Vector3 start, Vector3 end)
	{
		GameObject newArrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
		arrowComponent = newArrow.GetComponent<Arrow>();

		if (arrowComponent != null)
		{
			arrowComponent.SetArrow(start, end);
		}
	}

	public void UpdateArrow(Vector3 start, Vector3 end)
	{
		arrowComponent.SetArrow(start, end);
	}
}
