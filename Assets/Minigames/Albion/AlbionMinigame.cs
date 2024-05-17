using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlbionMinigame : Minigame
{
	public override string Name => "Albion Fishing";
	public UIDocument fishingUI;
	public float movementSpeed = 80;
	public float chargeSpeed = 35;
	public float bobberWidth = 20;
	public float fishWidth = 15;

	private VisualElement fish;
	private VisualElement bobber;

	private float maxPosition = 200;
	private float maxBobberPosition = 200 - 20;
	private float maxFishPosition = 200 - 20;
	
	private float fishPosition = 20;
	
	private float bobberPosition = 0;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		bobber = root.Query("bobber").First();
		fish = root.Query("fish").First();

		bobberPosition = maxBobberPosition / 2f;
    }

	protected override void UpdateMinigame()
	{
		// update bounds
		maxBobberPosition = maxPosition - bobberWidth;
		maxFishPosition = maxPosition - fishWidth;

		// Check if the space bar is pressed
		var bobberVelocity = Input.GetKey(KeyCode.Space) ? 1 : -1;
		bobberPosition += movementSpeed * Time.deltaTime * bobberVelocity;

		// clip bar position
		bobberPosition = Mathf.Clamp(bobberPosition, 0, maxBobberPosition);

		// move fish
		var fishVelocity = Input.GetKey(KeyCode.Space) ? 1 : 0;
		fishPosition += chargeSpeed * Time.deltaTime * fishVelocity;

		// clip fish position
		fishPosition = Mathf.Clamp(fishPosition, 0, maxFishPosition);

		fish.style.left = fishPosition;
		bobber.style.left = bobberPosition;

		// check for final states
		if (fishPosition == maxFishPosition) {
			State = MinigameState.Success;
		} else if (fishPosition == 0) {
			State = MinigameState.Failure;
		}

		if (bobberPosition < 15 || maxBobberPosition - bobberPosition < 15) {
			State = MinigameState.Failure;
		}
	}
}
