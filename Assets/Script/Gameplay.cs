using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Gameplay : MonoBehaviour
{
	public TextMeshProUGUI countText;
	public Transform cameraTarget;
	public Stack stack;

	public void MoveCameraUpToView(StackCube cube)
	{
		cameraTarget.DOMoveY(cube.transform.position.y, 0.5f).SetEase(Ease.OutSine);
	}

	public void UpdateCountText()
	{
		countText.text = (stack.stackCount - 1).ToString();
	}
}
