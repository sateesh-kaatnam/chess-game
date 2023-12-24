using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HumanPlayer : Player
{

    private const int TILE_COUNTX = 8;
    private const int TILE_COUNTY = 8;
    [SerializeField] private float yOffset = 0.2f;
    private const float TILE_SIZE = 2.0f;

    public HumanPlayer(BoardManager board, bool isWhite = true)
    {
        this.isWhite = isWhite;
        this.isAI = false;
        this.state = 0;
        this.nextMove = new Move(0, 0, 0, 0);
        this.board = board;
    }

    private ChessPiece currentPiece;
    private Vector2Int previousPieceIndex;
    private Vector2Int newPieceIndex;
    

    public bool isWhiteTurn;
    private Vector2Int currentTileInfo = -Vector2Int.one;

    /*public override void Update()
    {
        if (state == 1) // select the peice to move
        {
            int[] square = board.GetRayHitSquare();

            int x = square[0];
            int y = square[1];

            if (this.possibleMoves == null)
            {
                this.possibleMoves = new List<Move>();
                int t = Move.GetPossibleMoves(this.board.matrix, this.isWhite, ref this.possibleMoves);

                if (t == -1)
                {
                    board.CheckMate = true;
                    return;
                }
                else if (t == 0 || t == -2)
                {
                    board.drawn = true;
                    return;
                }
            }


            if (x != -1 && y != -1)
            {
                if (this.isWhite && board.matrix[x, y] > 0 || !this.isWhite && board.matrix[x, y] < 0)
                {
                    this.moveId = 0;

                    foreach (Move move in this.possibleMoves)
                    {
                        if (move.start[0] == x && move.start[1] == y)
                        {
                            board.HighlightSqure(square, 1);

                            if (Input.GetMouseButtonDown(0))
                            {
                                state = 2;
                                this.nextMove.start = square;
                            }
                        }
                        this.moveId++;
                    }
                }
                else
                {
                    board.CleareHighlightSquare(1);
                }
            }
            else
            {
                board.CleareHighlightSquare(1);
            }
        }
        else if (this.state == 2) // select the destination
        {
            this.possibleMoves = null;
            int[] square = board.GetRayHitSquare();

            int x = square[0];
            int y = square[1];

            if (x != -1 && y != -1)
            {
                int starti = this.nextMove.start[0];
                int startj = this.nextMove.start[1];
                int pieceType = this.board.matrix[starti, startj];

                List<Move> possibleMovesPiece = new List<Move>();
                Move.GetPossibleMovesPiece(starti, startj, pieceType, board.matrix, ref possibleMovesPiece);

                foreach (Move move in possibleMovesPiece)
                {
                    if (this.isWhite && x == move.end[0] && y == move.end[1])
                    {
                        board.HighlightSqure(square, 2);

                        if (Input.GetMouseButtonDown(0))
                        {
                            state = 0;
                            this.nextMove = move;
                            board.CleareHighlightSquare(1);
                            board.CleareHighlightSquare(2);
                        }
                        break;

                    }
                    else
                    {
                        board.CleareHighlightSquare(2);
                    }
                }

            }
            else
            {
                board.CleareHighlightSquare(2);
            }
        }
        else
        {
            this.state = 1;
        }

    }
*/

    public override void Update()
    {

        RaycastHit hitInfo;
        isWhite = board.isWhiteTurn;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask(GameConstants.TILE_LAYER, GameConstants.SELECTED_TILE, GameConstants.POSSIBLE_MOVE)))
        {

            Vector2Int hitPosition = GetTileIndex(hitInfo.transform.gameObject);
            if (hitPosition != -Vector2Int.one)
            {
                Debug.Log("isWhite:" + isWhite);

                if (currentTileInfo == -Vector2Int.one)
                {
                    currentTileInfo = hitPosition;
                    board.tiles[currentTileInfo.x, currentTileInfo.y].layer = LayerMask.NameToLayer(GameConstants.SELECTED_TILE);
                }

                if (currentTileInfo != hitPosition)
                {
                    board.tiles[currentTileInfo.x, currentTileInfo.y].layer = LayerMask.NameToLayer(GameConstants.TILE_LAYER);
                    currentTileInfo = hitPosition;
                    board.tiles[currentTileInfo.x, currentTileInfo.y].layer = LayerMask.NameToLayer(GameConstants.SELECTED_TILE);

                }


                if (Input.GetMouseButtonDown(0))
                {

                    if ((board.pieces[hitPosition.x, hitPosition.y] != null && currentPiece == null)
                       || (currentPiece != null && board.pieces[hitPosition.x, hitPosition.y] != null && board.pieces[hitPosition.x, hitPosition.y].team == currentPiece.team))
                    {
                        //Debug.Log("isWhite:"+isWhiteTurn);
                        if ((isWhite && board.pieces[hitPosition.x, hitPosition.y].team == 0) ||
                                (!isWhite && board.pieces[hitPosition.x, hitPosition.y].team == 1))
                        {
                            Debug.Log("Piece Selection");

                            currentPiece = board.pieces[hitPosition.x, hitPosition.y];
                            previousPieceIndex = hitPosition;
                            currentPiece.playPieceSelectSound();
                            board.currentPossibleMoves = currentPiece.getPossibleMoves(board.pieces);
                            HighlightPossiblePath(currentPiece);
                        }


                    }
                    else if (currentPiece != null && board.currentPossibleMoves[hitPosition.x, hitPosition.y])
                    {
                        Debug.Log("Piece moving to:"+ hitPosition.x+":"+hitPosition.y);
                        newPieceIndex = hitPosition;
                        Vector3 moveLocation = GetTileCenter(newPieceIndex.x, newPieceIndex.y);
                        if (board.pieces[hitPosition.x, hitPosition.y] != null)
                        {
                            board.pieces[hitPosition.x, hitPosition.y].gameObject.tag = "Enemy";
                            board.pieces[hitPosition.x, hitPosition.y].GetComponent<Collider>().enabled = true;
                            currentPiece.GetComponent<Collider>().enabled = true;
                            // board.pieces[hitPosition.x, hitPosition.y].gameObject.GetComponent<Collider>().enabled = false;
                            // Destroy(board.pieces[hitPosition.x, hitPosition.y]);
                        }
                        currentPiece.movePiece(moveLocation);

                        RemoveHighlightPossiblePaths();
                        currentPiece.currentX = newPieceIndex.x;
                        currentPiece.currentY = newPieceIndex.y;
                        board.pieces[newPieceIndex.x, newPieceIndex.y] = currentPiece;
                        board.matrix[newPieceIndex.x, newPieceIndex.y] = (int)currentPiece.type;
                        board.pieces[previousPieceIndex.x, previousPieceIndex.y] = null;
                        board.matrix[previousPieceIndex.x, previousPieceIndex.y] = 0;
                        currentPiece = null;
                        board.isWhiteTurn = !board.isWhiteTurn;


                    }
                    else
                    {
                        Debug.Log(hitPosition);
                    }
                }
                //}



            }
            else
            {
                if (currentTileInfo != -Vector2Int.one)
                {
                    board.tiles[currentTileInfo.x, currentTileInfo.y].layer = LayerMask.NameToLayer(GameConstants.TILE_LAYER);
                    currentTileInfo = -Vector2Int.one;
                }
            }
        }


    }

    private Vector3 GetTileCenter(int x, int y)
    {

        return new Vector3(x * TILE_SIZE , yOffset, y * TILE_SIZE ) - board.bounds + new Vector3(TILE_SIZE / 2, 0, TILE_SIZE / 2);
    }

    private void HighlightPossiblePath(ChessPiece piece)
    {
        Debug.Log(board.currentPossibleMoves);

        for (int i = 0; i < TILE_COUNTX; i++)
        {
            for (int j = 0; j < TILE_COUNTY; j++)
            {
                if (board.currentPossibleMoves[i, j] == true)
                {
                    Debug.Log("Got Possible moves");
                    board.tiles[i, j].layer = 8;
                }
            }
        }


    }

    private void revertHighlightedPaths()
    {
        if (board.currentPossibleMoves != null)
        {
            for (int i = 0; i < TILE_COUNTX; i++)
            {
                for (int j = 0; j < TILE_COUNTY; j++)
                {
                    if (board.currentPossibleMoves[i, j])
                        board.tiles[i, j].layer = LayerMask.NameToLayer(GameConstants.POSSIBLE_MOVE);
                }
            }
        }
    }
    private void RemoveHighlightPossiblePaths()
    {
        for (int i = 0; i < TILE_COUNTX; i++)
        {
            for (int j = 0; j < TILE_COUNTY; j++)
            {

                board.tiles[i, j].layer = 6;
                board.currentPossibleMoves[i, j] = false;

            }
        }
    }

    private Vector2Int GetTileIndex(GameObject hitInfo)
    {
        for (int i = 0; i < TILE_COUNTX; i++)
        {
            for (int j = 0; j < TILE_COUNTY; j++)
            {
                if (board.tiles[i, j] == hitInfo)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return -Vector2Int.one;
    }

}
