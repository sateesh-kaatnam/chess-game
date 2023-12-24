using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool[,] getPossibleMoves(ChessPiece[,] boardState)
    {
        Debug.Log("Bishop Move");
        bool[,] possibleMoves = new bool[8, 8];

        // Top right
        
        for(int x= currentX+1, y= currentY+1; x<tileCount && y< tileCount; x++, y++)
        {
            if(boardState[x, y] == null)
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

        for (int x = currentX + 1, y = currentY - 1; x < tileCount && y >=0; x++, y--)
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

        for (int x = currentX - 1, y = currentY - 1; x >=0 && y >= 0; x--, y--)
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

    /*public override void movePiece(Vector3 tileLocation)
    {

        base.movePiece(tileLocation);
        this.GetComponent<Animator>().SetBool("isWalking", true);

    }

    public override bool isMoving()
    {
        bool isMoving = base.isMoving();
        this.GetComponent<Animator>().SetBool("isWalking", isMoving);
        if (!isMoving && this.isWhite)
        {
            
        } 
        return isMoving;
    }*/
}

