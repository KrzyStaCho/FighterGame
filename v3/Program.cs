using System.Globalization;
using FighterGame.Artifacts;
using FighterGame.Resources;
using FighterGame.Resources;

namespace FighterGame;

public static class Program
{
    private static readonly string[] AppExistsLangArray = new[] { "en", "pl" };
    private static readonly string EnemyCharacterName = "Enemy";
    private static Character Player;
    private static Character Enemy;

    /// <summary>
    /// Core of all App System
    /// </summary>
    static void Main()
    {
        while (!AppPrepare()) {}
        while (!AppCreateCharacters()) {}

        bool isPlayerWin = AppDoFight();
    }
    
    /// <summary>
    /// This method set Console's Title and Application Language
    /// </summary>
    /// <returns>Status of Resource's Culture</returns>
    static bool AppPrepare()
    {
        Console.Title = LangResource.ApplicationTitle;
        
        Console.Write(LangResource.AppPrepareChoose + @"[EN/PL]: ");
        string? userInput = Console.ReadLine();
        
        if (String.IsNullOrEmpty(userInput))
        {
            Console.WriteLine(LangResource.EmptyUserInput);
            return false;
        }

        userInput = userInput.ToLower();
        if (Array.Find(AppExistsLangArray, s => (s == userInput)) == null)
        {
            Console.WriteLine(LangResource.AppPrepareFalseLang);
            return false;
        }

        LangResource.Culture = new CultureInfo(userInput);
        Console.WriteLine(LangResource.ApplicationTitle);
        Console.WriteLine(LangResource.ApplicationReleaseDate);
        Console.WriteLine(LangResource.AppBreaker);
        return true;
    }

    /// <summary>
    /// Create 2 Character's Objects, and get data from user to save as attributes
    /// </summary>
    /// <returns>Status of Function</returns>
    static bool AppCreateCharacters()
    {
        // -->CREATE PLAYER CHARACTER<--
        
        Console.WriteLine(LangResource.CharacterCreatorTitle);

        int remainingUpgradePoints = Character.CharacterMaxUpgrade;

        //Name
        string characterName = String.Empty;
        while (String.IsNullOrEmpty(characterName))
        {
            //GetName
            Console.Write(LangResource.CharacterCreatorGetName);
            string? userInput = Console.ReadLine();
            
            //Name's Validation
            if (String.IsNullOrEmpty(userInput))
            {
                Console.WriteLine(LangResource.EmptyUserInput);
                continue;
            }

            characterName = userInput;
        }
        
        //HealthPoints
        int healthPoints = GetUserAttributePoints(LangResource.CharacterAttributeHealth, remainingUpgradePoints);
        remainingUpgradePoints -= healthPoints;
        
        //ArmorPoints
        int armorPoints = GetUserAttributePoints(LangResource.CharacterAttributeArmor, remainingUpgradePoints);
        remainingUpgradePoints -= armorPoints;
        
        //AttackDamagePoints
        int attackDamagePoints =
            GetUserAttributePoints(LangResource.CharacterAttributeAttackDamage, remainingUpgradePoints);
        remainingUpgradePoints -= attackDamagePoints;
        
        //CritRatePoints
        int critRatePoints = GetUserAttributePoints(LangResource.CharacterAttributeCritRate, remainingUpgradePoints);
        remainingUpgradePoints -= critRatePoints;
        
        //AttackSpeedPoints
        int attackSpeedPoints =
            GetUserAttributePoints(LangResource.CharacterAttributeAttackSpeed, remainingUpgradePoints);

        Console.WriteLine(LangResource.CharacterDrawArtifactTitle);
        
        //Getting chance of get artifact
        Artifact characterArtifact;
        Random rnd = new Random();
        int characterChance = rnd.Next(100) + 1; // 1..100
        
        //Draw an Artifact
        if (characterChance <= Artifact.ChanceOfGettingArtifact)
        {
            Artifact[] artifactTable = { new ArtifactDarkBlade(), new ArtifactHeroShield() };
            int index = rnd.Next(artifactTable.Length);

            characterArtifact = artifactTable[index];
            string artifactName = characterArtifact.GetType().Name;
            Console.WriteLine(LangResource.CharacterDrawArtifactResult, artifactName);
        }
        else
        {
            Console.WriteLine(LangResource.CharacterDrawArtifactFail);
            characterArtifact = new ArtifactNone();
        }
        
        //Create Character's Object
        Player = new Character(characterName, healthPoints, armorPoints, attackDamagePoints, critRatePoints,
            attackSpeedPoints, characterArtifact);

        Console.WriteLine(LangResource.CharacterCreatorPlayerResult);
        
        // -->CREATE ENEMY CHARACTER<--
        Console.WriteLine(LangResource.CharacterCreatorEnemyPreparation);
        
        Enemy = CreateEnemyPlayer(Character.CharacterMaxUpgrade);

        Console.WriteLine(LangResource.CharacterCreatorEnemyFinalText);

        //Print characters attributes
        PrintCharacter(Player);
        PrintCharacter(Enemy);

        return true;
    }

