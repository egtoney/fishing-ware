using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FF15Minigame : Minigame
{
	public override string Name => "FF15 Fishing";
	public UIDocument fishingUI;
	public Transform fish;
	public float catchSpeed = 1;
	public float healthLossSpeed = 125;
	public float catchPositionSuccess = -15;
	private VisualElement healthBar;
	public GameObject button;
	public GameObject letterW;
	public GameObject letterS;
	public GameObject letterA;
	public GameObject letterD;

	private char[] inputOptions = new char[]{'w', 's', 'a', 'd'};

	private float catchPosition = 0;
	private float nextInputTimer = 1;
	private char nextInput = ' ';
	private bool waitingForInput = false;
	private float health = 250;

	void Start()
    {
		var root = fishingUI.rootVisualElement;

		healthBar = root.Query("health-bar").First();
    }

	protected override void UpdateMinigame()
	{
		//// check if time to prompt user for input
		nextInputTimer -= Time.deltaTime;

		if (waitingForInput == false && nextInputTimer <= 0) {
			int nextInputIndex = Random.Range(0, inputOptions.Length);
			nextInput = inputOptions[nextInputIndex];
			waitingForInput = true;

			button.SetActive(true);
			switch (nextInput) {
				case 'w':
					letterW.SetActive(true);
					break;
				case 's':
					letterS.SetActive(true);
					break;
				case 'a':
					letterA.SetActive(true);
					break;
				case 'd':
					letterD.SetActive(true);
					break;
			}
		}

		if (waitingForInput == true) {
			bool doneWaiting = false;
			switch (nextInput) {
				case 'w':
					doneWaiting = Input.GetKey(KeyCode.W) == true;
					break;
				case 's':
					doneWaiting = Input.GetKey(KeyCode.S) == true;
					break;
				case 'a':
					doneWaiting = Input.GetKey(KeyCode.A) == true;
					break;
				case 'd':
					doneWaiting = Input.GetKey(KeyCode.D) == true;
					break;
			}

			if (doneWaiting) {
				nextInputTimer = Random.Range(.5f, 1.5f);
				waitingForInput = false;
				button.SetActive(false);
				letterW.SetActive(false);
				letterS.SetActive(false);
				letterA.SetActive(false);
				letterD.SetActive(false);
			}
		}

		//// update health
		if (waitingForInput == true) {
			health -= healthLossSpeed * Time.deltaTime;
		}

		// clip health
		health = Mathf.Clamp(health, 0, 250);

		//// fish is always moving toward us
		if (waitingForInput == false) {
			catchPosition -= catchSpeed * Time.deltaTime;
		}

		healthBar.style.width = health;
		fish.localPosition = new Vector3(catchPosition, fish.localPosition.y, fish.localPosition.z);

		//// check for end states
		if (health == 0) {
			State = MinigameState.Failure;
		}

		if (catchPosition <= catchPositionSuccess) {
			State = MinigameState.Success;
		}
	}
}
