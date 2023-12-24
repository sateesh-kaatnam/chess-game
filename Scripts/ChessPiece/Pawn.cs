using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Pawn : ChessPiece
{
    public Pawn()
    {
        value = 10;
    }


    public override bool[,] getPossibleMoves(ChessPiece[,] boradState)
    {
        Debug.Log("PawnMove");
        bool[,] possibleMoves = new bool[8, 8];

        int direction = (team == 0) ? -1 : 1;
        Debug.Log(boradState[currentX, currentY + direction] == null);
        // one in front
        if (boradState[currentX, currentY + direction] == null)
        {
            Debug.Log("one front");
            possibleMoves[currentX, currentY + direction] = true;
        }

        
        if (boradState[currentX, currentY + direction] == null)
        {
           if(team==0 && currentY == 6 && boradState[currentX,currentY+(direction*2)]==null)
            {
                possibleMoves[currentX, currentY + (direction*2)] = true;
            }

            if (team == 1 && currentY == 1 && boradState[currentX, currentY + (direction * 2)] == null)
            {
                possibleMoves[currentX, currentY + (direction * 2)] = true;
            }
        }

        if (currentX != tileCount-1)
        {
            if (boradState[currentX+1,currentY+direction]!=null && boradState[currentX + 1, currentY + direction].team != team)
            {
                possibleMoves[currentX + 1, currentY + direction] = true;
            }
        }
        if (currentX != 0)
        {
            if (boradState[currentX - 1, currentY + direction] != null && boradState[currentX - 1, currentY + direction].team != team)
            {
                possibleMoves[currentX - 1, currentY + direction] = true;
            }
        }
        return possibleMoves;

    }

    public override void movePiece(Vector3 tileLocation)
    {

        base.movePiece(tileLocation);
        this.GetComponent<Animator>().SetBool("isWalking", true);

    }

    public override bool isMoving()
    {
        bool isMoving=  base.isMoving();
        bool isAttacking = this.GetComponent<Animator>().GetBool("isAttacking");
        this.GetComponent<Animator>().SetBool("isWalking", isMoving && !isAttacking);
        return isMoving;
    }



    /* public override bool[,] getPossibleMoves(ChessPiece[,] pieces)
     {
         bool[,] moves = new bool[8, 8];
         int x = currentX;
         int y = currentY;

         ChessPiece leftChessman = null;
         ChessPiece rightChessman = null;
         ChessPiece forwardChessman = null;

         int[] EnPassant= new int[8];// = BoardManager.Instance.EnPassant;
         bool isWhite = team == 1;
         if (isWhite)
         {
             if (y > 0)
             {
                 // left
                 if (x > 0) leftChessman = BoardManager.Instance.pieces[x - 1, y - 1];
                 // right
                 if (x < 7) rightChessman = BoardManager.Instance.pieces[x + 1, y - 1];
                 // forward
                 forwardChessman = BoardManager.Instance.pieces[x, y - 1];
             }
             // move forward
             if (forwardChessman == null)
             {
                 if (!this.KingInDanger(x, y - 1))
                     moves[x, y - 1] = true;
             }
             // move diagonal left
             if (leftChessman != null && !leftChessman.isWhite)
             {
                 if (!this.KingInDanger(x - 1, y - 1))
                     moves[x - 1, y - 1] = true;
             }
             else if (leftChessman == null && EnPassant[1] == y - 1 && EnPassant[0] == x - 1)
             {
                 if (!this.KingInDanger(x - 1, y - 1))
                     moves[x - 1, y - 1] = true;
             }
             // move diagonal right
             if (rightChessman != null && !rightChessman.isWhite)
             {
                 if (!this.KingInDanger(x + 1, y - 1))
                     moves[x + 1, y - 1] = true;
             }
             else if (rightChessman == null && EnPassant[1] == y - 1 && EnPassant[0] == x + 1)
             {
                 if (!this.KingInDanger(x + 1, y - 1))
                     moves[x + 1, y - 1] = true;
             }
             // move 2 step forward on first move
             if (y == 6 && forwardChessman == null && BoardManager.Instance.pieces[x, y - 2] == null)
             {
                 if (!this.KingInDanger(x, y - 2))
                     moves[x, y - 2] = true;
             }
         }
         else
         {
             if (y < 7)
             {
                 // left
                 if (x > 0) leftChessman = BoardManager.Instance.pieces[x - 1, y + 1];
                 // right
                 if (x < 7) rightChessman = BoardManager.Instance.pieces[x + 1, y + 1];
                 // forward
                 forwardChessman = BoardManager.Instance.pieces[x, y + 1];
             }
             // move forward
             if (forwardChessman == null)
             {
                 if (!this.KingInDanger(x, y + 1))
                     moves[x, y + 1] = true;
             }
             // move diagonal left
             if (leftChessman != null && leftChessman.isWhite)
             {
                 if (!this.KingInDanger(x - 1, y + 1))
                     moves[x - 1, y + 1] = true;
             }
             else if (leftChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x - 1)
             {
                 if (!this.KingInDanger(x - 1, y + 1))
                     moves[x - 1, y + 1] = true;
             }
             // move diagonal right
             if (rightChessman != null && rightChessman.isWhite)
             {
                 if (!this.KingInDanger(x + 1, y + 1))
                     moves[x + 1, y + 1] = true;
             }
             else if (rightChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x + 1)
             {
                 if (!this.KingInDanger(x + 1, y + 1))
                     moves[x + 1, y + 1] = true;
             }
             // move 2 step forward on first move
             if (y == 1 && forwardChessman == null && BoardManager.Instance.pieces[x, y + 2] == null)
             {
                 if (!this.KingInDanger(x, y + 2))
                     moves[x, y + 2] = true;
             }
         }

         return moves;
     }*/

}
