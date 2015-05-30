﻿﻿﻿﻿﻿using System;

public class OthelloGame
{
    private Board m_GameBoard;
    private Player m_humanFirstOpponent;
    private Player m_humanSecondOpponent;
    private Computer m_CompOpponent;
    private eCurrentTurn m_Turn = eCurrentTurn.WhiteOpponentTurn;

    private enum eCurrentTurn : byte
    {
        WhiteOpponentTurn = 1,
        BlackOpponentTurn = 2
    }

    internal enum eGameToken : byte
    {
        WhiteToken = 1,
        BlackToken = 2,
        Empty = 3
    }

    internal OthelloGame.eGameToken[,] GameStatus
    {
        get { return m_GameBoard.GameBoard; }

        set { m_GameBoard.GameBoard = value; }
    }

    internal byte GameBoardSize
    {
        get { return m_GameBoard.BoardSize; }

        set
        {
            m_GameBoard = new Board(value);
            m_GameBoard.ClearBoard();
        }
    }

    internal bool IsBoardSizeValid(byte i_BoardSize)
    {
        return Board.IsFittedToBounderiesOfBoardSize(i_BoardSize);
    }

    internal void ChangePlayerTurn()
    {
        if (m_Turn == eCurrentTurn.WhiteOpponentTurn)
        {
            m_Turn = eCurrentTurn.BlackOpponentTurn;
        }
        else
        {
            m_Turn = eCurrentTurn.WhiteOpponentTurn;
        }
    }

    internal bool IsComputerTurn()
    {
        bool isComputerTurn = false;

        if (m_CompOpponent != null)
        {
            eGameToken compToken = m_CompOpponent.ComputerToken;
            if (compToken == GetCurrentPlayerToken())
            {
                isComputerTurn = true;
            }
        }

        return isComputerTurn;
    }

    internal void PlayComputerTurn()
    {
        byte row = 0;
        byte col = 0;

        m_CompOpponent.GetDecisionFromComputer(ref row, ref col);
        GameStatus.SetValue(this.GetCurrentPlayerToken(), row, col);
        this.AddWonTokensToBoard(row, col);
    }

    internal eGameToken GetCurrentPlayerToken()
    {
        eGameToken currentToken = eGameToken.BlackToken;

        if (m_Turn == eCurrentTurn.WhiteOpponentTurn)
        {
            currentToken = eGameToken.WhiteToken;
        }

        return currentToken;
    }

    private eGameToken getOppositeCurrentPlayerToken()
    {
        eGameToken currentToken = this.GetCurrentPlayerToken();

        return (currentToken == eGameToken.BlackToken) ? eGameToken.WhiteToken : eGameToken.BlackToken;
    }

    internal void PlacingFirstGameTokens()
    {
        byte middleOfBoard = (byte)((GameBoardSize / 2) - 1);

        GameStatus.SetValue(eGameToken.BlackToken, middleOfBoard, middleOfBoard);
        GameStatus.SetValue(eGameToken.BlackToken, middleOfBoard + 1, middleOfBoard + 1);
        GameStatus.SetValue(eGameToken.WhiteToken, middleOfBoard, middleOfBoard + 1);
        GameStatus.SetValue(eGameToken.WhiteToken, middleOfBoard + 1, middleOfBoard);
    }

    internal void CreateHumanFirstOpponent(string i_PlayerName)
    {
        m_humanFirstOpponent = new Player(i_PlayerName, eGameToken.WhiteToken);
    }

    internal void CreateHumanSecondOpponent(string i_PlayerName)
    {
        m_humanSecondOpponent = new Player(i_PlayerName, eGameToken.BlackToken);
    }

    internal void CreateComputerOpponent()
    {
        m_CompOpponent = new Computer(eGameToken.BlackToken, this);
    }

    internal string GetCurrentPlayerName()
    {
        string playerName = string.Empty;

        if (m_Turn == eCurrentTurn.WhiteOpponentTurn)
        {
            playerName = m_humanFirstOpponent.PlayerName;
        }
        else
        {
            if (m_humanSecondOpponent != null)
            {
                playerName = m_humanSecondOpponent.PlayerName;
            }
            else
            {
                playerName = m_CompOpponent.ComputerName;
            }
        }

        return playerName;
    }

    internal bool IsExistingSquareOnBoard(byte i_ChosenRow, byte i_ChosenColumn)
    {
        return i_ChosenRow < GameBoardSize && i_ChosenRow >= 0 &&
            i_ChosenColumn < GameBoardSize && i_ChosenColumn >= 0;
    }

