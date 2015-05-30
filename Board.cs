using System;

internal class Board
{
    private static byte s_MinBoardLength = 6;
    private static byte s_MaxBoardLength = 8;
    private byte m_BoardSize;
    private OthelloGame.eGameToken[,] m_GameBoard;

    internal static byte MinBoardLength
    {
        get { return s_MinBoardLength; }
    }

    internal static byte MaxBoardLength
    {
        get { return s_MaxBoardLength; }
    }

    internal byte BoardSize
    {
        get { return m_BoardSize; }

        set { m_BoardSize = value; }
    }

    internal OthelloGame.eGameToken[,] GameBoard
    {
        get { return m_GameBoard; }

        set { m_GameBoard = value; }
    }

    public Board(byte i_BoardSize)
    {
        BoardSize = i_BoardSize;
        GameBoard = new OthelloGame.eGameToken[i_BoardSize, i_BoardSize];
    }

    internal static bool IsFittedToBounderiesOfBoardSize(byte i_BoardSize)
    {
        return i_BoardSize == MinBoardLength || i_BoardSize == MaxBoardLength;
    }

    internal void ClearBoard()
    {
        for (int i = 0; i < BoardSize; ++i)
        {
            for (int j = 0; j < BoardSize; ++j)
            {
                GameBoard[i, j] = OthelloGame.eGameToken.Empty;
            }
        }
    }
}