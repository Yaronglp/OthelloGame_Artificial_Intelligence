﻿﻿﻿using System;

internal class Player
{
    private readonly string r_PlayerName = string.Empty;
    private readonly OthelloGame.eGameToken r_PlayerToken;
    private byte m_PlayerScore;

    internal string PlayerName
    {
        get { return r_PlayerName; }
    }

    internal OthelloGame.eGameToken PlayerToken
    {
        get { return r_PlayerToken; }
    }

    internal byte PlayerScore
    {
        set { m_PlayerScore = value; }

        get { return m_PlayerScore; }
    }

    public Player(string i_PlayerName, OthelloGame.eGameToken i_PlayerToken)
        : this(i_PlayerName, i_PlayerToken, 0)
    { }

    public Player(string i_PlayerName, OthelloGame.eGameToken i_PlayerToken, byte i_PlayerScore)
    {
        r_PlayerName = i_PlayerName;
        r_PlayerToken = i_PlayerToken;
        PlayerScore = i_PlayerScore;
    }
}