namespace FighterGame;

public static class GuiManager
{
    public static void PrintTitle()
    {
        Console.WriteLine("====================");
        Console.WriteLine("FighterGame v1.0");
        Console.WriteLine("Release Date: 27.08.2022");
        Console.WriteLine("====================");
    }

    public static List<string?> GetAttributes()
    {
        Console.WriteLine("=== Creating Character ===");
        List<string?> attributes = new List<string?>();

        Console.Write("Please insert Name of your character: ");
        attributes.Add(Console.ReadLine());
        
        Console.Write("Please insert Health of your character: ");
        attributes.Add(Console.ReadLine());
        
        Console.Write("Please insert Attack Damage of your character: ");
        attributes.Add(Console.ReadLine());
        
        Console.Write("Please insert Speed Attack of your character: ");
        attributes.Add(Console.ReadLine());

        return attributes;
    }

    public static void ShowDescriptionOfIncorrectData()
    {
        Console.WriteLine("Your data was incorrect, please try again");
    }

    public static void CreateCharacterSuccess()
    {
        Console.WriteLine("=== Your character was created ===");
    }

    public static void PrintDealDmg(string atkPlayerName, int dmg, string defPlayerName)
    {
        Console.WriteLine($"{atkPlayerName} deal {dmg} to {defPlayerName}");
    }

    public static void ShowResult(string victoryPlayerName)
    {
        Console.WriteLine($"=== {victoryPlayerName} win this duel! ===");
    }
}