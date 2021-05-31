using UnityEngine;

public class Stack : MonoBehaviour
{
    public StackCube baseCube;
    public StackCube stackingCube;

	[Header("Color")]
	public int stackCount = 0;
	public int changeColorLoop = 10;
	public Material[] colors;
	public int colorID = 0;

	[Header("Event")]
	public EventStackCube onStackCut = new EventStackCube();
	public EventStackCube onNewStack = new EventStackCube();
	public EventStackCube onPerfectCut = new EventStackCube();

	private void Start()
	{
		OnStackCut(baseCube);
	}

	public void Restart()
	{
		stackCount = 0;
		colorID = 0;

		foreach (Transform child in transform)
			if (child != baseCube.transform)
				Destroy(child.gameObject);

		onNewStack.Invoke(baseCube);
		OnStackCut(baseCube);
	}

	public void OnStackCut(StackCube cube)
	{
		stackCount++;

		stackingCube = Instantiate(cube, cube.transform.parent, true);
		stackingCube.transform.position += Vector3.up;
		ChangeColor(stackingCube);
		stackingCube.Init(cube, this);

		onStackCut.Invoke(cube);
		onNewStack.Invoke(stackingCube);

		if (cube.isPerfectCut)
			onPerfectCut.Invoke(cube);
	}

	protected void ChangeColor(StackCube cube)
	{
		if (stackCount % changeColorLoop == 0)
			colorID = (colorID + 1) % colors.Length;

		cube.rend.material.Lerp(
			colors[colorID],
			colors[(colorID+1) % colors.Length],
			(stackCount % changeColorLoop) / (float)changeColorLoop);

	}
}
