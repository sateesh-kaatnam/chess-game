/*using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;
using System.ComponentModel;
using Unity.VisualScripting;
// using Unity.Collections;

public class MiniMaxAI: Player
{
    private const int treeHeight = 4;

    private Move drawMove = null;

    BoardManager boardManager;
    
    private Thread thread;
    


    // constructor
    public MiniMaxAI(BoardManager manager,  ChessPiece[,] board, bool isWhite = false)
    {
        this.isWhite = isWhite;
        this.isAI = true;
        this.chessBoard = board;
        this.nextMove = null;
        this.state = 1;
        this.boardManager = manager;

    }


    public override void Update()
    {
        if (state == 1)
        {
            state = 2;
            this.thread = new Thread(ThreadFunc);
            thread.Start();
            // this.nextMove = MiniMax( ref board.matrix, isWhite, 0, -Mathf.Infinity, Mathf.Infinity );
        }
        else
        {

        }

    }

    // tree traversing recursive
    private Move MiniMax(int[,] board, bool isMax, int depth, float alpha, float beta)
    {
        List<Move> moves = new List<Move>();
        int[,] previousBoard;
        int t = Move.GetPossibleMoves(board, isMax, ref moves);
        Debug.Log("AI possible Move:" + t);
        if (t == 0)
        {
            if (depth == 0)
                this.boardManager.drawn = true;

            return null;
        }
        else if (t == -1)
        {
            if (depth == 0)
            {
                this.boardManager.CheckMate = true;
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
            int temp = ApplyMove( board, moves[i]);
            ChessPiece tempPiece = ApplyMoveOnOriginalBoard(moves[i]);
            if (depth == treeHeight - 1)
            {
                eval = Evaluator.Evaluate(board);
            }
            else
            {
                Move bestMove = MiniMax( board, !isMax, depth + 1, alpha, beta);

                if (bestMove == null)
                {
                    previousBoard = board;
                    reverseoriginalBoard(previousBoard, moves[i], tempPiece);
                    ReverseMove( board, moves[i], temp);
                    
                    continue;
                }

                int temp1 = ApplyMove( board, bestMove);

                eval = Evaluator.Evaluate(board);
                previousBoard = board;
                reverseoriginalBoard(previousBoard, moves[i], tempPiece);

                ReverseMove( board, bestMove, temp1);
            }

            if (i == 0)
            {
                minMax = eval;
                previousBoard = board;
                reverseoriginalBoard(previousBoard, moves[i], tempPiece);

                ReverseMove( board, moves[i], temp);
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


            previousBoard = board;
            reverseoriginalBoard(previousBoard, moves[i], tempPiece);

            ReverseMove(board, moves[i], temp);

            if (beta <= alpha)
                break;
        }


        if (t == -2)
        {
            if (depth == 0)
                this.drawMove = moves[k];
        }

        return moves[k];
    }

    *//*void ThreadFunc()
    {
        Debug.Log("start " + this.state);
        int[,] boardValueMatrix = generateBoardValueMatrix(board);
        this.nextMove = MiniMax(boardValueMatrix, isWhite, 0, -Mathf.Infinity, Mathf.Infinity);

        if (this.drawMove != null)
            if (this.drawMove.Equals(this.nextMove))
            {
                //BoardDrawn//
                //board.drawn = true;
            }
               

        state = 0;
        // this.thread.Abort();
        Debug.Log("end" + this.state);

    }*//*

    public int[,] generateBoardValueMatrix(ChessPiece[,] board)
    {
        int[,] matrix = new int[8, 8];
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(board[i, j] != null)
                {
                    matrix[i, j] = (int)board[i, j].type;
                    if (board[i,j].team != 0)
                    {
                        matrix[i, j] = (int)board[i, j].type*-1;
                    }

                }
                else
                {
                    matrix[i, j] = 0;
                }
            }
        }
        return matrix;
    }

    public static void ReverseMove(int[,] board, Move move, int old_piece)
    {
        Debug.Log("reverse Move");
        board[move.start[0], move.start[1]] = board[move.end[0], move.end[1]];
        board[move.end[0], move.end[1]] = old_piece;
       
 
    }

    public void reverseoriginalBoard(int[,] matrixBoard, Move move, ChessPiece temp)
    {
        chessBoard[move.start[0], move.start[1]] = chessBoard[move.end[0], move.end[1]];
        chessBoard[move.end[0], move.end[1]] = temp;
    }
    public static int ApplyMove(int[,] board, Move move)
    {
        Debug.Log("AI apply move");
        int temp = board[move.end[0], move.end[1]];
        board[move.end[0], move.end[1]] = board[move.start[0], move.start[1]];
        board[move.start[0], move.start[1]] = 0;
        return temp;
    }

    public ChessPiece ApplyMoveOnOriginalBoard(Move move)
    {
        Debug.Log("AI apply move");

        ChessPiece temp = chessBoard[move.end[0], move.end[1]];
        chessBoard[move.end[0], move.end[1]] = chessBoard[move.start[0], move.start[1]];
        chessBoard[move.start[0], move.start[1]] = null;
        temp.movePiece(new Vector3(0, 0, 0));
        return temp;
    }

    void ThreadFunc()
    {
        Debug.Log("start " + this.state);
        this.nextMove = MiniMax(generateBoardValueMatrix(chessBoard), isWhite, 0, -Mathf.Infinity, Mathf.Infinity);

        if (this.drawMove != null)
            if (this.drawMove.Equals(this.nextMove))
                boardManager.drawn = true;

        state = 0;
        // this.thread.Abort();
        Debug.Log("end" + this.state);

    }
    public Move getMiniMaxMove()
    {
        return MiniMax(generateBoardValueMatrix(chessBoard), isWhite, 0, -Mathf.Infinity, Mathf.Infinity);
    }
}
*/