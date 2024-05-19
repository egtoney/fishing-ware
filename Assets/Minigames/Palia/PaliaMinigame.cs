using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PaliaMinigame : Minigame
{
	public override string Name => "Palia Fishing";
	public UIDocument fishingUI;
	public Transform bobber;
	public Transform fish;
	public float bobberSpeed = 4;
	public float fishSpeed = 4;
	public float fishMaxDelay = .4f;
	public float fishMinDelay = 0f;
	public float catchSpeed = 1;
	public float healthLossSpeed = 125;
	public float catchPositionSuccess = -15;

	private VisualElement healthBar;

	private float bobberPosition = 0;
	private float fishPosition = 0;
	private float nextFishPosition = 0;
	private float nextFishDelay = 0;
	private float catchPosition = 0;
	private float health = 300;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		healthBar = root.Query("health-bar").First();
    }

	protected override void UpdateMinigame()
	{
		//// move fish around
		// var fishHeight = fish.resolvedStyle.height;
		var maxFishPosition = 4;

		// check if at next location
		if (Mathf.Abs(nextFishPosition - fishPosition) < 0.1)
		{
			nextFishDelay -= Time.deltaTime;
			// wait for fish delay
			if (nextFishDelay <= 0) {
				nextFishPosition = Random.Range(-maxFishPosition, maxFishPosition);
				nextFishDelay = Random.Range(fishMinDelay, fishMaxDelay);
			}
		}

		// move to next location
		var fishVelocity = nextFishPosition - fishPosition;
		// normalize (keeping sign)
		if (Mathf.Abs(fishVelocity) > 1)
		{
			fishVelocity /= Mathf.Abs(fishVelocity);
		}

		fishPosition += fishSpeed * Time.deltaTime * fishVelocity;

		// fish is always moving toward us
		catchPosition -= catchSpeed * Time.deltaTime;

		//// move bobber
		float bobberVelocity = 0;
		if (Input.GetKey(KeyCode.W)) {
			bobberVelocity += 1;
		}
		if (Input.GetKey(KeyCode.S)) {
			bobberVelocity -= 1;
		}

		bobberPosition += bobberSpeed * Time.deltaTime * bobberVelocity;

		//// check if bobber is over fish
		if (Mathf.Abs(fishPosition - bobberPosition) <= 1) {

		} else {
			// update health
			health -= healthLossSpeed * Time.deltaTime;
		}

		// clip health
		health = Mathf.Clamp(health, 0, 300);

		// check for end states
		if (health == 0) {
			State = MinigameState.Failure;
		}

		if (catchPosition <= catchPositionSuccess) {
			State = MinigameState.Success;
		}

		healthBar.style.width = health;
		fish.localPosition = new Vector3(catchPosition, fishPosition, fish.position.z);
		bobber.localPosition = new Vector3(catchPosition, bobberPosition, bobber.position.z);
	}
}
