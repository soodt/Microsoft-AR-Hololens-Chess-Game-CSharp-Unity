using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]


public abstract class Piece : MonoBehaviour, IMixedRealityPointerHandler
{
	[SerializeField] private MaterialSetter materialSetter;
	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; set; }
	public List<Vector2Int> avaliableMoves;

	public bool taken = false;
	public Vector3 finalCoords; // specify the pos of the piece after being taken
	public ChessGameController controller {get; set;}
	public TurnIndicator turnIndicator { get; set;}
	public abstract List<Vector2Int> SelectAvaliableSquares();
	public abstract bool isAttackingSquare(Vector2Int coords);
	public String typeName {get; set;}
    public abstract bool hasMovedTwoSquares();

    private void Awake()
	{
		avaliableMoves = new List<Vector2Int>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
		turnIndicator = GetComponent<TurnIndicator>();
	}

	public TeamColor getTeam() {
		return this.team;
	}

	public void SetMaterial(Material selectedMaterial)
	{
        if (materialSetter == null) 
            materialSetter = GetComponent<MaterialSetter>();
		materialSetter.SetSingleMaterial(selectedMaterial);
	}

	public ChessPlayer getPlayerFromSameTeam() {
		if (this.team == TeamColor.White) {
			return controller.getWhitePlayer();
		} else {
			return controller.getBlackPlayer();
		}
	}

	public bool IsFromSameTeam(Piece piece)
	{
		return team == piece.team;
	}

    public bool CanMoveTo(Vector2Int coords)
	{
		return avaliableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{

	}
    public virtual void PossibleMoves()
	{

	}

    protected void TryToAddMove(Vector2Int coords)
	{
		if (!taken)
		{
			avaliableMoves.Add(coords);
		}
	}

	public void removeMovesLeavingKingInCheck() {
		List<Vector2Int> updatedMoves = new List<Vector2Int>();
		foreach (Vector2Int move in this.avaliableMoves.ToList()) 
		{
			Vector2Int temp = this.occupiedSquare;
			this.occupiedSquare = move;
			if (!controller.checkCond()) {
				updatedMoves.Add(move);
			}
			this.occupiedSquare = temp;
		}
		this.avaliableMoves.Clear();
		foreach (Vector2Int move in updatedMoves)
		{
			this.avaliableMoves.Add(move);
		}
	}


	public void SetData(Vector2Int coords, TeamColor team, Board board, ChessGameController c, String type)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		this.controller = c;
		this.typeName = type;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
		PossibleMoves();
		removeMovesLeavingKingInCheck();
        board.HightlightTiles(avaliableMoves);
       // Debug.Log("Down"); ;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
		List<Vector2Int> temp = new List<Vector2Int>(avaliableMoves); // creates temporary copy
        avaliableMoves.Clear();
        board.HightlightTiles(avaliableMoves);	// destroys highlights
		avaliableMoves = new List<Vector2Int>(temp); // resets available moves
       // Debug.Log("up");
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
      //  Debug.Log("click");
    }
}