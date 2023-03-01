using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer
{

	public TeamColor team { get; set; }
	public Board board { get; set; }
	public List<Piece> activePieces { get; private set; } //pieces that are still on the board(same team as the player)
	public List<Piece> takenPieces { get; private set; } // pieces the player has taken from other team

	public ChessPlayer(TeamColor team, Board board)
	{
		activePieces = new List<Piece>();
		this.board = board;
		this.team = team;
	}

	//add the piece to activePiece list if not already exists
	public void AddPiece(Piece piece)
	{
		if (!activePieces.Contains(piece))
			activePieces.Add(piece);
	}

	//remove piece from the activePiece list if exists
	public void RemovePiece(Piece piece)
	{
		if (activePieces.Contains(piece))
			activePieces.Remove(piece);
	}

	//return team of the player
	public TeamColor getTeam()
	{
		return this.team;
	}

	public void AddToTakenPieces(Piece piece)
	{
		takenPieces.Add(piece);
	}
}
