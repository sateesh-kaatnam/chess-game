using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }

    [Header("Board Materials")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Material selectedTileMaterial;
    [SerializeField] private float tileSize = 2.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;

    [Header("Prefabs & Mats")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Texture[] pieceTextures;


    private const float TILE_SIZE = 2.0f;
    private const float TILE_OFFSET = 0.5f;
    private const int TILE_COUNTX = 8;
    private const int TILE_COUNTY = 8;

    public GameObject[,] tiles;
    private Vector2Int currentTileInfo = -Vector2Int.one;
    public Vector3 bounds;


    public ChessPiece[,] pieces { set; get; }

    public ChessPiece[,] tmp_chessmans = new ChessPiece[8, 8];

    private ChessPiece currentPiece;
    private Vector2Int previousPieceIndex;
    private Vector2Int newPieceIndex;
    public bool[,] currentPossibleMoves;


    public ChessPiece WhiteKing;
    public ChessPiece BlackKing;
    public ChessPiece WhiteRook1;
    public ChessPiece WhiteRook2;
    public ChessPiece BlackRook1;
    public ChessPiece BlackRook2;

    //Player Controls
    public bool isWhiteTurn;

    public bool CheckMate = false;
    public bool drawn = false;

    public Player aiPlayer;

    public int[,] matrix;

    //Menu Selection
    public static int playMode;
    public static int difficultyLevel;

    public Player player1;
    public Player player2;

    private void Awake()
    {

        isWhiteTurn = true;
        matrix = new int[8, 8];
        GenerateChessTiles(TILE_SIZE, TILE_COUNTX, TILE_COUNTY);
        //SpawnPiece(ChessPieceType.King, 0);
        SpawnAllPieces();
        //aiPlayer = new MiniMaxAI(this, pieces, false);

    }

    public void Start()
    {
        Debug.Log(difficultyLevel + ": playmode:" + playMode);
        InItGame();
    }
    private void Update()
    {
        if (isAnyPieceMoving())
        {
            return;
        }
        revertHighlightedPaths();

        if (isWhiteTurn)
        {
            Debug.Log("white turn");
           if (checkmateCheck(1))
            {
                WinBoardScript.defaultText = "White Wins !!!";
                SceneManager.LoadScene(2);
                return;
            }

            if (drawn)
            {
                WinBoardScript.defaultText = "Game Drawn !!!";
                SceneManager.LoadScene(2);
                return;
            }
            player1.Update();

        }
        else
        {
            if (checkmateCheck(0))
            {
                WinBoardScript.defaultText = "Black Wins !!!";
                SceneManager.LoadScene(2);
                return;
            }

            if (drawn)
            {
                WinBoardScript.defaultText = "Game Drawn !!!";
                Debug.Log("Game Drawn");
                SceneManager.LoadScene(2);
                return;
            }

            player2.Update();

        }


    }

    //Initialize the game
    public void InItGame()
    {
        switch (playMode)
        {
            case 1:
                Debug.Log("Double Player");
                player1 = new HumanPlayer(this,true);
                player2 = new HumanPlayer(this);

                break;
            case 2:
                player1 = new HumanPlayer(this, true);
                player2 = new MiniMaxAI(this,false, difficultyLevel);
                Debug.Log("Ai vs Player: difficulty"+ difficultyLevel);
                break;
            case 3:
                player1 = new MiniMaxAI(this, true,0);
                player2 = new MiniMaxAI(this,false,0);
                Debug.Log("AI vs AI");
                break;
            default:
                Debug.Log("Double Player");
                player1 = new HumanPlayer(this, true);
                player2 = new HumanPlayer(this);
                break;

        }
    }
    private void GenerateChessTiles(float tileSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + boardCenter;
        tiles = new GameObject[tileCountX, tileCountY];
        for (int i = 0; i < tileCountX; i++)
        {
            for (int j = 0; j < tileCountY; j++)
            {
                tiles[i, j] = GenerateSingleTile(tileSize, i, j);
                // Debug.Log("Current Tile:" + tiles[i, j].name);
            }
        }

    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tile = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tile.transform.parent = transform;

        Mesh mesh = new Mesh();
        tile.AddComponent<MeshFilter>().mesh = mesh;
        tile.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y + 1) * tileSize) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        tile.layer = LayerMask.NameToLayer(GameConstants.TILE_LAYER);
        tile.AddComponent<BoxCollider>();

        return tile;
    }


    //Spawn the pieces
    private void SpawnAllPieces()
    {
        pieces = new ChessPiece[TILE_COUNTX, TILE_COUNTY];

        spawnTeam(0);
        spawnTeam(1);
        PositionAllPieces();

        WhiteKing = pieces[3, 7];
        BlackKing = pieces[3, 0];

        WhiteRook1 = pieces[0, 7];
        WhiteRook2 = pieces[7, 7];
        BlackRook1 = pieces[0, 0];
        BlackRook2 = pieces[7, 0];

    }

    private void spawnTeam(int teamCode)
    {
        int yPosition = teamCode == 0 ? 7 : 0;
        int pawnPosition = teamCode == 0 ? 6 : 1;
        int k = teamCode == 0 ? 1 : -1;
        pieces[0, yPosition] = SpawnPiece(ChessPieceType.Rook, teamCode);
        matrix[0, yPosition] = (int)ChessPieceType.Rook*k;

        pieces[1, yPosition] = SpawnPiece(ChessPieceType.Knight, teamCode);
        matrix[1, yPosition] = (int)ChessPieceType.Knight*k;

        pieces[2, yPosition] = SpawnPiece(ChessPieceType.Bishop, teamCode);
        matrix[2, yPosition] = (int)ChessPieceType.Bishop * k;

        pieces[4, yPosition] = SpawnPiece(ChessPieceType.Queen, teamCode);
        matrix[4, yPosition] = (int)ChessPieceType.Queen * k;

        pieces[3, yPosition] = SpawnPiece(ChessPieceType.King, teamCode);
        matrix[3, yPosition] = (int)ChessPieceType.King * k;

        pieces[5, yPosition] = SpawnPiece(ChessPieceType.Bishop, teamCode);
        matrix[5, yPosition] = (int)ChessPieceType.Bishop * k;

        pieces[6, yPosition] = SpawnPiece(ChessPieceType.Knight, teamCode);
        matrix[6, yPosition] = (int)ChessPieceType.Knight * k;

        pieces[7, yPosition] = SpawnPiece(ChessPieceType.Rook, teamCode);
        matrix[7, yPosition] = (int)ChessPieceType.Rook * k;

        for (int i = 0; i < TILE_COUNTX; i++)
        {
            pieces[i, pawnPosition] = SpawnPiece(ChessPieceType.Pawn, teamCode);
            matrix[i, pawnPosition] = (int)ChessPieceType.Pawn * k;
        }

    }

    private ChessPiece SpawnPiece(ChessPieceType type, int team)
    {
        ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();
        cp.type = type;
        cp.team = team;
        if (team == 0)
        {
            cp.transform.rotation = Quaternion.Euler(0, 180, 0);

        }

        Renderer[] renderers = cp.transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material.SetTexture("_BaseMap", pieceTextures[team]);
            if (team == 1)
                r.material.SetColor("_Color", Color.black);
        }
        return cp;
    }

    private void PositionAllPieces()
    {
        for (int i = 0; i < TILE_COUNTX; i++)
        {
            for (int j = 0; j < TILE_COUNTX; j++)
            {
                if (pieces[i, j] != null)
                {
                    PositionSinglePiece(i, j, true);
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        pieces[x, y].currentX = x;
        pieces[x, y].currentY = y;

        pieces[x, y].transform.position = GetTileCenter(x, y);


    }

    public Vector3 GetTileCenter(int x, int y)
    {

        return new Vector3(x * TILE_SIZE + 0.5f, yOffset, y * TILE_SIZE + 0.5f) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }



    private void revertHighlightedPaths()
    {
        if (currentPossibleMoves != null)
        {
            for (int i = 0; i < TILE_COUNTX; i++)
            {
                for (int j = 0; j < TILE_COUNTY; j++)
                {
                    if (currentPossibleMoves[i, j])
                        tiles[i, j].layer = LayerMask.NameToLayer(GameConstants.POSSIBLE_MOVE);
                }
            }
        }
    }
    
    public bool isAnyPieceMoving()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < TILE_COUNTY; j++)
            {
                if (pieces[i, j] != null && pieces[i, j].isMoving())
                {
                    return true;
                }
            }
        }
        return false;
    }

    // apply the given move to the given board
    // return = the value of the piece (or 0 if empty) that was in the 
    // end move squar 
    public static int ApplyMove(ref int[,] board, Move move)
    {
        int temp = board[move.end[0], move.end[1]];
        board[move.end[0], move.end[1]] = board[move.start[0], move.start[1]];
        board[move.start[0], move.start[1]] = 0;

        return temp;
    }


    // apply move to this board
    public void ApplyMove(Move move)
    {
        int x = move.start[0];
        int y = move.start[1];

        ChessPiece piece = pieces[x, y];

        if (piece == null)
        {
            return;
        }
        piece.Move(move);
        ApplyMove(ref matrix, move);
    }

    public static void ReverseMove(ref int[,] board, Move move, int old_piece)
    {
        board[move.start[0], move.start[1]] = board[move.end[0], move.end[1]];
        board[move.end[0], move.end[1]] = old_piece;
    }


    public bool checkmateCheck(int team)
    {
        ChessPiece king = null;
        int kingX=0, kingY = 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j=0;j< 8; j++)
            {
                if (pieces[i,j]!=null && pieces[i,j].team==team && (int)pieces[i,j].type == 6)
                {
                    kingX = i;
                    kingY = j;
                    king = pieces[i, j];
                    break;
                }
            }
        }
        if(king == null)
        {
            return true;
        }
        else
        {
            int oppTeam = team == 0 ? 1 : 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i, j] != null)
                    {
                        bool[,] possibleMoves = pieces[i, j].getPossibleMoves(pieces);
                        if (possibleMoves[kingX, kingY] == true)
                        {
                            return true;
                        }
                    }
                }
            }

        }

        return false;
    }
}
