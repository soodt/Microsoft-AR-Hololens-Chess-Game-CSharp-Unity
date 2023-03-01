using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer : MonoBehaviour
{

    public TeamColor team { get; set; }
	public Board board { get; set; }
	public List<Piece> activePieces { get; private set; }
    public List<Piece> takenPieces { get; private set; } // pieces the player has taken from other team

	public ChessPlayer(TeamColor team, Board board)
	{
		activePieces = new List<Piece>();
		this.board = board;
		this.team = team;
	}
    public void AddPiece(Piece piece)
	{
		if (!activePieces.Contains(piece))
			activePieces.Add(piece);
	}

	public void RemovePiece(Piece piece)
	{
		if (activePieces.Contains(piece))
			activePieces.Remove(piece);
	}

    public TeamColor getTeam() {
        return this.team;
    }

    public void AddToTakenPieces(Piece piece) {
        takenPieces.Add(piece);
    }
}
