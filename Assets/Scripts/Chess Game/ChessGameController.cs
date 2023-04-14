using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine.InputSystem.XR;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    [SerializeField] private Material red;
    //[SerializeField] private Material black;
    //[SerializeField] private Material white;
    private PieceCreator pieceCreator;
    public Piece[] activePieces = new Piece[32];
    public TurnIndicator turnIndicator;
    public SinglePlayer ai;


    private Piece blackKing;
    private Piece whiteKing;
    private Piece checkedKing;
    public Piece currentKing;
    public bool isSinglePlayer = false; //triggers on and off single player mode
    public ChessPlayer whitePlayer{get; set;}
    public ChessPlayer blackPlayer{get; set;}
    private ChessPlayer activePlayer{get; set;}

    private PhotonView photonView;


    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
        photonView = gameObject.GetComponent<PhotonView>();

    }

    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartNewGame();
    }

     private void StartNewGame()
    {
        CreatePiecesFromLayout(startingBoardLayout);
        board.SetDependencies(this);
        activePlayer = whitePlayer;
        turnIndicator.SetDependencies(this);
        ai = new SinglePlayer();
    }

    public void StartNetworkGame()
    {
        NetworkCreatePieces(startingBoardLayout);

    }

    public String getActivePlayerString()
    {
        if (activePlayer == whitePlayer)
        {
            return "whitePlayer";
        }
        else return "blackPlayer";
    }


    public ChessPlayer getActivePlayer() {
        return activePlayer;
    }

    public ChessPlayer getBlackPlayer() {
        return blackPlayer;
    }

    public ChessPlayer getWhitePlayer() {
        return whitePlayer;
    }

    private void CreatePlayers()
    {
        whitePlayer = new ChessPlayer(TeamColor.White, board);
        blackPlayer = new ChessPlayer(TeamColor.Black, board);
    }

    private void NetworkCreatePieces(BoardLayout layout)
    {
        Debug.Log(layout.GetPiecesCount().ToString());
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            string typeName = layout.GetSquarePieceNameAtIndex(i);
            //Debug.Log(i.ToString() + " = " + typeName);
            Type type = Type.GetType(typeName);
            Piece newPiece = pieceCreator.CreateNetworkPiece(type, i).GetComponent<Piece>();
            initailzeActivePieces(newPiece);
        }

    }

    public void NetworkInitialisePieces(int layoutIndex, GameObject chessPiece)
    {
        board.SetDependencies(this);
        activePlayer = whitePlayer;

        Piece piece = chessPiece.GetComponent<Piece>();
        TeamColor team = startingBoardLayout.GetSquareTeamColorAtIndex(layoutIndex);
        Vector2Int squareCoords = startingBoardLayout.GetSquareCoordsAtIndex(layoutIndex);
        string typeName = startingBoardLayout.GetSquarePieceNameAtIndex(layoutIndex);

        InsertComponents(piece);

        piece.SetData(squareCoords, team, board, this, typeName);

        //Debug.Log(piece.controller);

        assignTeam(piece, team);

        activePieces[layoutIndex] = piece;

    }

    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);

            Type type = Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, team, type);
        }
    }

    private void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();

        InsertComponents(newPiece);

        newPiece.SetData(squareCoords, team, board, this, type.ToString());

        initailzeActivePieces(newPiece);

        assignTeam(newPiece, team);
    }

    private void InsertComponents(Piece newPiece)
    {
        //make each piece interactable with AR
        newPiece.gameObject.AddComponent<BoxCollider>();
        newPiece.gameObject.AddComponent<NearInteractionGrabbable>();
        newPiece.gameObject.AddComponent<ObjectManipulator>();
        //make each piece have the board anchor as parent.
        newPiece.gameObject.AddComponent<BoardAnchorAsParent>();

        // add snapping to each piece
        newPiece.GetComponent<ObjectManipulator>().OnManipulationEnded.AddListener(delegate
        {
            float distance = board.squareSize * 4;
            Vector2Int newCoords = new Vector2Int(-1, -1);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int nextSquare = new Vector2Int(i, j);
                    float newDistance = Vector3.Distance(newPiece.transform.position, board.CalculatePositionFromCoords(nextSquare));
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        newCoords.Set(i, j);
                    }
                }
            }
            if (distance < board.squareSize * 1.5)
            {
                newPiece.MovePiece(newCoords);
            }
            else
            {
                newPiece.MovePiece(newPiece.occupiedSquare);
            }
        }
        );
    }

    public void initailzeActivePieces(Piece piece)
    {

        for (int i = 0; i < 32; i++)
        {
            if (this.activePieces[i] == null)
            {
                //Debug.Log("Pieces have been set.");
                this.activePieces[i] = piece;
                break;
            }
        }
    }

    private void assignTeam(Piece newPiece, TeamColor team)
    {
        if (newPiece.getTeam() == TeamColor.White)
        {
            whitePlayer.AddPiece(newPiece);
        }
        else
        {
            blackPlayer.AddPiece(newPiece);
        }

        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);
        if (team == TeamColor.Black)
        {
            newPiece.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
    }

    public void recordPieceRemoval(Piece taken)
    {
        if (taken.getTeam() == TeamColor.White)
        {
            blackPlayer.AddToTakenPieces(taken);
            whitePlayer.RemovePiece(taken);
        }
        else
        {
            whitePlayer.AddToTakenPieces(taken);
            blackPlayer.RemovePiece(taken);
        }

        taken.transform.position += new Vector3(0.0f, -5f, 0.0f);

        if (taken.photonView != null && PhotonNetwork.PlayerList.Length >= 2)
        {
            if (taken.photonView.Controller == PhotonNetwork.PlayerList[0])
            {
                taken.photonView.TransferOwnership(PhotonNetwork.PlayerList[1]);
            }
            else taken.photonView.TransferOwnership(PhotonNetwork.PlayerList[0]);

            taken.transform.position += new Vector3(0.0f, -5f, 0.0f);
        }

    }

    public void endTurn()
    {
        // Swap active player
        if (getActivePlayer().kingInCheck)
        {
            // player managed to get themselves out of check
            Debug.Log("Succesfully moved out of check");
            getActivePlayer().kingInCheck = false;
            Material teamMaterial = pieceCreator.GetTeamMaterial(activePlayer.team);
            checkedKing.SetMaterial(teamMaterial);
            checkedKing = null;
        }

        if (getActivePlayer() == whitePlayer)
        {
            activePlayer = blackPlayer;
            foreach (Piece p in blackPlayer.activePieces)
            {
                if (p.typeName == "Pawn")
                {
                    Pawn pawnref = (Pawn)p;
                    pawnref.movedTwoSquares = false;
                }
            }
            turnIndicator.ColourTeam();
            if (isSinglePlayer) // if true allows single player moves to take place. AI is always blackPlayer
            {
                ai.getComputerMove("h6", activePieces);
            }
        }
        else if (getActivePlayer() == blackPlayer)
        {
            activePlayer = whitePlayer;
            foreach (Piece p in whitePlayer.activePieces)
            {
                if (p.typeName == "Pawn")
                {
                    Pawn pawnref = (Pawn)p;
                    pawnref.movedTwoSquares = false;
                }
            }
            turnIndicator.ColourTeam();
        }
        if (photonView != null && PhotonNetwork.PlayerList.Length >= 2)
        {
            photonView.RPC("UpdateNetworkTurn", RpcTarget.All, getActivePlayerString());
        }
        if (checkCond())
        {
            Debug.Log("Check");
            activePlayer.kingInCheck = true;
            checkedKing.SetMaterial(red);
            isGameOver();
        }
        if (checkStaleMate())
        {
            isGameOver();
        }

        // Debug
        if (getActivePlayer() == whitePlayer)
        {
            //Debug.Log("White");
        }
        else
        {
            //Debug.Log("Black");
        }
        /*
        if(activePlayer.kingInCheck == true)
        {
            checkedKing.SetMaterial(red);
        }
        else
        {
            Material teamMaterial = pieceCreator.GetTeamMaterial(activePlayer.team);
            checkedKing.SetMaterial(teamMaterial);
        }
        */


    }

    [PunRPC]
    private void UpdateNetworkTurn(string activePlayerString)
    {
        NetworkUpdatePlayer(activePlayerString);
        Debug.Log("It is the turn of " + activePlayerString);
    }

    public void ChangeTeam() // to make cleaner
    {
        if (getActivePlayer() == whitePlayer)
        {
            activePlayer = blackPlayer;
        }
        else if (getActivePlayer() == blackPlayer)
        {
            activePlayer = whitePlayer;
        }
        
    }

    private void NetworkUpdatePlayer(string activePlayerString)
    {
        if (activePlayerString.Equals("whitePlayer"))
        {
            activePlayer = whitePlayer;
        }
        else activePlayer = blackPlayer;
    }

    public bool checkCond()                     // Evaluates check condition return true if checked else false
    {
        ChessPlayer otherPlayer;
        whiteKing = null;
        blackKing = null;
        foreach (Piece p in whitePlayer.activePieces) {
            if (p.typeName == "King") {
                whiteKing = p;
            }
        }
        foreach (Piece p in blackPlayer.activePieces) {
            if (p.typeName == "King") {
                blackKing = p;
            }
        }
        if (activePlayer == whitePlayer)
        {
            checkedKing = whiteKing;
            otherPlayer = blackPlayer;
        }
        else
        {
            checkedKing = blackKing;
            otherPlayer = whitePlayer;
        }

        for (int i = 0; i < otherPlayer.activePieces.Count; i++)
        {
            Piece piece = otherPlayer.activePieces[i];
            piece.PossibleMoves();
            for (int z = 0; z < piece.availableMoves.Count; z++)
            {
                if (piece.availableMoves[z] == checkedKing.occupiedSquare)
                {
                    return true;
                }
            }

        }

        return false;
    }

    public bool checkStaleMate()                                                // evaluates stalemate condition return true if so else false
    {
        ChessPlayer otherPlayer;
        whiteKing = activePieces[4];
        blackKing = activePieces[20];
        if (activePlayer == whitePlayer)
        {
            currentKing = whiteKing;
            otherPlayer = blackPlayer;
        }
        else
        {
            currentKing = blackKing;
            otherPlayer = whitePlayer;
        }
        for(int x =0; x< activePlayer.activePieces.Count; x++)                  // if pieces other than king have available moves, return 
        {                                                                       // false, stalemate condition not satisfied
            if (activePlayer.activePieces[x] != currentKing)
            {
                activePlayer.activePieces[x].PossibleMoves();
                if (activePlayer.activePieces[x].availableMoves.Count != 0)
                {
                    return false;
                }
            }
        }
        for(int i =0; i < otherPlayer.activePieces.Count; i++)                  // if any move of King is available (without running into check condition)
        {
            
            Piece piece = otherPlayer.activePieces[i];
            piece.PossibleMoves();
            currentKing.PossibleMoves();
            for (int j = 0; j < currentKing.availableMoves.Count; j++)     
            {
                bool notOccupied = true;
                for (int k = 0; k < piece.availableMoves.Count; k++)
                {
                    if (currentKing.availableMoves[j] == piece.availableMoves[k])
                    {
                        notOccupied = false;
                    }
                }
                if (notOccupied) return false;
            }
            
        }
        return true;
    }

    public bool isGameOver()
    {
        foreach (Piece p in activePlayer.activePieces)
        {
            p.PossibleMoves();
            p.removeMovesLeavingKingInCheck();
            if (p.avaliableMoves.Count != 0)
            {
                return false;
            }
        }
        if (activePlayer.kingInCheck)
        {
            Debug.Log("Checkmate");
        }
        else
        {
            Debug.Log("Stalemate");
        }
        return true;
    }

    public bool checkmate()
    {
        foreach (Piece p in activePlayer.activePieces)
        {
            p.PossibleMoves();
            p.removeMovesLeavingKingInCheck();
            if (p.availableMoves.Count != 0)
            {
                return false;
            }
        }
        if (activePlayer.kingInCheck)
        {
            return true;
        }
        else return false;
    }
}
