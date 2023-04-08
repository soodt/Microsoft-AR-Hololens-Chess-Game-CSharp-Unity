using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SinglePlayer
{
    public ChessGameController controller;
    ChessPlayer computerPlayer;
    public void getComputerMove(string notation, Piece[] activePieces)
    {
        char[] notationChars = notation.ToCharArray();
        Piece[] specificPiece = splitUpActivePieces(notationChars, activePieces);
        
        for (int i = 0; i < specificPiece.Length; i++)
        {
            Piece piece = specificPiece[i];
            if (piece != null)
            {
                piece.PossibleMoves();
                for (int z = 0; z < piece.avaliableMoves.Count; z++)
                {
                    if (piece.avaliableMoves[z] == convertNotationToVector(notationChars))
                    {
                        piece.MovePiece(convertNotationToVector(notationChars));
                    }
                }
            }
        }
    }

    private Piece[] splitUpActivePieces(char[] chars, Piece[] activePieces)
    {
        List<Piece> pieces = new List<Piece>(activePieces);
        List<Piece> specificPiece = new List<Piece>();
        if (chars[0] ==  'K')
        {
            specificPiece.Add(pieces[20]);
        }
        if (chars[0] == 'N')
        {
            specificPiece.Add(pieces[17]);
            specificPiece.Add(pieces[22]);
        }
        if (chars[0] == 'B')
        {
            specificPiece.Add(pieces[18]);
            specificPiece.Add(pieces[21]);
        }
        if (chars[0] == 'Q')
        {
            specificPiece.Add(pieces[19]);
        }
        if (chars[0] == 'R')
        {
            specificPiece.Add(pieces[16]);
            specificPiece.Add(pieces[23]);
        }
        if (Char.IsLower(chars[0]))
        {
            specificPiece.Add(pieces[24]);
            specificPiece.Add(pieces[25]);
            specificPiece.Add(pieces[26]);
            specificPiece.Add(pieces[27]);
            specificPiece.Add(pieces[28]);
            specificPiece.Add(pieces[29]);
            specificPiece.Add(pieces[30]);
            specificPiece.Add(pieces[31]);
        }
        Debug.Log(specificPiece.Count);
        return specificPiece.ToArray();
    }

    public Vector2Int convertNotationToVector(char[] chars)
    {
        int x = -1;
        int y = -1;
        while (chars.Length>2)
        {
            chars = chars.Where((item, index)=>index!=0).ToArray();
        }
        if (chars.Length == 2)
        {
            if (chars[0] == 'a') x = 0;
            if (chars[0] == 'b') x = 1;
            if (chars[0] == 'c') x = 2;
            if (chars[0] == 'd') x = 3;
            if (chars[0] == 'e') x = 4;
            if (chars[0] == 'f') x = 5;
            if (chars[0] == 'g') x = 6;
            if (chars[0] == 'h') x = 7;
            y = chars[1] - 49;
        }

        Vector2Int move = new Vector2Int(x, y);
        return move;
    }
}
