﻿using UnityEngine;
using System;
using System.Collections.Generic; 		
using Random = UnityEngine.Random; 		
namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{
		[Serializable]
		public class Count
		{
			public int minimum; 			
			public int maximum; 			
			
			
			//Assignment constructor.
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
		
		
		public int columns = 8; 										
		public int rows = 8;											
		public Count wallCount = new Count (5, 9);						
		public Count foodCount = new Count (1, 5);						
		public GameObject exit;											
		public GameObject[] floorTiles;									
		public GameObject[] wallTiles;									
		public GameObject[] foodTiles;									
		public GameObject[] enemyTiles;									 
		public GameObject[] outerWallTiles;								
		
		private Transform boardHolder;									
		private List <Vector3> gridPositions = new List <Vector3> ();	
		
		
		void InitialiseList ()
		{
			
			gridPositions.Clear ();
			
			
			for(int x = 1; x < columns-1; x++)
			{
				
				for(int y = 1; y < rows-1; y++)
				{
					
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}
		
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			boardHolder = new GameObject ("Board").transform;

			for(int x = -1; x < columns + 1; x++)
			{
				for(int y = -1; y < rows + 1; y++)
				{
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					
					//Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
					if(x == -1 || x == columns || y == -1 || y == rows)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					
					//Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
					//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			int randomIndex = Random.Range (0, gridPositions.Count);
			
			Vector3 randomPosition = gridPositions[randomIndex];
			
			gridPositions.RemoveAt (randomIndex);
			
			return randomPosition;
		}
		
		
		//LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			int objectCount = Random.Range (minimum, maximum+1);
			
			for(int i = 0; i < objectCount; i++)
			{
				Vector3 randomPosition = RandomPosition();
				
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}
		
		public void SetupScene (int level)
		{
			BoardSetup ();
			
			InitialiseList ();
			
			LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
			
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);

            int enemyCount = (int)Mathf.Log(level, 2f);
			
			
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

            Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		}
	}
}