    /// <summary>
    /// Simulate fight between characters
    /// </summary>
    /// <returns>is Player win a duel</returns>
    static bool AppDoFight()
    {
        //Preparation Variables
        bool statusOfGame = true; //True = InGame; False = Game Over
        double PlayerTmpSpeed = 0;
        double EnemyTmpSpeed = 0;
        bool isPlayerWin = false;

        //Round of fight
        while (statusOfGame)
        {
            PlayerTmpSpeed += Player.AttackSpeed;
            EnemyTmpSpeed += Enemy.AttackSpeed;

            //Player Move
            while (PlayerTmpSpeed >= 1.0)
            {
                PlayerTmpSpeed -= 1.0;
                int playerDamage = Enemy.DealDamageCharacter(Player.GetCharacterAttackDamage());
                Console.WriteLine(LangResource.CharacterFightSchemat, Player.Name, playerDamage, Enemy.Name);

                if (Enemy.Health == 0)
                {
                    Console.WriteLine(LangResource.CharacterFightResult, Player.Name);
                    isPlayerWin = true;
                    statusOfGame = false;
                    break;
                }
            }
            
            //Enemy Move
            while (EnemyTmpSpeed >= 1.0 && statusOfGame)
            {
                EnemyTmpSpeed -= 1.0;
                int enemyDamage = Player.DealDamageCharacter(Enemy.GetCharacterAttackDamage());
                Console.WriteLine(LangResource.CharacterFightSchemat, Enemy.Name, enemyDamage, Player.Name);

                if (Player.Health == 0)
                {
                    Console.WriteLine(LangResource.CharacterFightResult, Enemy.Name);
                    isPlayerWin = false;
                    statusOfGame = false;
                }
            }
        }

        return isPlayerWin;
    }

    /// <summary>
    /// Simple get integer from user and simple validation
    /// </summary>
    /// <param name="characterAttribute">Attribute Name</param>
    /// <param name="remainingUpgradePoints">Remaining Upgrade Points</param>
    /// <returns>Integer when number is correct, -1 is not correct</returns>
    static int GetUserAttributePoints(string characterAttribute, int remainingUpgradePoints)
    {
        int userPoints = -1;
        while (userPoints == -1)
        {
            //GetPoints
            Console.Write(LangResource.CharacterCreatorGetPoints, characterAttribute, remainingUpgradePoints, Character.CharacterMaxUpgrade);
            string? userInput = Console.ReadLine();
            
            //Validation
            if (String.IsNullOrEmpty(userInput))
            {
                Console.WriteLine(LangResource.EmptyUserInput);
                continue;
            }
            
            if (!int.TryParse(userInput, out userPoints))
            {
                Console.WriteLine(LangResource.InvalidNumberInput);
                userPoints = -1;
                continue;
            }
            
            if (userPoints < 0 || userPoints > remainingUpgradePoints)
            {
                Console.WriteLine(LangResource.CharacterCreatorInvalidRangePoints);
                userPoints = -1;
            }
        }

        return userPoints;
    }

    /// <summary>
    /// Randomize assign points to attribute and create Character's Object
    /// </summary>
    /// <param name="maxUpgradePoints">Max points for attributes</param>
    /// <returns>Character's Object</returns>
    static Character CreateEnemyPlayer(int maxUpgradePoints)
    {
        int remainingPoints = maxUpgradePoints;
        string name = EnemyCharacterName;

        Random rnd = new Random();

        //HealthPoints
        int healthPoints = (remainingPoints == 0) ? 0 : rnd.Next(remainingPoints + 1);
        remainingPoints -= healthPoints;

        //ArmorPoints
        int armorPoints = (remainingPoints == 0) ? 0 : rnd.Next(remainingPoints + 1);
        remainingPoints -= armorPoints;

        //AttackDamagePoints
        int attackDamagePoints = (remainingPoints == 0) ? 0 : rnd.Next(remainingPoints + 1);
        remainingPoints -= attackDamagePoints;

        //CritRatePoints
        int critRatePoints = (remainingPoints == 0) ? 0 : rnd.Next(remainingPoints + 1);
        remainingPoints -= critRatePoints;

        //AttackSpeedPoints
        int attackSpeedPoints = remainingPoints;

        Character character = new Character(name, healthPoints, armorPoints, attackDamagePoints, critRatePoints,
            attackSpeedPoints, new ArtifactNone());

        return character;
    }

    static void PrintCharacter(Character character)
    {
        Console.WriteLine(LangResource.CharacterPrintTitle, character.Name);
        Console.WriteLine(LangResource.CharacterPrintSchemat, LangResource.CharacterAttributeHealth, character.Health);
        Console.WriteLine(LangResource.CharacterPrintSchemat, LangResource.CharacterAttributeArmor, character.Armor);
        Console.WriteLine(LangResource.CharacterPrintSchemat, LangResource.CharacterAttributeAttackDamage, character.AttackDamage);
        Console.WriteLine(LangResource.CharacterPrintSchemat, LangResource.CharacterAttributeCritRate, character.CritRate);
        Console.WriteLine(LangResource.CharacterPrintSchemat, LangResource.CharacterAttributeAttackSpeed, character.AttackSpeed);
        Console.WriteLine(LangResource.CharacterPrintSchemat, LangResource.CharacterArtifactName, character.CharacterArtifact.GetType().Name);
        Console.WriteLine();
    }
}
