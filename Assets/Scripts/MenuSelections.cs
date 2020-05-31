public static class MenuSelection
{
  private static int player1Character = 0, player2Character = 0, numEnemies = 5;

  public static int Player1Character
  {
    get
    {
      return player1Character;
    }
    set
    {
      player1Character = value;
    }
  }

  public static int Player2Character
  {
    get
    {
      return player2Character;
    }
    set
    {
      player2Character = value;
    }
  }

  public static int NumEnemies
  {
    get
    {
      return numEnemies;
    }
    set
    {
      numEnemies = value;
    }
  }
}
