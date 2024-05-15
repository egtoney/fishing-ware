using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class GameDirector : MonoBehaviour
{
	public UIDocument transitionUI;

	public List<Minigame> minigames;

	private Minigame activeMinigame;
	private float startMinigameCountdown = 0;
	private float nextMinigameCountdown = 0;

	private Label countdownUI;
	private VisualElement countdownWrapperUI;
	private Label minigameNameUI;

    // Start is called before the first frame update
    void Start()
    {
		var root = transitionUI.rootVisualElement;

		countdownWrapperUI = root.Query("countdown-wrapper").First();
		countdownUI = root.Query<Label>("countdown").First();
		minigameNameUI = root.Query<Label>("minigame-name").First();
    }

    // Update is called once per frame
    void Update()
    {
		Assert.IsTrue(minigames.Count > 0, "minigames array contains at least one item");

		startMinigameCountdown -= Time.deltaTime;
		nextMinigameCountdown -= Time.deltaTime;

		if (
			// no minigame
			activeMinigame == null ||
			// need a new minigame
			(
				activeMinigame.State == MinigameState.Done &&
				nextMinigameCountdown < 0
			)
		) {
			var minigamePrefab = minigames[0];

			// remove from game scene if already in scene
			if (activeMinigame != null) {
				Destroy(activeMinigame.gameObject);
			}

			activeMinigame = Instantiate(minigamePrefab, transform.position, transform.rotation, transform);
			minigameNameUI.text = activeMinigame.Name;
			startMinigameCountdown = 3;
		}

		if (activeMinigame.State == MinigameState.Waiting) {
			if (startMinigameCountdown <= 0) {
				countdownUI.text = "";
				countdownWrapperUI.style.display = DisplayStyle.None;

				// start minigame
				activeMinigame.State = MinigameState.InProgress;

			} else {
				countdownWrapperUI.style.display = DisplayStyle.Flex;
				var num = Mathf.RoundToInt(startMinigameCountdown).ToString();
				if (num == "0") {
					countdownUI.text = "go!";
				} else {
					countdownUI.text = num;
				}
			}
		}

		// check if game is over
		if (
			activeMinigame.State == MinigameState.Success ||
			activeMinigame.State == MinigameState.Failure
		) {
			countdownWrapperUI.style.display = DisplayStyle.Flex;
			nextMinigameCountdown = 1.5f;

			if (activeMinigame.State == MinigameState.Success) {
				countdownUI.text = "Success!";
			} else if (activeMinigame.State == MinigameState.Failure) {
				countdownUI.text = "Failure!";
			}
			activeMinigame.State = MinigameState.Done;
		}

		// var state = activeMinigame.GetState();
		// if (state != MinigameState.InProgress) {
		// 	Debug.Log(state);
		// }
    }
}
