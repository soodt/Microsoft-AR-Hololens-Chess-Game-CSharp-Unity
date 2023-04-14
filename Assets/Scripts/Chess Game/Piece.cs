using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(MaterialSetter))]
[RequireComponent(typeof(IObjectTweener))]


public abstract class Piece : MonoBehaviour, IMixedRealityPointerHandler, IPunObservable
{
	[SerializeField] private MaterialSetter materialSetter;
	public Board board { protected get; set; }
	public Vector2Int occupiedSquare { get; set; }
	public TeamColor team { get; set; }
	public bool hasMoved { get; set; }
	public List<Vector2Int> availableMoves;

	public bool taken = false;
	public Vector3 finalCoords; // specify the pos of the piece after being taken
	public ChessGameController controller {get; set;}
	public TurnIndicator turnIndicator { get; set;}
	public abstract List<Vector2Int> SelectAvailableSquares();
	public abstract bool isAttackingSquare(Vector2Int coords);
	public String typeName {get; set;}
    public abstract bool hasMovedTwoSquares();

    private void Awake()

	private void Awake()
	{
		public PhotonView photonView;

		availableMoves = new List<Vector2Int>();
		materialSetter = GetComponent<MaterialSetter>();
		hasMoved = false;
		turnIndicator = GetComponent<TurnIndicator>();

		photonView = gameObject.GetComponent<PhotonView>();
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
		return availableMoves.Contains(coords);
	}

	public virtual void MovePiece(Vector2Int coords)
	{

	}
    public virtual void PossibleMoves()
	{

	}

	public virtual String AlgebraicNotation(Vector2Int coords, Vector2Int prevCoords, bool capture, bool pawnPromote, bool enPassant, bool castle)
	{
		return "";
	}

    protected void TryToAddMove(Vector2Int coords)
	{

		if (!taken)
		{
			availableMoves.Add(coords);
		}
	}

	public void removeMovesLeavingKingInCheck() {
		List<Vector2Int> updatedMoves = new List<Vector2Int>();
		foreach (Vector2Int move in this.availableMoves.ToList()) 
		{
			
			if (board.getPiece(move) && !IsFromSameTeam(board.getPiece(move))) {
				Piece tempPiece = board.getPiece(move);
				ChessPlayer pl = tempPiece.getPlayerFromSameTeam();
				pl.activePieces.Remove(tempPiece);
				Vector2Int temp = this.occupiedSquare;
				this.occupiedSquare = move;
				if (!controller.checkCond()) {
					updatedMoves.Add(move);
				}
				pl.activePieces.Add(tempPiece);
				this.occupiedSquare = temp;
			} else {
				Vector2Int temp = this.occupiedSquare;
				this.occupiedSquare = move;
				if (!controller.checkCond()) {
					updatedMoves.Add(move);
				}
				this.occupiedSquare = temp;
			}
		}
		this.availableMoves.Clear();
		foreach (Vector2Int move in updatedMoves)
		{
			this.availableMoves.Add(move);
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
		//Debug.Log(controller);
	}

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
		PossibleMoves();
		removeMovesLeavingKingInCheck();
        board.HightlightTiles(availableMoves);
       // Debug.Log("Down"); ;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {

    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
		List<Vector2Int> temp = new List<Vector2Int>(availableMoves); // creates temporary copy
        availableMoves.Clear();
        board.HightlightTiles(availableMoves);	// destroys highlights
		availableMoves = new List<Vector2Int>(temp); // resets available moves
       // Debug.Log("up");
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
      //  Debug.Log("click");
    }

	public void AssignPlayerBlack()
    {
		if (PhotonNetwork.PlayerList.Length == 2)
        {
			photonView.TransferOwnership(PhotonNetwork.PlayerList[1]);
        }
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(gameObject.transform.position);
			stream.SendNext(gameObject.transform.rotation);
			stream.SendNext(occupiedSquare.x);
			stream.SendNext(occupiedSquare.y);
			//Debug.Log(occupiedSquare.x);
			//Debug.Log(this + " moved to " + occupiedSquare.x + ", " + occupiedSquare.y);
		}
		else if (stream.IsReading)
		{
			gameObject.transform.position = (Vector3)stream.ReceiveNext();
			gameObject.transform.rotation = (Quaternion)stream.ReceiveNext();
			occupiedSquare = new Vector2Int((int)stream.ReceiveNext(), (int)stream.ReceiveNext());

			//occupiedSquare = new Vector2Int((int)x, (int)y);
		}
	}
}