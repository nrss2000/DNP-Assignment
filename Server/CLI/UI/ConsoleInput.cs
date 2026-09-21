namespace CLI.UI;

public static class ConsoleInput
{
    public static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }
            Console.WriteLine("Value cannot be empty.");
        }
    }

    // Returns null when the user just presses enter (used for optional input).
    public static string? ReadOptional(string prompt)
    {
        Console.Write($"{prompt}: ");
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }
            Console.WriteLine("Please enter a valid number.");
        }
    }

    public static int? ReadOptionalInt(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            if (int.TryParse(input, out int value))
            {
                return value;
            }
            Console.WriteLine("Please enter a valid number, or leave empty.");
        }
    }
}
