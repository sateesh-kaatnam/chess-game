using UnityEngine;

public class Queen : ChessPiece
{
    public override bool[,] getPossibleMoves(ChessPiece[,] boardState)
    {
        Debug.Log("Queen Move");
        bool[,] possibleMoves = new bool[8, 8];

        // Rook Moves
        for (int i = currentY - 1; i >= 0; i--)
        {
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

        for (int i = currentX + 1; i < tileCount; i++)
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

        //Bishop Moves
        // Top right

        for (int x = currentX + 1, y = currentY + 1; x < tileCount && y < tileCount; x++, y++)
        {
            if (boardState[x, y] == null)
            {
                possibleMoves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].team != team)
                {
                    possibleMoves[x, y] = true;
                }
                break;
            }
        }

        // Top Left
        for (int x = currentX - 1, y = currentY + 1; x >= 0 && y < tileCount; x--, y++)
        {
            if (boardState[x, y] == null)
            {
                possibleMoves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].team != team)
                {
                    possibleMoves[x, y] = true;
                }
                break;
            }
        }

        // Bottom Right

        for (int x = currentX + 1, y = currentY - 1; x < tileCount && y >= 0; x++, y--)
        {
            if (boardState[x, y] == null)
            {
                possibleMoves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].team != team)
                {
                    possibleMoves[x, y] = true;
                }
                break;
            }
        }

        for (int x = currentX - 1, y = currentY - 1; x >= 0 && y >= 0; x--, y--)
        {
            if (boardState[x, y] == null)
            {
                possibleMoves[x, y] = true;
            }
            else
            {
                if (boardState[x, y].team != team)
                {
                    possibleMoves[x, y] = true;
                }
                break;
            }
        }

        return possibleMoves;
    }
}
