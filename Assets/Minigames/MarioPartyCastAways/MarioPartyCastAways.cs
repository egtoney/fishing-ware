using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Lane {
	public GameObject lane;
	public float spawnDelay;
	public float nextSpawn;
}

public class MarioPartyCastAways : Minigame
{
	public override string Name => "Mario Party Fishing";

	public float fullCastTime = 2;
	public float fullCastDistance = 10;
	public float catchDistance = .65f;
	public float castSpeed = 4;
	public GameObject fishPrefab;
	public GameObject bobberPrefab;
	public GameObject bobberLane;
	public List<Lane> lanes = new List<Lane>();

	private float castStart = 0;
	private float castTarget = 0;
	private float castDirection = 0;
	private GameObject bobber;
	private GameObject bobberCatch;
	private List<GameObject> fishList = new List<GameObject>();

	// Start is called before the first frame update
	void Start()
    {
        
    }

	protected override void UpdateMinigame()
	{
		//// remove fish
		fishList.RemoveAll(fish => fish == null);
		
		//// update lanes
		for (int i=0 ; i<lanes.Count ; i++) {
			var lane = lanes[i];
			
			lane.nextSpawn -= Time.deltaTime;

			if (lane.nextSpawn <= 0) {
				lane.nextSpawn = lane.spawnDelay;

				var fish = Instantiate(fishPrefab, lane.lane.transform);

				if (i == 1) {
					fish.GetComponent<MarioPartyCastAwaysFish>().up = false;
				}
				if (i == 2) {
					fish.GetComponent<MarioPartyCastAwaysFish>().speed *= 1.25f;
				}

				fishList.Add(fish);
			}

			lanes[i] = lane;
		}

		//// check for cast
		if (Input.GetKey(KeyCode.Space)) {
			if (castStart == 0) {
				castStart = Time.time;
			}
		} else {
			if (castStart != 0) {
				float delta = Time.time - castStart;

				castStart = 0;
				castTarget = Math.Min(delta / fullCastTime, 1) * fullCastDistance;
				castDirection = 1;

				bobber = Instantiate(bobberPrefab, bobberLane.transform);
			}
		}

		//// move bobber
		if (bobber != null) {
			bobber.transform.localPosition = new Vector3(
				bobber.transform.localPosition.x + castDirection * castSpeed * Time.deltaTime,
				bobber.transform.localPosition.y,
				bobber.transform.localPosition.z
			);

			if (bobber.transform.localPosition.x >= castTarget) {
				castDirection = -1;
			}

			if (bobber.transform.localPosition.x <= 0) {
				if (bobberCatch != null) {
					State = MinigameState.Success;
				} else {
					State = MinigameState.Failure;
				}
				
				Destroy(bobber);
				Destroy(bobberCatch);

				bobber = null;
				bobberCatch = null;
			}

			// check for collision
			if (bobber != null && bobberCatch == null && castDirection == -1) {
				for (int i=0 ; i<fishList.Count ; i++) {
					var diff = new Vector2(
						bobber.transform.position.x - fishList[i].transform.position.x,
						bobber.transform.position.y - fishList[i].transform.position.y
					);
					if (diff.magnitude < catchDistance) {
						bobberCatch = fishList[i];
						bobberCatch.GetComponent<MarioPartyCastAwaysFish>().speed = 0;

						bobberCatch.transform.SetParent(bobber.transform);
					}
				}
			}
		}
	}
}
