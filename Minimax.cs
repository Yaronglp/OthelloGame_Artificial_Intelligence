﻿using System;
using System.Collections.Generic;
using System.Text;

public class Minimax
{
    private static byte s_MatrixSize;
    private int m_MinimaxFinalValue;
    private LinkedList<Decision> m_Decisions;

    public Minimax(byte i_SizeOfMatrix)
    {
        s_MatrixSize = i_SizeOfMatrix;
        Decisions = new LinkedList<Decision>();
    }

    internal LinkedList<Decision> Decisions
    {
        get { return m_Decisions; }

        set { m_Decisions = value; }
    }

    internal void CreateNewTree()
    {
        Decisions = new LinkedList<Decision>();
    }

    internal void CalculateMaxValueOfEntireTree(ref byte io_Row, ref byte io_Col)
    {
        int currMaxValueDecision = int.MinValue;
        io_Row = 0;
        io_Col = 0;

        foreach (Decision decisionInTree in m_Decisions)
        {
            if (decisionInTree.DecisionValue > currMaxValueDecision)
            {
                currMaxValueDecision = decisionInTree.DecisionValue;
                io_Row = decisionInTree.RowOfDecision;
                io_Col = decisionInTree.ColumnOfDecision;
            }
        }

        m_MinimaxFinalValue = currMaxValueDecision;
    }

    internal void AddDesicionToTree(byte i_Row, byte i_Col, OthelloGame.eGameToken[,] i_MatrixOfDecision, OthelloGame.eGameToken i_Token)
    {
        m_Decisions.AddLast(new Decision(i_Row, i_Col, i_MatrixOfDecision, i_Token));
    }

    internal class Decision
    {
        private OthelloGame.eGameToken[,] m_MatrixOfDecision;
        private LinkedList<OpponentDecision> m_OpponentDecisions;
        private OthelloGame.eGameToken m_TokenOfComputer;
        private byte m_RowOfDecision;
        private byte m_ColOfDecision;
        private int m_valueOfDecision;

        internal Decision(byte i_RowOfDecision, byte i_ColOfDecision, OthelloGame.eGameToken[,] i_currGameMatrix, OthelloGame.eGameToken i_Token)
        {
            RowOfDecision = i_RowOfDecision;
            ColumnOfDecision = i_ColOfDecision;
            m_MatrixOfDecision = new OthelloGame.eGameToken[s_MatrixSize, s_MatrixSize];
            Computer.CopyTokensMatrice(i_currGameMatrix, m_MatrixOfDecision, s_MatrixSize);
            m_TokenOfComputer = i_Token;
            m_OpponentDecisions = new LinkedList<OpponentDecision>();
            setDecisionInsideMatrix();
        }

        internal int DecisionValue
        {
            get { return m_valueOfDecision; }

            set { m_valueOfDecision = value; }
        }

        internal byte RowOfDecision
        {
            get { return m_RowOfDecision; }

            set { m_RowOfDecision = value; }
        }

        internal byte ColumnOfDecision
        {
            get { return m_ColOfDecision; }

            set { m_ColOfDecision = value; }
        }

        internal void SetMinValueDecision()
        {
            int minValue = int.MaxValue;

            foreach (OpponentDecision currCandidateMinOpponentDecision in m_OpponentDecisions)
            {
                if (minValue > currCandidateMinOpponentDecision.ValueOfopponentDecisionMatrix)
                {
                    minValue = currCandidateMinOpponentDecision.ValueOfopponentDecisionMatrix;
                }
            }

            DecisionValue = minValue;
        }

        private void setDecisionInsideMatrix()
        {
            m_MatrixOfDecision.SetValue(m_TokenOfComputer, m_RowOfDecision, m_ColOfDecision);
        }

        internal void AddOpponentDecision(byte i_RowOfOpponentDecision, byte i_ColOfOpponentDecision, OthelloGame.eGameToken[,] i_MatrixOfDecision, OthelloGame.eGameToken i_OpponentToken)
        {
            OpponentDecision opp = new OpponentDecision(i_RowOfOpponentDecision, i_ColOfOpponentDecision, i_MatrixOfDecision, i_OpponentToken);
            this.m_OpponentDecisions.AddLast(opp);
        }
    }

    internal class OpponentDecision
    {
        private const int k_CornerValue = 150;
        private const int k_EdgeValue = 30;
        private const int k_TokenValue = 20;
        private OthelloGame.eGameToken[,] m_MatrixOfOpponentDecision;
        private OthelloGame.eGameToken m_TokenOfOpponent;
        private byte m_RowOfOpponentDecision;
        private byte m_ColOfOpponentDecision;
        private int m_ValueOfOpponentDecisionMatrix;

