using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Random = UnityEngine.Random;

public enum TicTacToeState{none, cross, circle}

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
	
}

public class TicTacToeAI : MonoBehaviour
{

	int _aiLevel;

	TicTacToeState[,] boardState;

	[SerializeField]
	private bool _isPlayerTurn;

	[SerializeField]
	private int _gridSize = 3;
	
	[SerializeField]
	private TicTacToeState playerState = TicTacToeState.cross;
	TicTacToeState aiState = TicTacToeState.circle;

	[SerializeField]
	private GameObject _xPrefab;

	[SerializeField]
	private GameObject _oPrefab;

	public UnityEvent onGameStarted;

	//Call This event with the player number to denote the winner
	public WinnerEvent onPlayerWin;

	ClickTrigger[,] _triggers;
	
	private void Awake()
	{
		if(onPlayerWin == null){
			onPlayerWin = new WinnerEvent();
		}
	}

	public void StartAI(int AILevel){
		_aiLevel = AILevel;
		Debug.Log(_aiLevel);
		StartGame();
	}

	public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
	{
		_triggers[myCoordX, myCoordY] = clickTrigger;
	}

	private void StartGame()
	{
		_triggers = new ClickTrigger[3,3];
		boardState = new TicTacToeState[3, 3];

		onGameStarted.Invoke();
	}

	public void PlayerSelects(int coordX, int coordY){

		if (_isPlayerTurn == true)
		{
			boardState[coordX, coordY] = playerState;
			_triggers[coordX, coordY].SetInputEndabled(false);
			SetVisual(coordX, coordY, playerState);
			_isPlayerTurn = false;
			checkWinner();
            //need to delay to allow computer to check for winner
            if (_aiLevel == 0)
            {
				available();

			}
            else
            {
				minmax();

            }
			

			

		}
	}

    private void minmax()
    {
		
		return;
	}

	public void available()
    {
		List<Array> availableSlots = new List<Array>();
		
		for (int i=0;i< boardState.GetLength(0); i++)
        {for(int j = 0; j < boardState.GetLength(1); j++)
            {
				if (_triggers[i,j].canClick==true)
				{
					int[] slot =new int[]{ i, j };
					//AiSelects(i, j);
					availableSlots.Add(slot);
					

				}

			}
            
        }

		int index = Random.Range(0, availableSlots.Count);
		List<int> coord = new List<int>();
		Array chosenSlot = availableSlots[index];
		foreach(int i in chosenSlot)
        {
			coord.Add(i);

		}
		AiSelects(coord[0], coord[1]);
	}

    public void AiSelects(int coordX, int coordY){
        
			boardState[coordX, coordY] = aiState;
			_triggers[coordX, coordY].SetInputEndabled(false);
			SetVisual(coordX, coordY, aiState);
			_isPlayerTurn = true;
			checkWinner();

        
      
		
	}

	private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
	{
		Instantiate(
			targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
			_triggers[coordX, coordY].transform.position,
			Quaternion.identity
		);
	}
	private void checkWinner()
	{


		for (int i = 0; i < 3; i++)
		{
			if (boardState[0, i] == playerState && boardState[1, i] == playerState && boardState[2, i] == playerState)
			{
				Debug.Log("Player Wins");
				onPlayerWin.Invoke(0);

			}
		}
		for (int i = 0; i < 3; i++)
		{
			if (boardState[i, 0] == playerState && boardState[i, 1] == playerState && boardState[i, 2] == playerState)
			{
				Debug.Log("Player Wins");
				onPlayerWin.Invoke(0);

			}
		}
		for (int i = 0; i < 3; i++)
		{
			if (boardState[0, i] == aiState && boardState[1, i] == aiState && boardState[2, i] == aiState)
			{
				Debug.Log("AI Wins");
				onPlayerWin.Invoke(1);

			}
		}
		for (int i = 0; i < 3; i++)
		{
			if (boardState[i, 0] == aiState && boardState[i, 1] == aiState && boardState[i, 2] == aiState)
			{
				Debug.Log("AI Wins");
				onPlayerWin.Invoke(1);

			}
		}
		if (boardState[2, 0] == playerState && boardState[1, 1] == playerState && boardState[0, 2] == playerState)
		{
			Debug.Log("Player Wins");
			onPlayerWin.Invoke(0);
		}
		if (boardState[0, 0] == playerState && boardState[1, 1] == playerState && boardState[2, 2] == playerState)
		{
			Debug.Log("Player Wins");
			onPlayerWin.Invoke(0);
		}

		if (boardState[0, 0] == aiState && boardState[1, 1] == aiState && boardState[2, 2] == aiState)
		{
			Debug.Log("AI Wins");
		    onPlayerWin.Invoke(1);
		}
		if (boardState[2, 0] == aiState && boardState[1, 1] == aiState && boardState[0, 2] == aiState)
		{
			Debug.Log("AI Wins");
			onPlayerWin.Invoke(1);
		}
		else
		{
			return;
		}
	}
	
        

	
}
