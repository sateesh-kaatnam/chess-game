using UnityEngine;

public class King : ChessPiece
{
    public override bool[,] getPossibleMoves(ChessPiece[,] boardState)
    {
        Debug.Log("King Move");
        bool[,] possibleMoves = new bool[8, 8];

        // Right Moves
        if (currentX + 1 < tileCount)
        {
            //Right
            if (boardState[currentX+1, currentY]== null)
            {
                possibleMoves[currentX+1, currentY] = true;
            }
            else if (boardState[currentX+1, currentY].team!=team)
            {
                possibleMoves[currentX + 1, currentY] = true;
            }
            //Top Right
            if (currentY + 1 < tileCount)
            {
                if (boardState[currentX + 1, currentY+1] == null)
                {
                    possibleMoves[currentX + 1, currentY+1] = true;
                }
                else if (boardState[currentX + 1, currentY+1].team != team)
                {
                    possibleMoves[currentX + 1, currentY+1] = true;
                }
            }

            if (currentY - 1 >=0)
            {
                if (boardState[currentX + 1, currentY - 1] == null)
                {
                    possibleMoves[currentX + 1, currentY - 1] = true;
                }
                else if (boardState[currentX + 1, currentY - 1].team != team)
                {
                    possibleMoves[currentX + 1, currentY - 1] = true;
                }
            }

        }
        //Left Moves
        if (currentX - 1 < tileCount)
        {
            //Left
            if (boardState[currentX - 1, currentY] == null)
            {
                possibleMoves[currentX - 1, currentY] = true;
            }
            else if (boardState[currentX - 1, currentY].team != team)
            {
                possibleMoves[currentX - 1, currentY] = true;
            }
            //Top Left
            if (currentY + 1 < tileCount)
            {
                if (boardState[currentX - 1, currentY + 1] == null)
                {
                    possibleMoves[currentX - 1, currentY + 1] = true;
                }
                else if (boardState[currentX - 1, currentY + 1].team != team)
                {
                    possibleMoves[currentX - 1, currentY + 1] = true;
                }
            }
            // Bottom Left
            if (currentY - 1 >= 0)
            {
                if (boardState[currentX - 1, currentY - 1] == null)
                {
                    possibleMoves[currentX - 1, currentY - 1] = true;
                }
                else if (boardState[currentX - 1, currentY - 1].team != team)
                {
                    possibleMoves[currentX - 1, currentY - 1] = true;
                }
            }

        }

        //Up
        if (currentY + 1 < tileCount)
        {
            if (boardState[currentX, currentY + 1] == null || boardState[currentX, currentY + 1].team != team)
            {
                possibleMoves[currentX, currentY + 1]= true;
            }
        }
        //Down
        if (currentY - 1 >=0)
        {
            if (boardState[currentX, currentY - 1] == null || boardState[currentX, currentY - 1].team != team)
            {
                possibleMoves[currentX, currentY - 1] = true;
            }
        }

        return possibleMoves;
    }
}