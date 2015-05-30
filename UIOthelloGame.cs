﻿﻿﻿﻿using System;
using System.Threading;
using Ex02.ConsoleUtils;

public class UIOthelloGame
{
    private static OthelloGame s_TheGame = new OthelloGame();
    private const char k_WhiteTokenRep = 'O';
    private const char k_BlackTokenRep = 'X';
    private const char k_EmptyTokenRep = ' ';

    private enum eOpponentUserChoice : byte
    {
        Computer = 1,
        Human = 2
    }

    private enum eMenu : byte
    {
        PlayGame = 1,
        Exit = 2
    }

    public void StartGame()
    {
        Console.WriteLine("\t\t*** Welcome to Othello Game ***");
        this.userChoiceFromMainMenu();
    }

    private void userChoiceFromMainMenu()
    {
        byte userChoice = 0;
        bool wrongInput = true;

        while (wrongInput)
        {
            Console.WriteLine("What do you want to do ?{0}1 - Play game{0}2 - Exit", Environment.NewLine);
            if (!this.validatePlayerChoiceFormat(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("You have typed illegal number format!{0}Please try again.{0}", Environment.NewLine);
            }
            else if (!isExistingMainMenuOption((eMenu)userChoice))
            {
                Console.WriteLine("You have typed non existing number from the menu!{0}Please try again.{0}", Environment.NewLine);
            }
            else
            {
                wrongInput = false;
            }
        }
    }

    private bool isExistingMainMenuOption(eMenu i_MenuChoice)
    {
        bool isExistChoice = true;

        switch (i_MenuChoice)
        {
            case eMenu.PlayGame:
                this.configureGameScreen();
                break;
            case eMenu.Exit:
                Console.WriteLine("Have a good day.");
                break;
            default:
                isExistChoice = false;
                break;
        }

        return isExistChoice;
    }

    private bool validatePlayerChoiceFormat(string i_StrFromUser, out byte o_Result)
    {
        return byte.TryParse(i_StrFromUser, out o_Result);
    }

    private void configureGameScreen()
    {
        this.chooseFirstPlayer();
        this.userChoiceForBoardSize();
        this.chooseOponnentTypeFromMenu();
        this.playTheGame();
    }

    private void chooseFirstPlayer()
    {
        s_TheGame.CreateHumanFirstOpponent(this.setPlayerName());
    }

    private string setPlayerName()
    {
        Console.Write("Please enter player name: ");

        return Console.ReadLine();
    }

    private void userChoiceForBoardSize()
    {
        byte userChoice = 0;
        bool wrongInput = true;

        while (wrongInput)
        {
            Console.WriteLine("What board size you wish to play with?{0}Choose {1} or {2}", Environment.NewLine, Board.MinBoardLength, Board.MaxBoardLength);
            if (!this.validatePlayerChoiceFormat(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("You have typed illegal number format!{0}Please try again.{0}", Environment.NewLine);
            }
            else if (!isExistingBoardSizeUserChoice(userChoice))
            {
                Console.WriteLine("You have typed number who is not {1} or {2}!{0}Please try again.{0}", Environment.NewLine, Board.MinBoardLength, Board.MaxBoardLength);
            }
            else
            {
                wrongInput = false;
            }
        }

        s_TheGame.GameBoardSize = userChoice;
    }

    private bool isExistingBoardSizeUserChoice(byte i_BoardSize)
    {
        return s_TheGame.IsBoardSizeValid(i_BoardSize);
    }

    private void chooseOponnentTypeFromMenu()
    {
        byte userChoice = 0;
        bool wrongInput = true;

        while (wrongInput)
        {
            Console.WriteLine("Who will be your opponent ?{0}1 - Computer{0}2 - Real human", Environment.NewLine);
            if (!this.validatePlayerChoiceFormat(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("You have typed illegal number format!{0}Please try again.{0}", Environment.NewLine);
            }
            else if (!isExistingOpponentMenuOption((eOpponentUserChoice)userChoice))
            {
                Console.WriteLine("You have typed non existing number from the menu!{0}Please try again.{0}", Environment.NewLine);
            }
            else
            {
                wrongInput = false;
            }
        }
    }

    private bool isExistingOpponentMenuOption(eOpponentUserChoice i_OpponentOption)
    {
        bool isExistOption = true;

        switch (i_OpponentOption)
        {
            case eOpponentUserChoice.Human:
                s_TheGame.CreateHumanSecondOpponent(setPlayerName());
                break;
            case eOpponentUserChoice.Computer:
                s_TheGame.CreateComputerOpponent();
                break;
            default:
                Console.WriteLine("You have typed wrong number!{0}Please try again.{0}", Environment.NewLine);
                isExistOption = false;
                break;
        }

        return isExistOption;
    }

    private void playTheGame()
    {
        s_TheGame.PlacingFirstGameTokens();
        bool endGameByUser = false;
        while (!s_TheGame.IsGameOver() && !endGameByUser)
        {
            Screen.Clear();
            this.displayCurrentBoardGameStatus();
            endGameByUser = this.playTurnOrQuit();
            s_TheGame.ChangePlayerTurn();
        }

        if (!endGameByUser)
        {
            Screen.Clear();
            this.displayCurrentBoardGameStatus();
            this.playersPoints();
            this.winnerDeclaration();
            this.playAnotherGame();
        }
        else
        {
            Console.WriteLine("Have a good day.");
        }
    }

    public void displayCurrentBoardGameStatus()
    {
        this.insertAlphaHeadlineToBoard();
        for (int i = 0; i < s_TheGame.GameBoardSize; ++i)
        {
            this.betweenLinesDesign();
            Console.Write("{0}{1}", Environment.NewLine, i + 1);
            for (int j = 0; j < s_TheGame.GameBoardSize; ++j)
            {
                Console.Write("| {0} ", displayToken(s_TheGame.GameStatus[i, j]));
            }

            Console.WriteLine("|");
        }

        this.betweenLinesDesign();
    }

    private void insertAlphaHeadlineToBoard()
    {
        for (int j = 0; j < s_TheGame.GameBoardSize; ++j)
        {
            Console.Write("   {0}", this.displayAlphaFromNumber(j));
        }

        Console.WriteLine("");
    }

    private char displayAlphaFromNumber(int i_Number)
    {
        return (char)(i_Number + 65);
    }

    private void betweenLinesDesign()
    {
        Console.Write(" ");
        for (int j = 0; j < s_TheGame.GameBoardSize; ++j)
        {
            Console.Write("====");
        }

        Console.Write("=");
    }

    private char displayToken(OthelloGame.eGameToken i_Token)
    {
        char tokenRep;

        switch (i_Token)
        {
            case OthelloGame.eGameToken.BlackToken:
                tokenRep = k_BlackTokenRep;
                break;
            case OthelloGame.eGameToken.WhiteToken:
                tokenRep = k_WhiteTokenRep;
                break;
            default:
                tokenRep = k_EmptyTokenRep;
                break;
        }

        return tokenRep;
    }

    private bool playTurnOrQuit()
    {
        bool endGame = false;

        if (s_TheGame.ThereIsAvailableMovementForPlayerOnBoard())
        {
            this.playersPoints();
            if (s_TheGame.IsComputerTurn())
            {
                Console.WriteLine("{0}Computer's Turn", Environment.NewLine);
                Thread.Sleep(2500);
                s_TheGame.PlayComputerTurn();
            }
            else
            {
                endGame = this.userSquareOrQuitFromGameChoice();
            }
        }
        else
        {
            Console.WriteLine("{0}{1} has no available movement on board, {1}'s turn is over.",
            Environment.NewLine, s_TheGame.GetCurrentPlayerName());
            Thread.Sleep(5000);
        }

        return endGame;
    }

    private bool userSquareOrQuitFromGameChoice()
    {
        byte row = 0;
        byte column = 0;
        string playerChoice = string.Empty;
        bool usertypedQKeyword = false;

        Console.WriteLine("{1}It's {0}'s turn!{1}Enter your desired square (For example: A3) or press Q to quit", s_TheGame.GetCurrentPlayerName(), Environment.NewLine);
        playerChoice = Console.ReadLine();
        usertypedQKeyword = this.endGameSessionAndQuitFromGame(playerChoice);
        while (!this.userMovementValidation(ref row, ref column, playerChoice) && !usertypedQKeyword)
        {
            if (isPlayerSquareOnBoardChoiceInValidFormat(playerChoice))
            {
                Console.WriteLine("You have typed illegal input. Please try again.");
            }
            else
            {
                Console.WriteLine("You have typed illegal format input. Please try again.");
            }

            playerChoice = Console.ReadLine();
            usertypedQKeyword = this.endGameSessionAndQuitFromGame(playerChoice);
        }

        if (!usertypedQKeyword)
        {
            s_TheGame.AddTokenFromUserToBoard(row, column);
            s_TheGame.AddWonTokensToBoard(row, column);
        }

        return usertypedQKeyword;
    }

    private bool userMovementValidation(ref byte io_Row, ref byte io_Column, string i_PlayerCoice)
    {
        bool validMove = false;

        if (this.isPlayerSquareOnBoardChoiceInValidFormat(i_PlayerCoice))
        {
            io_Column = (byte)(Convert.ToByte(i_PlayerCoice[0]) - 65);
            io_Row = (byte)(Convert.ToByte(i_PlayerCoice[1]) - 49);
            if (s_TheGame.IsExistingSquareOnBoard(io_Row, io_Column))
            {
                if (s_TheGame.IsLegalMovement(io_Row, io_Column))
                {
                    validMove = true;
                }
            }
        }

        return validMove;
    }

    private bool isPlayerSquareOnBoardChoiceInValidFormat(string i_PlayerChoice)
    {
        bool isValid = false;

        if (i_PlayerChoice != null && i_PlayerChoice.Length == 2)
        {
            isValid = char.IsLetter(i_PlayerChoice[0]) && char.IsNumber(i_PlayerChoice[1]);
        }

        return isValid;
    }

    private void playersPoints()
    {
        s_TheGame.CountingPlayersTokens();
        Console.WriteLine("{0}Score:{0}White Tokens: {1}{0}Black Tokens: {2}",
            Environment.NewLine,
            s_TheGame.GetPlayerScore(OthelloGame.eGameToken.WhiteToken),
            s_TheGame.GetPlayerScore(OthelloGame.eGameToken.BlackToken));
    }

    private void winnerDeclaration()
    {
        string winner = s_TheGame.WinnerDecision();

        Console.WriteLine("{0}The game is over.", Environment.NewLine);
        Thread.Sleep(2000);
        if (winner == string.Empty)
        {
            Console.WriteLine("{0}It's a tie !{0}", Environment.NewLine);
        }
        else
        {
            Console.WriteLine("{0}{1} won the game!{0}", Environment.NewLine, winner);
        }

        Thread.Sleep(7000);
        Screen.Clear();
    }

    private void playAnotherGame()
    {
        s_TheGame.CleanOpponents();
        this.userChoiceFromMainMenu();
    }

    private bool endGameSessionAndQuitFromGame(string i_StrFromUser)
    {
        bool endGame = false;

        if (i_StrFromUser.Equals("Q"))
        {
            endGame = true;
        }

        return endGame;
    }
}