using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gameplay : MonoBehaviour
{
	public Transform cameraTarget;

	public void MoveCameraUpToView(StackCube cube)
	{
		cameraTarget.DOMoveY(cube.transform.position.y, 0.5f).SetEase(Ease.OutSine);
	}

	
}
