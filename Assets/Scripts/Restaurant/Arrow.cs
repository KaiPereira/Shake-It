using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Arrow : MonoBehaviour
{
	public float stemWidth;
	public float tipLength;
	public float tipWidth;

	[System.NonSerialized]
	public List<Vector3> verticesList;
	[System.NonSerialized]
	public List<int> trianglesList;

	private Mesh mesh;

	void Awake()
	{
		mesh = this.GetComponent<MeshFilter>().mesh;
	}

	void GenerateArrow(float stemLength)
	{
		verticesList = new List<Vector3>();
		trianglesList = new List<int>();

		Vector3 stemOrigin = Vector3.zero;
		float stemHalfWidth = stemWidth / 2f;

		verticesList.Add(stemOrigin + new Vector3(0, -stemHalfWidth, 0));
		verticesList.Add(stemOrigin + new Vector3(0, stemHalfWidth, 0));
		verticesList.Add(stemOrigin + new Vector3(stemLength, -stemHalfWidth, 0));
		verticesList.Add(stemOrigin + new Vector3(stemLength, stemHalfWidth, 0));

		trianglesList.Add(0);
		trianglesList.Add(1);
		trianglesList.Add(2);
		trianglesList.Add(1);
		trianglesList.Add(3);
		trianglesList.Add(2);

		Vector3 tipOrigin = new Vector3(stemLength, 0, 0);
		float tipHalfWidth = tipWidth / 2;

		verticesList.Add(tipOrigin + new Vector3(0, tipHalfWidth, 0));
		verticesList.Add(tipOrigin + new Vector3(0, -tipHalfWidth, 0));
		verticesList.Add(tipOrigin + new Vector3(tipLength, 0, 0));

		trianglesList.Add(4);
		trianglesList.Add(5);
		trianglesList.Add(6);

		mesh.vertices = verticesList.ToArray();
		mesh.triangles = trianglesList.ToArray();
		mesh.RecalculateNormals();
	}

	public void SetArrow(Vector3 start, Vector3 end)
	{
		Vector3 direction = (start - end).normalized;
		float totalLength = Vector3.Distance(start, end);
		float stemLength = totalLength - tipLength;

		transform.position = start;
		//transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle + 180);

		GenerateArrow(stemLength);
	}
}
