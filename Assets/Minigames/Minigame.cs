using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameState {
	Waiting,
	InProgress,
	Success,
	Failure,
	Done
}

public abstract class Minigame : MonoBehaviour
{
	public GameDirector director;
	public abstract string Name { get; }
	public MinigameState State = MinigameState.Waiting;
	public bool PlayerAnimationShouldCast;
	public bool HideStaticBobble;

    // Update is called once per frame
    void Update()
    {
        if (State == MinigameState.InProgress) {
			UpdateMinigame();
		}
    }

	protected abstract void UpdateMinigame();
}
