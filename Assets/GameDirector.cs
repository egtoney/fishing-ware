using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameDirector : MonoBehaviour
{
	public UIDocument transitionUI;

	public List<Minigame> minigames;

	private Minigame activeMinigame;
	private float minigameCountdown = 0;

	private Label countdownUI;
	private Label minigameNameUI;

    // Start is called before the first frame update
    void Start()
    {
		var root = transitionUI.rootVisualElement;

		countdownUI = root.Query<Label>("countdown").First();
		minigameNameUI = root.Query<Label>("minigame-name").First();
    }

    // Update is called once per frame
    void Update()
    {
		minigameCountdown -= Time.deltaTime;

		if (minigameCountdown <= 0) {
			countdownUI.text = "";
		} else {
			var num = Mathf.RoundToInt(minigameCountdown).ToString();
			if (num == "0") {
				countdownUI.text = "go!";
			} else {
				countdownUI.text = num;
			}
		}

        if (activeMinigame == null) {
			var minigamePrefab = minigames[0];

			activeMinigame = Instantiate(minigamePrefab, transform.position, transform.rotation, transform);
			minigameNameUI.text = activeMinigame.Name;
			minigameCountdown = 3;
		}

		// var state = activeMinigame.GetState();
		// if (state != MinigameState.InProgress) {
		// 	Debug.Log(state);
		// }
    }
}
