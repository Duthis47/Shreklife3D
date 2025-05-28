using UnityEngine;
using JetBrains.Annotations;

class Player
{
    public static Player Instance { get; set; }
    public static string pseudo { get; private set; }
    public static int score { get; private set; } = 0;
    public static int id { get; private set; }

    public static void setIdPlayer(int id)
    {
        if (id > 0)
        {
            Player.id = id;
        }
    }
    public static void setPseudo(string pseudo)
    {
        Player.pseudo = pseudo;
    }

    public static void setScore(int score)
    {
        if (Player.score < score)
        {
            Player.score = score;
        }
        else
        {
            Debug.Log("POOOOUUUUURRQUUUUOI");
        }
    }
}