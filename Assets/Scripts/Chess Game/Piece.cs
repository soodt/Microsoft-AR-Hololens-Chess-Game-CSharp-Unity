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

	public ChessGameController controller {get; set;}

	public abstract List<Vector2Int> SelectAvaliableSquares();
	public abstract bool isAttackingSquare(Vector2Int coords);

	private void Awake()
	{
		avaliableMoves = new List<Vector2Int>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
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
		avaliableMoves.Add(coords);
	}


	public void SetData(Vector2Int coords, TeamColor team, Board board, ChessGameController c)
	{
		this.team = team;
		occupiedSquare = coords;
		this.board = board;
		this.controller = c;
		transform.position = board.CalculatePositionFromCoords(coords);
	}

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        PossibleMoves();
        board.HightlightTiles(avaliableMoves);
       // Debug.Log("Down"); ;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
         avaliableMoves.Clear();
        board.HightlightTiles(avaliableMoves);
       // Debug.Log("up");
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
      //  Debug.Log("click");
    }
}