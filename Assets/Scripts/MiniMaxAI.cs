using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;
// using Unity.Collections;

public class MiniMaxAI : Player
{
    private int treeHeight = 3;

    private Move drawMove = null;

    private Thread thread;


    // constructor
    public MiniMaxAI(BoardManager board, bool isWhite = false, int difficultyLevel=0)
    {
        this.isWhite = isWhite;
        this.isAI = true;
        this.board = board;
        this.nextMove = null;
        this.state = 1;
        if (difficultyLevel == 0)
        {
            treeHeight = 1;
        }
        else
        {
            treeHeight = difficultyLevel;
        }

    }


    public override void Update()
    {
        if (state == 1)
        {
            Debug.Log("Ai Move");
            state = 2;
            getAiMove();
            
            // this.nextMove = MiniMax( ref board.matrix, isWhite, 0, -Mathf.Infinity, Mathf.Infinity );
        }
        else
        {

        }

    }

    // tree traversing recursive
    private Move MiniMax(ref int[,] board, bool isMax, int depth, float alpha, float beta)
    {
        Debug.Log("Mini Max");
        List<Move> moves = new List<Move>();

        int t = Move.GetPossibleMoves(board, isMax, ref moves, this.board.pieces);
        Debug.Log("possible Move:" + t);
        if (t == 0)
        {
            if (depth == 0)
                this.board.drawn = true;

            return null;
        }
        else if (t == -1)
        {
            if (depth == 0)
            {
                this.board.CheckMate = true;
            }
            return null;
        }


        float minMax = 0.0f;
        int k = 0;
        float eval = 0.0f;

        if (isMax) eval = -Mathf.Infinity;
        else eval = Mathf.Infinity;

        for (int i = 0; i < moves.Count; i++)
        {
            int temp = BoardManager.ApplyMove(ref board, moves[i]);

            if (depth == treeHeight - 1)
            {
                eval = Evaluator.Evaluate(board);
            }
            else
            {
                Move bestMove = MiniMax(ref board, !isMax, depth + 1, alpha, beta);

                if (bestMove == null)
                {
                    BoardManager.ReverseMove(ref board, moves[i], temp);
                    continue;
                }

                int temp1 = BoardManager.ApplyMove(ref board, bestMove);

                eval = Evaluator.Evaluate(board);

                BoardManager.ReverseMove(ref board, bestMove, temp1);
            }

            if (i == 0)
            {
                minMax = eval;
                BoardManager.ReverseMove(ref board, moves[i], temp);
                continue;
            }

            if (isMax)
            {
                if (minMax < eval)
                {
                    minMax = eval;
                    k = i;
                }

                if (alpha < eval)
                    alpha = eval;

            }
            else
            {
                if (minMax > eval)
                {
                    minMax = eval;
                    k = i;
                }

                if (beta > eval)
                    beta = eval;
            }



            BoardManager.ReverseMove(ref board, moves[i], temp);

            if (beta <= alpha)
                break;
        }


        if (t == -2)
        {
            if (depth == 0)
                this.drawMove = moves[k];
        }
        Debug.Log("Move start:" + moves[k].start[0] + ":" + moves[k].start[1]);
        Debug.Log("Move end :"+ moves[k].end[0]+":"+ moves[k].end[1]);
        return moves[k];
    }

    Move getAiMove()
    {
        this.nextMove = MiniMax(ref board.matrix, isWhite, 0, -Mathf.Infinity, Mathf.Infinity);
        if(this.nextMove == null)
        {
            Debug.Log("No move");
            return null;
        }
        Debug.Log("Start:" + nextMove.start[0] + ":" + nextMove.start[1]);
        Debug.Log("end:" + nextMove.end[0] + ":" + nextMove.end[1]);
        if (this.drawMove != null)
        {
            if (this.drawMove.Equals(this.nextMove))
            {
                Debug.Log("Drawn");
                board.drawn = true;
            }
        }

        ChessPiece piece = board.pieces[nextMove.start[0], nextMove.start[1]];
        Vector3 target = board.GetTileCenter(nextMove.end[0], nextMove.end[1]);
        piece.movePiece(target);
        if(board.pieces[nextMove.end[0], nextMove.end[1]] != null)
        {
            board.pieces[nextMove.end[0],nextMove.end[1]].tag ="Enemy";
            piece.GetComponent<Collider>().enabled = true;
            board.pieces[nextMove.end[0], nextMove.end[1]].GetComponent<Collider>().enabled = true;
        }

        board.pieces[nextMove.end[0], nextMove.end[1]]= piece;

        board.pieces[nextMove.start[0], nextMove.start[1]] = null;

        board.matrix[nextMove.end[0], nextMove.end[1]] = isWhite== true?(int)piece.type: -1* (int)piece.type;
        board.matrix[nextMove.start[0], nextMove.start[1]] = 0;
        board.isWhiteTurn = !board.isWhiteTurn;
        state = 1;
        return this.nextMove;
        // this.thread.Abort();

    }

}