    internal bool IsLegalMovement(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool isBlockerTokenExist = false;
        eGameToken currentSquare = GameStatus[i_ChosenRow, i_ChosenColumn];

        if (currentSquare == eGameToken.Empty)
        {
            if (availableTokenPlacementFromDownToUp(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementFromUpToDown(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementFromLeftToRight(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementFromRightToLeft(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementDiagonalFromDownToUpFromLeftToRight(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementDiagonalFromDownToUpFromRightToLeft(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementDiagonalFromUpToDownFromLeftToRight(i_ChosenRow, i_ChosenColumn) ||
                availableTokenPlacementDiagonalFromUpToDownFromRightToLeft(i_ChosenRow, i_ChosenColumn))
            {
                isBlockerTokenExist = true;
            }
        }

        return isBlockerTokenExist;
    }

    private bool availableTokenPlacementFromDownToUp(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availablePlaceFromDown = false;

        if (i_ChosenRow > 1)
        {
            eGameToken oneSquareUp = this.GameStatus[i_ChosenRow - 1, i_ChosenColumn];
            if (oneSquareUp == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenRow - 1;
                while ((i >= 0) && (this.GameStatus[i, i_ChosenColumn] != eGameToken.Empty))
                {
                    if (this.GameStatus[i, i_ChosenColumn] == this.GetCurrentPlayerToken())
                    {
                        availablePlaceFromDown = true;
                    }

                    --i;
                }
            }
        }

        return availablePlaceFromDown;
    }

    private bool availableTokenPlacementFromUpToDown(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availablePlaceFromDown = false;

        if (i_ChosenRow < GameBoardSize - 2)
        {
            eGameToken oneSquareDown = this.GameStatus[i_ChosenRow + 1, i_ChosenColumn];
            if (oneSquareDown == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenRow + 1;
                while ((i < GameBoardSize) && (this.GameStatus[i, i_ChosenColumn] != eGameToken.Empty))
                {
                    if (this.GameStatus[i, i_ChosenColumn] == this.GetCurrentPlayerToken())
                    {
                        availablePlaceFromDown = true;
                    }

                    ++i;
                }
            }
        }

        return availablePlaceFromDown;
    }

    private bool availableTokenPlacementFromLeftToRight(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availablePlaceFromDown = false;

        if (i_ChosenColumn < GameBoardSize - 2)
        {
            eGameToken oneSquareRight = this.GameStatus[i_ChosenRow, i_ChosenColumn + 1];
            if (oneSquareRight == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenColumn + 1;
                while ((i < GameBoardSize) && (this.GameStatus[i_ChosenRow, i] != eGameToken.Empty))
                {
                    if (this.GameStatus[i_ChosenRow, i] == this.GetCurrentPlayerToken())
                    {
                        availablePlaceFromDown = true;
                    }

                    ++i;
                }
            }
        }

        return availablePlaceFromDown;
    }

    private bool availableTokenPlacementFromRightToLeft(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availablePlaceFromDown = false;

        if (i_ChosenColumn > 1)
        {
            eGameToken oneSquareLeft = this.GameStatus[i_ChosenRow, i_ChosenColumn - 1];
            if (oneSquareLeft == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenColumn - 1;
                while ((i >= 0) && (this.GameStatus[i_ChosenRow, i] != eGameToken.Empty))
                {
                    if (this.GameStatus[i_ChosenRow, i] == this.GetCurrentPlayerToken())
                    {
                        availablePlaceFromDown = true;
                    }

                    --i;
                }
            }
        }

        return availablePlaceFromDown;
    }

    private bool availableTokenPlacementDiagonalFromDownToUpFromLeftToRight(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availableTokenPlacement = false;

        if (i_ChosenRow > 1 && i_ChosenColumn < GameBoardSize - 2)
        {
            eGameToken oneSquareRightUp = this.GameStatus[i_ChosenRow - 1, i_ChosenColumn + 1];
            if (oneSquareRightUp == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenRow - 1;
                int j = i_ChosenColumn + 1;
                while ((i >= 0) && (j < GameBoardSize) && (this.GameStatus[i, j] != eGameToken.Empty))
                {
                    if (this.GameStatus[i, j] == this.GetCurrentPlayerToken())
                    {
                        availableTokenPlacement = true;
                    }

                    --i;
                    ++j;
                }
            }
        }

        return availableTokenPlacement;
    }

    private bool availableTokenPlacementDiagonalFromDownToUpFromRightToLeft(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availableTokenPlacement = false;

        if (i_ChosenRow > 1 && i_ChosenColumn > 1)
        {
            eGameToken oneSquareLeftUp = this.GameStatus[i_ChosenRow - 1, i_ChosenColumn - 1];
            if (oneSquareLeftUp == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenRow - 1;
                int j = i_ChosenColumn - 1;
                while ((i >= 0) && (j >= 0) && (this.GameStatus[i, j] != eGameToken.Empty))
                {
                    if (this.GameStatus[i, j] == this.GetCurrentPlayerToken())
                    {
                        availableTokenPlacement = true;
                    }

                    --i;
                    --j;
                }
            }
        }

        return availableTokenPlacement;
    }

    private bool availableTokenPlacementDiagonalFromUpToDownFromLeftToRight(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availableTokenPlacement = false;

        if (i_ChosenRow < GameBoardSize - 2 && i_ChosenColumn < GameBoardSize - 2)
        {
            eGameToken oneSquareRightDown = this.GameStatus[i_ChosenRow + 1, i_ChosenColumn + 1];
            if (oneSquareRightDown == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenRow + 1;
                int j = i_ChosenColumn + 1;
                while ((i < GameBoardSize) && (j < GameBoardSize) && (this.GameStatus[i, j] != eGameToken.Empty))
                {
                    if (this.GameStatus[i, j] == this.GetCurrentPlayerToken())
                    {
                        availableTokenPlacement = true;
                    }

                    ++i;
                    ++j;
                }
            }
        }

        return availableTokenPlacement;
    }

    private bool availableTokenPlacementDiagonalFromUpToDownFromRightToLeft(byte i_ChosenRow, byte i_ChosenColumn)
    {
        bool availableTokenPlacement = false;

        if (i_ChosenRow < GameBoardSize - 2 && i_ChosenColumn > 1)
        {
            eGameToken oneSquareLeftDown = this.GameStatus[i_ChosenRow + 1, i_ChosenColumn - 1];
            if (oneSquareLeftDown == this.getOppositeCurrentPlayerToken())
            {
                int i = i_ChosenRow + 1;
                int j = i_ChosenColumn - 1;
                while ((i < GameBoardSize) && (j >= 0) && (this.GameStatus[i, j] != eGameToken.Empty))
                {
                    if (this.GameStatus[i, j] == this.GetCurrentPlayerToken())
                    {
                        availableTokenPlacement = true;
                    }

                    ++i;
                    --j;
                }
            }
        }

        return availableTokenPlacement;
    }

    internal bool IsGameOver()
    {
        bool gameOver = false;

        if (!ThereIsAvailableMovementForPlayerOnBoard())
        {
            this.ChangePlayerTurn();
            if (!ThereIsAvailableMovementForPlayerOnBoard())
            {
                gameOver = true;
            }

            ChangePlayerTurn();
        }

        return gameOver;
    }

    internal void AddTokenFromUserToBoard(byte i_Row, byte i_Column)
    {
        GameStatus.SetValue(this.GetCurrentPlayerToken(), i_Row, i_Column);
    }

    internal void AddWonTokensToBoard(byte i_Row, byte i_Column)
    {
        if (availableTokenPlacementFromLeftToRight(i_Row, i_Column))
        {
            addWonTokensFromLeftToRight(i_Row, i_Column);
        }

        if (availableTokenPlacementFromRightToLeft(i_Row, i_Column))
        {
            addWonTokensFromRightToLeft(i_Row, i_Column);
        }

        if (availableTokenPlacementFromUpToDown(i_Row, i_Column))
        {
            addWonTokensFromUpToDown(i_Row, i_Column);
        }

        if (availableTokenPlacementFromDownToUp(i_Row, i_Column))
        {
            addWonTokensFromDownToUp(i_Row, i_Column);
        }

        if (availableTokenPlacementDiagonalFromUpToDownFromLeftToRight(i_Row, i_Column))
        {
            addWonTokensFromUpToDownFromLeftToRight(i_Row, i_Column);
        }

        if (availableTokenPlacementDiagonalFromUpToDownFromRightToLeft(i_Row, i_Column))
        {
            addWonTokensFromUpToDownFromRightToLeft(i_Row, i_Column);
        }

        if (availableTokenPlacementDiagonalFromDownToUpFromLeftToRight(i_Row, i_Column))
        {
            addWonTokensFromDownToUpFromLeftToRight(i_Row, i_Column);
        }

        if (availableTokenPlacementDiagonalFromDownToUpFromRightToLeft(i_Row, i_Column))
        {
            addWonTokensFromDownToUpFromRightToLeft(i_Row, i_Column);
        }
    }

    private void addWonTokensFromUpToDownFromLeftToRight(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenColumn + 1;
        int j = i_ChosenRow + 1;

        while ((i < GameBoardSize) && (j < GameBoardSize) && (this.GameStatus[j, i] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), j, i);
            ++i;
            ++j;
        }
    }

    private void addWonTokensFromUpToDownFromRightToLeft(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenColumn - 1;
        int j = i_ChosenRow + 1;

        while ((i > 0) && (j < GameBoardSize) && (this.GameStatus[j, i] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), j, i);
            --i;
            ++j;
        }
    }

    private void addWonTokensFromDownToUpFromRightToLeft(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenColumn - 1;
        int j = i_ChosenRow - 1;

        while ((i > 0) && (j > 0) && (this.GameStatus[j, i] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), j, i);
            --i;
            --j;
        }
    }

    private void addWonTokensFromDownToUpFromLeftToRight(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenColumn + 1;
        int j = i_ChosenRow - 1;

        while ((i < GameBoardSize) && (j > 0) && (this.GameStatus[j, i] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), j, i);
            ++i;
            --j;
        }
    }

    private void addWonTokensFromLeftToRight(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenColumn + 1;

        while ((i < GameBoardSize) && (this.GameStatus[i_ChosenRow, i] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), i_ChosenRow, i);
            ++i;
        }
    }

    private void addWonTokensFromRightToLeft(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenColumn - 1;

        while ((i > 0) && (this.GameStatus[i_ChosenRow, i] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), i_ChosenRow, i);
            --i;
        }
    }

