﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
	public GameObject[] RoadBlockPrefabs;
	public GameObject StartBlock;

	float startBlockXPos = 0;
	float currentBlockXPos = 0;
	int blocksCount = 7;
	float blockLength = 0;
	int   safeZone = 50;

	public Transform PlayerTransf;
	List<GameObject> CurrentBlocks = new List<GameObject>();

	Vector3 startPlayerPos;
	void Start()
	{
		currentBlockXPos = StartBlock.transform.position.x;
		blockLength = StartBlock.GetComponent<BoxCollider>().bounds.size.x;
		startPlayerPos = PlayerTransf.position;
		StartGame();
	}

	public void StartGame()
	{
		currentBlockXPos = startBlockXPos;
		PlayerTransf.position = startPlayerPos;

		foreach (var go in CurrentBlocks)
			Destroy(go);
		
		CurrentBlocks.Clear();
		
		for (int i = 0; i < blocksCount; i++)
			SpawnBlock();
	}
	void Update()
	{
		CheckForSpawn();
	}

	void CheckForSpawn()
	{
		if (PlayerTransf.position.x - safeZone> (currentBlockXPos - blocksCount * blockLength))
		{
			SpawnBlock();
			DestroyBlock();
		}
	}

	void SpawnBlock()
	{
		GameObject block = Instantiate(RoadBlockPrefabs[Random.Range(0, RoadBlockPrefabs.Length)], transform);

		currentBlockXPos += blockLength;
		
		block.transform.position = new Vector3(currentBlockXPos, 0, 0);
		
		CurrentBlocks.Add(block);
	}

	void DestroyBlock()
	{
		Destroy(CurrentBlocks[0]);
		CurrentBlocks.RemoveAt(0);
	}
}
