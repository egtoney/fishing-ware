using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HadesMinigame : Minigame
{
	public override string Name => "Hell Dive Tripe";
	public SpriteRenderer bobber;

	public float fishingDuration = 1;
	public Animator bobbleAnimator;

	private float fishingTimer = 0;

	// Start is called before the first frame update
	void Start()
	{
		fishingTimer = Random.Range(1, 5);
	}

	// Update is called once per frame
	protected override void UpdateMinigame()
	{
		Assert.IsNotNull(bobber, "bobber must be set");

		var wasInActiveState = fishingTimer <= 0 && fishingTimer >= -fishingDuration;

		fishingTimer -= Time.deltaTime;

		var inActiveState = fishingTimer <= 0 && fishingTimer >= -fishingDuration;

		if (inActiveState == true && wasInActiveState != inActiveState) {
			director.PlayerSetToggle("is_alert");
		}

		bobbleAnimator.SetBool("active", inActiveState);

		// check if space is pressed in window
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (inActiveState) {
				State = MinigameState.Success;
			} else {
				State = MinigameState.Failure;
			}
		}

		// check for out of time state
		if (
			fishingTimer < -fishingDuration &&
			State == MinigameState.InProgress
		)
		{
			State = MinigameState.Failure;
		}
	}
}
