using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoonstoneMinigame : Minigame
{
	public override string Name => "Moon Fishing";
	public UIDocument fishingUI;
	public float movementSpeed = 80;
	public float chargeSpeed = 35;

	private VisualElement fish;
	private VisualElement bobber;

	private float maxDistance = 0;
	
	private Vector2 fishPosition = new Vector2(0, 0);
	private float fishDelay = 1;
	private Vector2 nextFishPosition = new Vector2(0, 0);
	
	private Vector2 bobberPosition = new Vector2(0, 0);

	private float percentCaught = 0;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		bobber = root.Query("bobber").First();
		fish = root.Query("fish").First();
    }

	protected override void UpdateMinigame()
	{
		maxDistance = fish.parent.resolvedStyle.width / 2f;
		// do nothing

		// Check if the space bar is pressed
		var bobberDelta = new Vector2(0, 0);
		if (Input.GetKey(KeyCode.W))
		{
			bobberDelta.y -= 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			bobberDelta.y += 1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			bobberDelta.x -= 1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			bobberDelta.x += 1;
		}

		bobberPosition += movementSpeed * Time.deltaTime * bobberDelta.normalized;

		// clip bobber to UI
		if (bobberPosition.magnitude > maxDistance) {
			bobberPosition = bobberPosition.normalized * maxDistance;
		}

		// check if at next location
		if (Vector2.Distance(fishPosition, nextFishPosition) < 0.1)
		{
			fishDelay -= Time.deltaTime;
			// wait for fish delay
			if (fishDelay <= 0) {
				var angle = Random.Range(-Mathf.PI, Mathf.PI);
				var distance = Random.Range(0, maxDistance);
				nextFishPosition = new Vector2(
					distance * Mathf.Cos(angle),
					distance * Mathf.Sin(angle)
				);
				fishDelay = Random.Range(1f, 2.5f);
			}
		}

		// move to next location
		var fishDelta = nextFishPosition - fishPosition;

		fishPosition += movementSpeed * Time.deltaTime * fishDelta.normalized;

		// check if bobber is covering fish
		float percentDelta;
		if (
			Vector2.Distance(fishPosition, bobberPosition) <= 10
		)
		{
			percentDelta = 1;
		}
		else
		{
			percentDelta = -1;
		}
		percentCaught += chargeSpeed * Time.deltaTime * percentDelta;

		if (percentCaught > 100)
		{
			percentCaught = 100;
		}
		else if (percentCaught < 0)
		{
			percentCaught = 0;
		}

		fish.style.left = fishPosition.x + maxDistance;
		fish.style.top = fishPosition.y + maxDistance;

		bobber.style.left = bobberPosition.x + maxDistance;
		bobber.style.top = bobberPosition.y + maxDistance;

		// check for final states
		if (percentCaught == 100) {
			State = MinigameState.Success;
		} else if (percentCaught == 0) {
			State = MinigameState.Failure;
		}
	}
}
