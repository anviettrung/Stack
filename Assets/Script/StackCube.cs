using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class EventStackCube : UnityEvent<StackCube> { }
public class StackCube : MonoBehaviour
{
	//[Header("General")]
	[HideInInspector] public bool isPerfectCut = false;


	[Header("Ref")]
	public MoveOnAxis moveOnAxis;
	public Rigidbody rb;
	public Collider coll;
	public Renderer rend;
	public GameObject perfectLineEffect;

	public float Size {
		get {
			switch (moveOnAxis.moveAxis) {
				case MoveOnAxis.Axis.X:
					return transform.localScale.x;
				case MoveOnAxis.Axis.Y:
					return transform.localScale.y;
				case MoveOnAxis.Axis.Z:
					return transform.localScale.z;
			}
			return 0;
		}
		set {
			Vector3 temp = transform.localScale;
			switch (moveOnAxis.moveAxis) {
				case MoveOnAxis.Axis.X:
					temp.x = value;
					break;
				case MoveOnAxis.Axis.Y:
					temp.y = value;
					break;
				case MoveOnAxis.Axis.Z:
					temp.z = value;
					break;
			}
			transform.localScale = temp;
		}
	}

	[SerializeField] protected StackCube previ;
	[HideInInspector] public Stack stackParent;

	public const float snapEpsilon = 0.25f;

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && moveOnAxis.moving)
			Cut(previ);
	}

	public void Init(StackCube prev, Stack parent)
	{
		moveOnAxis.ReverseMoveAxis();
		moveOnAxis.Pos = -moveOnAxis.moveRange;
		moveOnAxis.moving = true;

		previ = prev;
		previ.moveOnAxis.moveAxis = this.moveOnAxis.moveAxis;
		stackParent = parent;
	}

	public void Cut(StackCube prev)
	{
		float cutLength = moveOnAxis.Pos - prev.moveOnAxis.Pos;
		float sign = Mathf.Sign(cutLength);
		cutLength = Mathf.Abs(cutLength);
		moveOnAxis.moving = false;

		// Snap
		if (cutLength < snapEpsilon) {
			moveOnAxis.Pos = prev.moveOnAxis.Pos;
			isPerfectCut = true;
			//PerfectEffect();
			stackParent.OnStackCut(this);
		}
		// Normal cut
		else if (cutLength < this.Size) {
			StackCube clone = Instantiate(this, this.transform.parent, true);

			clone.moveOnAxis.Pos = prev.moveOnAxis.Pos + sign * (this.Size + cutLength) / 2;
			this.moveOnAxis.Pos = this.moveOnAxis.Pos - sign * cutLength / 2;

			clone.Size = cutLength;
			this.Size = this.Size - cutLength;

			clone.rb.useGravity = true;
			clone.coll.isTrigger = false;

			stackParent.OnStackCut(this);
		}
		// Lose
		else {
			rb.useGravity = true;
		}
	}

	public void PerfectEffect()
	{
		perfectLineEffect.SetActive(true);

		perfectLineEffect.transform.DOScale(1.1f, 1f)
			.SetEase(Ease.OutSine)
			.OnComplete(() => perfectLineEffect.SetActive(false));
	}
}
