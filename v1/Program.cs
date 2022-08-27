namespace FighterGame;

static class Program
{
    private static Character Player1;
    private static Character Player2;
    
    
    static void Main(string[] args)
    {
        GuiManager.PrintTitle();
        
        Player1 = CreateCharacters();
        Player2 = CreateCharacters();

        string winnerName = (Fight() == 1) ? Player1.Name : Player2.Name;
        GuiManager.ShowResult(winnerName);
    }

    static Character CreateCharacters()
    {
        string name = "";
        int maxHealth = 0, attackDamage = 0;
        double attackSpeed = 0.0;

        bool isCorrect = false;
        while (!isCorrect)
        {
            List<string?> nullableAttributes = GuiManager.GetAttributes();
            List<string> attributes = new List<string>();

            isCorrect = true;
            foreach (var variable in nullableAttributes)
            {
                if (variable == null)
                {
                    isCorrect = false;
                    break;
                }
                attributes.Add(variable);
            }
            if (!isCorrect)
            {
                GuiManager.ShowDescriptionOfIncorrectData();
                continue;
            }

            name = attributes[0];
            if (!int.TryParse(attributes[1], out maxHealth))
            {
                isCorrect = false;
                GuiManager.ShowDescriptionOfIncorrectData();
                continue;
            }

            if (!int.TryParse(attributes[2], out attackDamage))
            {
                isCorrect = false;
                GuiManager.ShowDescriptionOfIncorrectData();
                continue;
            }

            if (!double.TryParse(attributes[3], out attackSpeed))
            {
                isCorrect = false;
                GuiManager.ShowDescriptionOfIncorrectData();
                continue;
            }
        }

        Character character = new Character(name, maxHealth, attackDamage, attackSpeed);
        GuiManager.CreateCharacterSuccess();
        return character;
    }

    static int Fight()
    {
        double tmpSpeedPlayer1 = 0.0;
        double tmpSpeedPlayer2 = 0.0;
        bool isFightEnd = false;

        while (!isFightEnd)
        {
            tmpSpeedPlayer1 += Player1.AttackSpeed;
            tmpSpeedPlayer2 += Player2.AttackSpeed;

            if (tmpSpeedPlayer1 >= 1.0)
            {
                tmpSpeedPlayer1 -= 1.0;
                isFightEnd = DealDmg(Player1, ref Player2);
            }
            
            if (tmpSpeedPlayer2 >= 1.0)
            {
                tmpSpeedPlayer2 -= 1.0;
                isFightEnd = DealDmg(Player2, ref Player1);
            }
        }

        if (Player1.Health <= 0.0) return 1;
        return 2;
    }

    static bool DealDmg(Character attackPlayer, ref Character defPlayer)
    {
        defPlayer.Health -= attackPlayer.AttackDamage;
        GuiManager.PrintDealDmg(attackPlayer.Name, attackPlayer.AttackDamage, defPlayer.Name);
        return (defPlayer.Health <= 0.0);
    }
}
