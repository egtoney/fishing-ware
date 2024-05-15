using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StardewMinigame : Minigame
{
	public GameDirector director;
	public UIDocument fishingUI;

	private float barHeight = 200 - 6;
	private float bobberHeight = 30;
	private float bobberAcceleration = .1f;
	private float fishHeight = 10;

	private float bobberPosition = 0;
	private float bobberVelocity = 0;
	private float fishPosition = 0;
	private float fishDelay = 0;
	private float nextFishPosition = 0;

	private float percentCaught = 0;

	private VisualElement bar;
	private VisualElement barSpacer;
	private VisualElement bobber;
	private VisualElement fish;
	private VisualElement progressValue;

	public override string Name => "Star Fishing";

	// Start is called before the first frame update
	void Start()
	{
		var root = fishingUI.rootVisualElement;

		bar = root.Query("bar").First();
		barSpacer = root.Query("bar-filler-bottom").First();
		bobber = root.Query("bobber").First();
		fish = root.Query("fish").First();
		progressValue = root.Query("progress-value").First();
	}

	// Update is called once per frame
	protected override void FixedUpdateGame()
	{
		// var barHeight = bar.resolvedStyle.height;
		// var bobberHeight = bobber.resolvedStyle.height;
		var maxBobberPosition = barHeight - bobberHeight;

		// Check if the space bar is pressed
		if (Input.GetKey(KeyCode.Space))
		{
			bobberVelocity += bobberAcceleration;
		}
		else
		{
			bobberVelocity -= bobberAcceleration;
		}

		bobberPosition += bobberVelocity;

		// clip bar position
		if (bobberPosition < 0)
		{
			bobberPosition = 0;
		}
		if (bobberPosition > maxBobberPosition)
		{
			bobberPosition = maxBobberPosition;
		}

		// clip bar velocity
		if (bobberPosition == 0 || bobberPosition == maxBobberPosition)
		{
			bobberVelocity = 0;
		}

		//// move fish around
		// var fishHeight = fish.resolvedStyle.height;
		var maxFishPosition = barHeight - fishHeight;

		// check if at next location
		if (Mathf.Abs(nextFishPosition - fishPosition) < 0.1)
		{
			fishDelay -= Time.deltaTime;
			// wait for fish delay
			if (fishDelay <= 0) {
				nextFishPosition = Random.Range(0, maxFishPosition);
				fishDelay = Random.Range(.5f, 2);
			}
		}

		// move to next location
		var fishDelta = nextFishPosition - fishPosition;
		// normalize (keeping sign)
		if (Mathf.Abs(fishDelta) > 1)
		{
			fishDelta /= Mathf.Abs(fishDelta);
		}

		fishPosition += fishDelta;

		// check if bobber is covering fish
		if (
			// bottom of fish is below top of bar
			fishPosition <= bobberPosition + bobberHeight &&
			// top of fish is above the bottom of the bar
			fishPosition + fishHeight >= bobberPosition
		)
		{
			percentCaught += 1;
		}
		else
		{
			percentCaught -= 1;
		}

		if (percentCaught > barHeight)
		{
			percentCaught = barHeight;
		}
		else if (percentCaught < 0)
		{
			percentCaught = 0;
		}

		barSpacer.style.height = bobberPosition;
		fish.style.bottom = fishPosition;
		progressValue.style.height = percentCaught;

		// check for final states
		if (percentCaught == barHeight) {
			State = MinigameState.Success;
		} else if (percentCaught == 0) {
			State = MinigameState.Failure;
		}
	}
}