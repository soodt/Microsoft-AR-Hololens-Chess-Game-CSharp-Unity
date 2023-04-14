using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour//, IPunObservable
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    private PieceCreator pieceCreator;
    public Piece[] activePieces = new Piece[32];

    private Piece blackKing;
    private Piece whiteKing;
    private Piece checkedKing;
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
    }

    public void StartNetworkGame()
    {
        NetworkCreatePieces(startingBoardLayout);
        
    }

    public ChessPlayer getActivePlayer() {
        return activePlayer;
    }

    public String getActivePlayerString()
    {
        if (activePlayer == whitePlayer)
        {
            return "whitePlayer";
        }
        else return "blackPlayer";
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

    public void initailzeActivePieces(Piece piece)
    {

        for (int i = 0; i < 32; i++)
        {
            if (this.activePieces[i] == null)
                {
                    Debug.Log("Pieces have been set.");
                    this.activePieces[i] = piece;
                    break;
                }
        }
    }

    public void recordPieceRemoval(Piece taken) {
        if (taken.getTeam() == TeamColor.White) {
            blackPlayer.AddToTakenPieces(taken);
            whitePlayer.RemovePiece(taken);
        } else {
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

    public void endTurn() {
        // Swap active player
        if (getActivePlayer().kingInCheck) {
            // player managed to get themselves out of check
            Debug.Log("Succesfully moved out of check");
            getActivePlayer().kingInCheck = false;
            checkedKing = null;
        }
        if (getActivePlayer() == whitePlayer) {
            activePlayer = blackPlayer;
        } else if (getActivePlayer() == blackPlayer) {
            activePlayer = whitePlayer;
        }
        if(checkCond()) {
            Debug.Log("Check");
            activePlayer.kingInCheck = true;
            isGameOver();
        }
        // Debug
        if (getActivePlayer() == whitePlayer) {
            Debug.Log("White");
        } else {
            Debug.Log("Black");
        }

        if (photonView != null && PhotonNetwork.PlayerList.Length >= 2)
        {
            photonView.RPC("UpdateNetworkTurn", RpcTarget.All, getActivePlayerString());
        }
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
        whiteKing = activePieces[4];
        blackKing = activePieces[20];
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
            for (int z = 0; z < piece.avaliableMoves.Count; z++)
            {
                if (piece.avaliableMoves[z] == checkedKing.occupiedSquare)
                {
                    return true;
                }
            }

        }

        return false;
    }

    public bool isGameOver() {
        foreach (Piece p in activePlayer.activePieces)
        {
            p.PossibleMoves();
            p.removeMovesLeavingKingInCheck();
            if (p.avaliableMoves.Count != 0) {
                return false;
            }
        }
        if (activePlayer.kingInCheck) {
            Debug.Log("Checkmate");
        } else {
            Debug.Log("Stalemate");
        }
        return true;
    }
    /**
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(getActivePlayerString());
            //Debug.Log("Sending activePlayer = " + getActivePlayerString());
        }
        else if (stream.IsReading)
        {
            //Debug.Log("Current activePlayer = " + getActivePlayerString());
            NetworkUpdatePlayer((string)stream.ReceiveNext());
            //Debug.Log("Updated activePlayer = " + getActivePlayerString());
        }
    }*/
}