    private void addWonTokensFromUpToDown(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenRow + 1;

        while ((i < GameBoardSize) && (this.GameStatus[i, i_ChosenColumn] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), i, i_ChosenColumn);
            ++i;
        }
    }

    private void addWonTokensFromDownToUp(byte i_ChosenRow, byte i_ChosenColumn)
    {
        int i = i_ChosenRow - 1;

        while ((i > 0) && (this.GameStatus[i, i_ChosenColumn] == getOppositeCurrentPlayerToken()))
        {
            GameStatus.SetValue(this.GetCurrentPlayerToken(), i, i_ChosenColumn);
            --i;
        }
    }

    internal bool ThereIsAvailableMovementForPlayerOnBoard()
    {
        bool availableMove = false;

        for (byte i = 0; i < GameBoardSize; ++i)
        {
            for (byte j = 0; j < GameBoardSize; ++j)
            {
                if (this.IsLegalMovement(i, j))
                {
                    availableMove = true;
                }
            }
        }

        return availableMove;
    }

    internal void CountingPlayersTokens()
    {
        byte amountOfWhiteTokens = 0;
        byte amountOfBlackTokens = 0;

        foreach (eGameToken tokenType in GameStatus)
        {
            if (eGameToken.BlackToken == tokenType)
            {
                amountOfBlackTokens++;
            }

            if (eGameToken.WhiteToken == tokenType)
            {
                amountOfWhiteTokens++;
            }
        }

        this.playersPointsDistributions(amountOfWhiteTokens, amountOfBlackTokens);
    }

    private void playersPointsDistributions(byte i_WhiteToken, byte i_BlackToken)
    {
        m_humanFirstOpponent.PlayerScore = i_WhiteToken;
        if (m_CompOpponent == null)
        {
            m_humanSecondOpponent.PlayerScore = i_BlackToken;
        }
        else
        {
            m_CompOpponent.PlayerScore = i_BlackToken;
        }
    }

    internal byte GetPlayerScore(eGameToken i_Token)
    {
        byte playerScore = 0;

        if (i_Token == eGameToken.WhiteToken)
        {
            playerScore = m_humanFirstOpponent.PlayerScore;
        }
        else
        {
            if (m_CompOpponent == null)
            {
                playerScore = m_humanSecondOpponent.PlayerScore;
            }
            else
            {
                playerScore = m_CompOpponent.PlayerScore;
            }
        }

        return playerScore;
    }

    internal string WinnerDecision()
    {
        string player1 = m_humanFirstOpponent.PlayerName;
        string player2 = string.Empty;
        string winner = string.Empty;

        if (m_CompOpponent == null)
        {
            player2 = m_humanSecondOpponent.PlayerName;
        }
        else
        {
            player2 = m_CompOpponent.ComputerName;
        }

        if (this.GetPlayerScore(eGameToken.BlackToken) > this.GetPlayerScore(eGameToken.WhiteToken))
        {
            winner = player2;
        }
        else if (this.GetPlayerScore(eGameToken.BlackToken) < this.GetPlayerScore(eGameToken.WhiteToken))
        {
            winner = player1;
        }

        return winner;
    }

    internal void CleanOpponents()
    {
        m_Turn = eCurrentTurn.WhiteOpponentTurn;
        m_humanFirstOpponent = null;
        if (m_CompOpponent == null)
        {
            m_humanSecondOpponent = null;
        }
        else
        {
            m_CompOpponent = null;
        }
    }
}