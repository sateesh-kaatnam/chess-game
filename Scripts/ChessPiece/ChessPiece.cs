using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum ChessPieceType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight =3,
    Bishop = 4,
    Queen = 5,
    King = 6
}
public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public const int tileCount = 8;
    public ChessPieceType type;

    private Vector3 desiredPosition;
    private Vector3 desiredScale;



    public bool isWhite;
    public int value;
    public bool isMoved = false;

    public AudioClip pieceSelectSound;
    public AudioClip pieceMove;
    public AudioClip pieceHit;

    int[] target;
    int[] target2;  // only for knight
    int[] attack;

    public static BoardManager board;

    public int[] square = new int[2] { 0, 0 };
    public virtual void  movePiece(Vector3 tileLocation)
    {
        NavMeshAgent agent = transform.GetComponent<NavMeshAgent>();
        Animator anim = transform.GetComponent<Animator>();
        anim.SetBool("isWalking", true);
        if (team == 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        agent.destination = tileLocation;


    }

    public virtual bool isMoving()
    {
        NavMeshAgent agent = this.GetComponent<NavMeshAgent>();
        if (agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.2)
        {
            if (this.team==0) { 
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            this.GetComponent<Animator>().SetBool("isWalking", false);
            return false;
        }

        bool isAttacking = this.GetComponent<Animator>().GetBool("isAttacking");
        this.GetComponent<Animator>().SetBool("isWalking", true && !isAttacking);
        return true;
    }

   public virtual void playPieceSelectSound()
    {
        AudioSource pieceSelectSource = GetComponent<AudioSource>();
        
        pieceSelectSource.PlayOneShot(pieceSelectSound);
    }
    
    /* public ChessPiece Clone()
    {
        return (ChessPiece)this.MemberwiseClone();
    }

    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }*/

    public virtual bool[,] getPossibleMoves(ChessPiece[,] boradState)
    {
        bool[,] arr = new bool[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                arr[i, j] = false;
            }
        }
        arr[2, 0] = true; arr[2, 1] = true;
        return arr;
    }
    public void printBoard(ChessPiece[,] floorMapArray)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < floorMapArray.GetLength(1); i++)
        {
            for (int j = 0; j < floorMapArray.GetLength(0); j++)
            {
                sb.Append(floorMapArray[i, j]!=null);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    public void Move(Move move)
    {
        switch (move.type)
        {
            case MoveType.NORMAL:
                    target = move.end;
                    board.pieces[square[0], square[1]] = null;


                break;

            case MoveType.CAPTURE:
                target = GetAttakingSquare(move);
                attack = move.end;
                board.pieces[square[0], square[1]] = null;
                break;

            default:
                break;
        }
    }

    public int[] GetAttakingSquare(Move move)
    {
        if (move.type != MoveType.CAPTURE)
            return null;

        int x0 = square[0];
        int y0 = square[1];

        int x1, y1, k1, k2;

        if ((int)type == 3 || (int)type == -3)
        {
            if (move.end[0] - x0 == 1 || move.end[0] - x0 == -1)
                return new int[] { x0, move.end[1] };

            else
                return new int[] { move.end[0], y0 };

        }
        else
        {
            x1 = move.end[0];
            y1 = move.end[1];

            k1 = 0;
            k2 = 0;

            if (x0 < x1) k1 = -1;
            if (x0 > x1) k1 = 1;

            if (y0 < y1) k2 = -1;
            if (y0 > y1) k2 = 1;
        }


        return new int[] { x1 + k1, y1 + k2 };
    }

}
