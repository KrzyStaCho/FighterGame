namespace FighterGame;

public static class Program
{
    private static Character Player;
    private static Character Enemy;
    private static string Lang = "ENG";
    private static string[] TextFile;

    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Incorrect number of arguments");
            return;
        }

        if (args.Length == 1)
        {
            Lang = args[0];
        }

        if (!LoadLangFile())
        {
            Console.WriteLine("Failed to load language file. File can be dont exists or be empty.");
            return;
        }
        
        //Print Title and Release Date of program
        Console.WriteLine("====================");
        Console.WriteLine(TextFile[(int)GuiDescription.Title]);
        Console.WriteLine(TextFile[(int)GuiDescription.ReleaseDate]);
        Console.WriteLine("====================");
        
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateTitle]);
        
        //Create Character
        Character? potentialCharacter;
        do
        {
            potentialCharacter = GuiCreateCharacter();

            if (potentialCharacter != null)
            {
                Console.WriteLine(TextFile[(int)GuiDescription.CharacterPrintTitle]);
                ShowCharacter(potentialCharacter);
                string answer = GetAnswer(TextFile[(int)GuiDescription.CharacterAcceptText], TextFile[(int)GuiDescription.CharacterAcceptInvalidAnswer]);

                if (answer == "n")
                {
                    potentialCharacter = null;
                }
            }
        } while (potentialCharacter == null);
        
        Player = potentialCharacter;
        
        //Create Enemy Character
        Console.WriteLine(TextFile[(int)GuiDescription.EnemyCharacterCreateTitle]);
        Enemy = CreateCharacter(Character.MaxUpgradeBaseAttributes);
        ShowCharacter(Enemy);

        //Fight
        bool isEnd = false;
        bool playerWin = false;
        double tmpSpeedPlayer = 0;
        double tmpSpeedEnemy = 0;
        while (!isEnd)
        {
            tmpSpeedPlayer += Player.AttackSpeed;
            tmpSpeedEnemy += Enemy.AttackSpeed;

            //Player action
            while (tmpSpeedPlayer >= 1.0)
            {
                tmpSpeedPlayer -= 1.0;
                Character.DoMove(Player, Enemy, TextFile[(int)GuiDescription.CharacterMove]);
                if (Enemy.Health <= 0)
                {
                    isEnd = true;
                    playerWin = true;
                    break;
                }
            }
            
            //Enemy action
            while (tmpSpeedEnemy >= 1.0 && !isEnd)
            {
                tmpSpeedEnemy -= 1.0;
                Character.DoMove(Enemy, Player, TextFile[(int)GuiDescription.CharacterMove]);
                if (Player.Health <= 0)
                {
                    isEnd = true;
                    playerWin = false;
                    break;
                }
            }
        }

        //Result
        if (playerWin)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterWin], Player.Name);
            Console.WriteLine("---------------------------");
            return;
        }

        Console.WriteLine("---------------------------");
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterWin], Enemy.Name);
        Console.WriteLine("---------------------------");
    }

    /// <summary>
    /// Loading line from language file.
    /// </summary>
    /// <returns>Status of loading lines</returns>
    static bool LoadLangFile()
    {
        string fileName = $"Lang{Lang}.KApp";

        if (!File.Exists(fileName)) return false;

        TextFile = File.ReadAllLines(fileName);

        if (TextFile.Length == 0) return false;
        return true;
    }

    /// <summary>
    /// Get Attributes data from user and create Character's Object
    /// </summary>
    /// <returns>Character's Object or null when user give incorrect data</returns>
    static Character? GuiCreateCharacter()
    {
        int remainingPoints = Character.MaxUpgradeBaseAttributes;

        //GetName
        Console.Write(TextFile[(int)GuiDescription.CharacterCreateGetName]);
        string? potentialName = Console.ReadLine();

        //Checking if name is correct
        if (String.IsNullOrEmpty(potentialName))
        {
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateInvalidName]);
            return null;
        }
        string name = potentialName;
        int? potentialPoints;
        int healthPoints;
        int attackDamagePoints;
        int critRatePoints;
        int attackSpeedPoints;

        //Health
        if (remainingPoints == 0)
        {
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateNoPointsLeft]);
            healthPoints = 0;
        }
        else
        {
            potentialPoints =
                GuiGetPointsOnAttribute(GuiDescription.CharacterAttributeNameHealth, ref remainingPoints, true);
            if (potentialPoints == null)
            {
                return null;
            }
            healthPoints = (int)potentialPoints;
        }
        
        //Attack Damage
        if (remainingPoints == 0)
        {
            attackDamagePoints = 0;
        }
        else
        {
            potentialPoints =
                GuiGetPointsOnAttribute(GuiDescription.CharacterAttributeNameAttackDamage, ref remainingPoints, true);
            if (potentialPoints == null)
            {
                return null;
            }
            attackDamagePoints = (int)potentialPoints;
        }
        
        //Critical Hit Rate
        if (remainingPoints == 0)
        {
            critRatePoints = 0;
        }
        else
        {
            potentialPoints =
                GuiGetPointsOnAttribute(GuiDescription.CharacterAttributeNameCritRate, ref remainingPoints, true);
            if (potentialPoints == null)
            {
                return null;
            }
            critRatePoints = (int)potentialPoints;
        }
        
        //Attack Speed
        if (remainingPoints == 0)
        {
            attackSpeedPoints = 0;
        }
        else
        {
            potentialPoints =
                GuiGetPointsOnAttribute(GuiDescription.CharacterAttributeNameAttackSpeed, ref remainingPoints, false);
            if (potentialPoints == null)
            {
                return null;
            }
            attackSpeedPoints = (int)potentialPoints;
        }

        return new Character(name, healthPoints, attackDamagePoints, critRatePoints, attackSpeedPoints);
    }
    
    /// <summary>
    /// Get Attribute points and check if answer is correct
    /// </summary>
    /// <param name="attributeNameIndex">TextFile index, where is name of attribute</param>
    /// <param name="maxUserPoints">Points can user give into this attribute</param>
    /// <param name="showMessageOn0Points">Show message when is 0 points</param>
    /// <returns>Points or null when user give incorrect data</returns>
    static int? GuiGetPointsOnAttribute(GuiDescription attributeNameIndex, ref int maxUserPoints, bool showMessageOn0Points)
    {
        Console.Write(TextFile[(int)GuiDescription.CharacterCreateGetPointsOnAttribute], TextFile[(int)attributeNameIndex], maxUserPoints, Character.MaxUpgradeBaseAttributes);
        string? input = Console.ReadLine();
        
        if (input == null)
        {
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateInvalidUserPoints]);
            return null;
        }

        if (!int.TryParse(input, out int userPoints))
        {
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateInvalidUserPoints]);
            return null;
        }

        if (userPoints < 0 || userPoints > maxUserPoints)
        {
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateInvalidUserPoints]);
            return null;
        }

        maxUserPoints -= userPoints;
        
        //Show information about zero points left.
        if (maxUserPoints == 0 && showMessageOn0Points)
        {
            Console.WriteLine(TextFile[(int)GuiDescription.CharacterCreateNoPointsLeft]);
        }

        return userPoints;
    }

    /// <summary>
    /// Show Name and Attributes
    /// </summary>
    /// <param name="character">Character to show</param>
    static void ShowCharacter(Character character)
    {
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterPrintName], character.Name);
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterPrintHealth], character.Health);
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterPrintAttackDamage], character.AttackDamage);
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterPrintCritRate], character.CritRate);
        Console.WriteLine(TextFile[(int)GuiDescription.CharacterPrintAttackSpeed], character.AttackSpeed);
        Console.WriteLine("------------------------------------------");
    }

    /// <summary>
    /// Get Y or N from user. Other answer repeat method.
    /// </summary>
    /// <param name="text">Text of ask</param>
    /// <param name="invalidAnswerText">Text when user give bad answer</param>
    /// <returns>Y or N</returns>
    static string GetAnswer(string text, string invalidAnswerText)
    {
        bool end;
        string userChoice = "n";
        do
        {
            Console.Write(text);
            string? input = Console.ReadLine();
            if (String.IsNullOrEmpty(input))
            {
                Console.WriteLine(invalidAnswerText);
                end = false;
            }
            else
            {
                input = input.ToLower();
                if (input == "n" || input == "y")
                {
                    userChoice = input;
                    end = true;
                }
                else
                {
                    Console.WriteLine(invalidAnswerText);
                    end = false;
                }
            }
        } while (!end);

        return userChoice;
    }

    /// <summary>
    /// Create character with random attributes
    /// </summary>
    /// <param name="maxPoints">max points for attributes</param>
    /// <returns>Character's object</returns>
    static Character CreateCharacter(int maxPoints)
    {
        Random rnd = new Random();

        string name = TextFile[(int)GuiDescription.EnemyCharacterName];
        
        //Health
        int healthPoints = 0;
        if (maxPoints != 0)
        {
            int points = rnd.Next(0, maxPoints);
            healthPoints = points;
            maxPoints -= points;
        }
        
        //Attack Damage
        int attackDamagePoints = 0;
        if (maxPoints != 0)
        {
            int points = rnd.Next(0, maxPoints);
            attackDamagePoints = points;
            maxPoints -= points;
        }
        
        //Crit Rate
        int critRatePoints = 0;
        if (maxPoints != 0)
        {
            int points = rnd.Next(0, maxPoints);
            critRatePoints = points;
            maxPoints -= points;
        }
        
        //Attack Speed
        int attackSpeedPoints = maxPoints;

        return new Character(name, healthPoints, attackDamagePoints, critRatePoints, attackSpeedPoints);
    }
}