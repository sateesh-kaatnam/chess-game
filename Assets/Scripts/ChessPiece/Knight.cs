using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] getPossibleMoves(ChessPiece[,] boardState)
    {
        Debug.Log("Knight Move");
        bool[,] possibleMoves = new bool[8, 8];

        // Top Right
        int x = currentX + 1;
        int y = currentY + 2;
        if(x<tileCount && y< tileCount)
        {
            if(boardState[x, y]==null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }

         x = currentX + 2;
         y = currentY + 1;
        if (x < tileCount && y < tileCount)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }

        //top Left
        x=currentX -1;
        y= currentY + 2;
        if (x >= 0 && y< tileCount)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }
       
        x = currentX - 2;
        y = currentY + 1;
        if (x >= 0 && y < tileCount)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }
        // Bottom Right
        x = currentX + 1;
        y = currentY - 2;

        if(x<tileCount && y >= 0)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }

        x = currentX + 2;
        y = currentY - 1;

        if (x < tileCount && y >= 0)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }
        
        //
        x = currentX - 1;
        y = currentY - 2;
       if (x >= 0 && y >= 0)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }

        x = currentX - 2;
        y = currentY - 1;

        if (x >= 0 && y >= 0)
        {
            if (boardState[x, y] == null || boardState[x, y].team != team)
            {
                possibleMoves[x, y] = true;
            }
        }
        
        return possibleMoves;
    }
}