        internal OpponentDecision(byte i_RowOfOpponentDecision, byte i_ColOfOpponentDecision, OthelloGame.eGameToken[,] i_MatrixOfOpponentDecision, OthelloGame.eGameToken i_TokenOfOpponent)
        {
            m_RowOfOpponentDecision = i_RowOfOpponentDecision;
            m_ColOfOpponentDecision = i_ColOfOpponentDecision;
            m_MatrixOfOpponentDecision = new OthelloGame.eGameToken[s_MatrixSize, s_MatrixSize];
            Computer.CopyTokensMatrice(i_MatrixOfOpponentDecision, m_MatrixOfOpponentDecision, s_MatrixSize);
            m_TokenOfOpponent = i_TokenOfOpponent;
            ValueOfopponentDecisionMatrix = calculateValueOfOpponentDecision();
        }

        internal int ValueOfopponentDecisionMatrix
        {
            get { return m_ValueOfOpponentDecisionMatrix; }

            set { m_ValueOfOpponentDecisionMatrix = value; }
        }

        private int calculateValueOfOpponentDecision()
        {
            int totalValue = 0;

            totalValue = calculateTokensValues() + calculateCornersValues() + calculateEdges();

            return totalValue;
        }

        private int calculateTokensValues()
        {
            int currentValueOfTokensInMatrix = 0;

            for (int i = 0; i < Minimax.s_MatrixSize; i++)
            {
                for (int j = 0; j < Minimax.s_MatrixSize; j++)
                {
                    currentValueOfTokensInMatrix += calculateCurrentTokenInMatrixAccordingToSpecificValue(m_MatrixOfOpponentDecision[i, j], k_TokenValue);
                }
            }

            return currentValueOfTokensInMatrix;
        }

        private int calculateCornersValues()
        {
            OthelloGame.eGameToken leftUpCorner = m_MatrixOfOpponentDecision[0, 0];
            OthelloGame.eGameToken leftDownCorner = m_MatrixOfOpponentDecision[Minimax.s_MatrixSize - 1, 0];
            OthelloGame.eGameToken RightUpCorner = m_MatrixOfOpponentDecision[0, Minimax.s_MatrixSize - 1];
            OthelloGame.eGameToken RightDownCorner = m_MatrixOfOpponentDecision[Minimax.s_MatrixSize - 1, Minimax.s_MatrixSize - 1];
            int valueOfCorners = 0;

            valueOfCorners += calculateCurrentTokenInMatrixAccordingToSpecificValue(leftUpCorner, k_CornerValue);
            valueOfCorners += calculateCurrentTokenInMatrixAccordingToSpecificValue(leftDownCorner, k_CornerValue);
            valueOfCorners += calculateCurrentTokenInMatrixAccordingToSpecificValue(RightUpCorner, k_CornerValue);
            valueOfCorners += calculateCurrentTokenInMatrixAccordingToSpecificValue(RightDownCorner, k_CornerValue);

            return valueOfCorners;
        }

        private int calculateEdges()
        {
            int valueOfEdges = 0;

            valueOfEdges += calculateUpEdge();
            valueOfEdges += calculateDownEdge();
            valueOfEdges += calculateRightEdge();
            valueOfEdges += calculateLeftEdge();

            return valueOfEdges;
        }

        private int calculateUpEdge()
        {
            int valueOfUpEdge = 0;

            for (int i = 1; i < Minimax.s_MatrixSize - 1; ++i)
            {
                valueOfUpEdge += calculateCurrentTokenInMatrixAccordingToSpecificValue(m_MatrixOfOpponentDecision[0, i], k_EdgeValue);
            }

            return valueOfUpEdge;
        }

        private int calculateDownEdge()
        {
            int valueOfDownEdge = 0;

            for (int i = 1; i < Minimax.s_MatrixSize - 1; ++i)
            {
                valueOfDownEdge += calculateCurrentTokenInMatrixAccordingToSpecificValue(m_MatrixOfOpponentDecision[Minimax.s_MatrixSize - 1, i], k_EdgeValue);
            }

            return valueOfDownEdge;
        }

        private int calculateLeftEdge()
        {
            int valueOfLeftEdge = 0;

            for (int i = 1; i < Minimax.s_MatrixSize - 1; ++i)
            {
                valueOfLeftEdge += calculateCurrentTokenInMatrixAccordingToSpecificValue(m_MatrixOfOpponentDecision[i, 0], k_EdgeValue);
            }

            return valueOfLeftEdge;
        }

        private int calculateRightEdge()
        {
            int valueOfRightEdge = 0;

            for (int i = 1; i < Minimax.s_MatrixSize - 1; ++i)
            {
                valueOfRightEdge += calculateCurrentTokenInMatrixAccordingToSpecificValue(m_MatrixOfOpponentDecision[i, Minimax.s_MatrixSize - 1], k_EdgeValue);
            }

            return valueOfRightEdge;
        }

        private int calculateCurrentTokenInMatrixAccordingToSpecificValue(OthelloGame.eGameToken i_CurrTokenInMatrix, int i_SpecificValue)
        {
            int tokenValue = 0;

            if (i_CurrTokenInMatrix == m_TokenOfOpponent)
            {
                tokenValue -= i_SpecificValue;
            }
            else
            {
                if (i_CurrTokenInMatrix != OthelloGame.eGameToken.Empty)
                {
                    tokenValue += i_SpecificValue;
                }
            }

            return tokenValue;
        }
    }
}