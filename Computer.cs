﻿﻿﻿using System;

internal class Computer
{
    private byte m_PlayerScore;
    private const string k_Computer = "Computer";
    private readonly OthelloGame.eGameToken r_ComputerToken;
    private Minimax m_MinimaxTree;
    private OthelloGame m_TheGame;

    public Computer(OthelloGame.eGameToken i_Token, OthelloGame i_Game)
    {
        r_ComputerToken = i_Token;
        m_TheGame = i_Game;
        m_MinimaxTree = new Minimax(m_TheGame.GameBoardSize);
    }

    internal OthelloGame.eGameToken ComputerToken
    {
        get { return r_ComputerToken; }
    }

    internal string ComputerName
    {
        get { return k_Computer; }
    }

    internal byte PlayerScore
    {
        get { return m_PlayerScore; }

        set { m_PlayerScore = value; }
    }

    internal static void CopyTokensMatrice(OthelloGame.eGameToken[,] i_CopyFromMatrix, OthelloGame.eGameToken[,] i_CopyToMatrix, byte i_MatrixSize)
    {
        for (byte i = 0; i < i_MatrixSize; ++i)
        {
            for (byte j = 0; j < i_MatrixSize; ++j)
            {
                i_CopyToMatrix.SetValue(i_CopyFromMatrix.GetValue(i, j), i, j);
            }
        }
    }

    internal void GetDecisionFromComputer(ref byte io_Row, ref byte io_Col)
    {
        setComputerDecisions();
        setOpponentDecisions();
        foreach (Minimax.Decision decision in m_MinimaxTree.Decisions)
        {
            decision.SetMinValueDecision();
        }

        m_MinimaxTree.CalculateMaxValueOfEntireTree(ref io_Row, ref io_Col);
        m_MinimaxTree.CreateNewTree();
    }

    private void setComputerDecisions()
    {
        OthelloGame.eGameToken[,] matrixOfTheGameToRemember = new OthelloGame.eGameToken[m_TheGame.GameBoardSize, m_TheGame.GameBoardSize];
        OthelloGame.eGameToken[,] matrixOfTheGameToDecisionsTree = m_TheGame.GameStatus;

        for (byte i = 0; i < m_TheGame.GameBoardSize; ++i)
        {
            for (byte j = 0; j < m_TheGame.GameBoardSize; ++j)
            {
                CopyTokensMatrice(matrixOfTheGameToDecisionsTree, matrixOfTheGameToRemember, m_TheGame.GameBoardSize);
                if (m_TheGame.IsLegalMovement(i, j))
                {
                    m_TheGame.AddTokenFromUserToBoard(i, j);
                    m_TheGame.AddWonTokensToBoard(i, j);
                    m_MinimaxTree.AddDesicionToTree(i, j, matrixOfTheGameToDecisionsTree, ComputerToken);
                    CopyTokensMatrice(matrixOfTheGameToRemember, matrixOfTheGameToDecisionsTree, m_TheGame.GameBoardSize);
                }
            }
        }
    }

    private void setOpponentDecisions()
    {
        foreach (Minimax.Decision decisionOfOpponent in m_MinimaxTree.Decisions)
        {
            byte row = decisionOfOpponent.RowOfDecision;
            byte col = decisionOfOpponent.ColumnOfDecision;
            OthelloGame.eGameToken[,] matrixOfTheGameToOpponentsDecisionsTree = m_TheGame.GameStatus;
            OthelloGame.eGameToken[,] matrixOfTheGameToRemember = new OthelloGame.eGameToken[m_TheGame.GameBoardSize, m_TheGame.GameBoardSize];
            OthelloGame.eGameToken[,] matrixToRememberBeforeOpponentsTurn = new OthelloGame.eGameToken[m_TheGame.GameBoardSize, m_TheGame.GameBoardSize];

            CopyTokensMatrice(matrixOfTheGameToOpponentsDecisionsTree, matrixOfTheGameToRemember, m_TheGame.GameBoardSize);
            m_TheGame.AddTokenFromUserToBoard(row, col);
            m_TheGame.AddWonTokensToBoard(row, col);
            m_TheGame.ChangePlayerTurn();
            findDecisionsForOpponent(matrixOfTheGameToOpponentsDecisionsTree, matrixToRememberBeforeOpponentsTurn, decisionOfOpponent);
            CopyTokensMatrice(matrixOfTheGameToRemember, matrixOfTheGameToOpponentsDecisionsTree, m_TheGame.GameBoardSize);
            m_TheGame.ChangePlayerTurn();
        }
    }

    private void findDecisionsForOpponent(OthelloGame.eGameToken[,] i_MatrixOfTheGameToOpponentsDecisionsTree, OthelloGame.eGameToken[,] i_MatrixToRememberBeforeOpponentsTurn, Minimax.Decision i_DecisionOfOpponent)
    {
        for (byte i = 0; i < m_TheGame.GameBoardSize; ++i)
        {
            for (byte j = 0; j < m_TheGame.GameBoardSize; ++j)
            {
                CopyTokensMatrice(i_MatrixOfTheGameToOpponentsDecisionsTree, i_MatrixToRememberBeforeOpponentsTurn, m_TheGame.GameBoardSize);
                if (m_TheGame.IsLegalMovement(i, j))
                {
                    m_TheGame.AddTokenFromUserToBoard(i, j);
                    m_TheGame.AddWonTokensToBoard(i, j);
                    i_DecisionOfOpponent.AddOpponentDecision(i, j, i_MatrixOfTheGameToOpponentsDecisionsTree, m_TheGame.GetCurrentPlayerToken());
                    CopyTokensMatrice(i_MatrixToRememberBeforeOpponentsTurn, i_MatrixOfTheGameToOpponentsDecisionsTree, m_TheGame.GameBoardSize);
                }
            }
        }
    }
}