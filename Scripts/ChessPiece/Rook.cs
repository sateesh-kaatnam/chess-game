using UnityEngine;

public class Rook : ChessPiece
{
    public override bool[,] getPossibleMoves(ChessPiece[,] boardState)
    {
        Debug.Log("rook selected");
        //base.printBoard(boardState);
        bool[,] possibleMoves = new bool[8, 8];

        for(int i= currentY - 1; i >= 0; i--)
        {
            if (boardState[currentX, i] == null)
            {
                possibleMoves[currentX, i] = true;
            }
            if(boardState[currentX, i] != null)
            {
                if (boardState[currentX, i].team != team)
                {
                    possibleMoves[currentX, i] = true;
                }
                break;
            }
        }

        for (int i = currentY + 1; i < tileCount; i++)
        {
            Debug.Log("Down");
            if (boardState[currentX, i] == null)
            {
                possibleMoves[currentX, i] = true;
            }
            if (boardState[currentX, i] != null)
            {
                if (boardState[currentX, i].team != team)
                {
                    possibleMoves[currentX, i] = true;
                }
                break;
            }
        }
   
        for (int i = currentX - 1; i >= 0; i--)
        {
            if (boardState[i, currentY] == null)
            {
                possibleMoves[i, currentY] = true;
            }
            if (boardState[i, currentY] != null)
            {
                if (boardState[i, currentY].team != team)
                {
                    possibleMoves[i, currentY] = true;
                }
                break;
            }
        }

        for (int i = currentX + 1; i <tileCount; i++)
        {
            if (boardState[i, currentY] == null)
            {
                possibleMoves[i, currentY] = true;
            }
            if (boardState[i, currentY] != null)
            {
                if (boardState[i, currentY].team != team)
                {
                    possibleMoves[i, currentY] = true;
                }
                break;
            }
        }

        
        return possibleMoves;
    }
}
