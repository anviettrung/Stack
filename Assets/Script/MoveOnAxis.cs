using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnAxis : MonoBehaviour
{
	public enum Axis {
		X, Y, Z
	}

	public Axis moveAxis;
	public float moveSpeed;
	public float moveRange;

	public bool moving = false;
	protected float currentDirectionSign = 1;

	public void Update()
	{
		if (moving) {
			DoMove();
		}
	}

	public float Pos {
		get {
			switch (moveAxis) {
				case Axis.X:
					return transform.position.x;
				case Axis.Y:
					return transform.position.y;
				case Axis.Z:
					return transform.position.z;
			}
			return 0;
		}
		set {
			Vector3 temp = transform.position;
			switch (moveAxis) {
				case Axis.X:
					temp.x = value;
					break;
				case Axis.Y:
					temp.y = value;
					break;
				case Axis.Z:
					temp.z = value;
					break;
			}
			transform.position = temp;
		}
	}

	public void ReverseMoveAxis()
	{
		moveAxis = moveAxis == Axis.X ? Axis.Z : Axis.X;
	}

	protected void DoMove()
	{
		float attempMove = Pos + currentDirectionSign * moveSpeed * Time.deltaTime;
		float extend = Mathf.Abs(attempMove) - moveRange;

		if (extend > 0) {
			attempMove = Mathf.Sign(attempMove) * (moveRange - extend);
			currentDirectionSign *= -1;
		}

		Pos = Mathf.Clamp(attempMove, -moveRange, moveRange);
	}
}
