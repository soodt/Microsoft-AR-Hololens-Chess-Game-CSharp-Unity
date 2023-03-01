using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]


public abstract class Piece : MonoBehaviour
{
	[SerializeField] private MaterialSetter materialSetter;
	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; set; }
	public List<Vector2Int> avaliableMoves;

	public ChessGameController controller { get; set; }

	public abstract List<Vector2Int> SelectAvaliableSquares();
	public abstract bool isAttackingSquare(Vector2Int coords);

	private void Awake()
	{
		//initialising avaliable moves list and setting component reference for material setter object
		avaliableMoves = new List<Vector2Int>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
	}

	//return team of the piece
	public TeamColor getTeam()
	{
		return this.team;
	}

	//setting the piece's material
	public void SetMaterial(Material selectedMaterial)
	{
		if (materialSetter == null)
			materialSetter = GetComponent<MaterialSetter>();
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	//comparing teamcolor value with the team color of the piece passed in the parameter
	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

	//check if the value passed in exists in the avaiable moves list
	public bool CanMoveTo(Vector2Int coords)
	{
		return avaliableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{

	}


	// adding available moves
	protected void TryToAddMove(Vector2Int coords)
	{
		avaliableMoves.Add(coords);
	}


	// assign passed in data to pieces
	public void SetData(Vector2Int coords, TeamColor team, Board board, ChessGameController c)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		this.controller = c;
		//position calculated from board
		transform.position = board.CalculatePositionFromCoords(coords);
	}
}