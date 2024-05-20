using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlbionMinigame : Minigame
{
	public override string Name => "Albiono Carp";
	public UIDocument fishingUI;
	public float movementSpeed = 80;
	public float chargeSpeed = 35;
	public float bobberWidth = 20;
	public float fishWidth = 18;
	public float failureZoneSize = 15;

	private VisualElement fish;
	private VisualElement bobber;

	private float maxPosition = 200 - 8;
	
	private float fishPosition = 1;
	
	private float bobberPosition = 0;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		bobber = root.Query("bobber").First();
		fish = root.Query("fish").First();

		float maxBobberPosition = maxPosition - bobberWidth;
		bobberPosition = maxBobberPosition / 2f;
    }

	protected override void UpdateMinigame()
	{
		// update bounds
		float maxBobberPosition = maxPosition - bobberWidth;
		float maxFishPosition = maxPosition - fishWidth;

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

		if (bobberPosition < failureZoneSize || maxBobberPosition - bobberPosition < failureZoneSize) {
			State = MinigameState.Failure;
		}
	}
}
