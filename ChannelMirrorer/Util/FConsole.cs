namespace ChannelMirrorer.Util;

internal static class FConsole
{
    private static readonly char[] Colors = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F' };

    // & - text color
    // % - background color

    public static void Write(string input)
    {
        var fore = Console.ForegroundColor;
        var back = Console.BackgroundColor;
        if (!input.Contains('&') && !input.Contains('%'))
        {
            Console.Write(input);
            return;
        }

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (c != '&' && c != '%')
            {
                if (!Colors.Contains(c) || input[i - 1] != '&' && input[i - 1] != '%')
                    Console.Write(c);
            }
            else switch (c)
            {
                case '&' when i == input.Length - 1 || !Colors.Contains(input[i + 1]):
                    Console.Write(c);
                    break;
                case '&':
                {
                    if (Colors.Contains(input[i + 1]))
                        Console.ForegroundColor = CharToColor(input[i + 1]);
                    break;
                }
                case '%' when i == input.Length - 1 || !Colors.Contains(input[i + 1]):
                    Console.Write(c);
                    break;
                case '%':
                {
                    if (Colors.Contains(input[i + 1]))
                        Console.BackgroundColor = CharToColor(input[i + 1]);
                    break;
                }
            }
        }
        Console.ForegroundColor = fore;
        Console.BackgroundColor = back;
    }
    public static void WriteLine(string input)
    {
        Write(input + "\n");
    }
    private static ConsoleColor CharToColor(char c)
    {
        return c switch
        {
            '0' => ConsoleColor.Black,
            '1' => ConsoleColor.DarkBlue,
            '2' => ConsoleColor.DarkGreen,
            '3' => ConsoleColor.DarkCyan,
            '4' => ConsoleColor.DarkRed,
            '5' => ConsoleColor.DarkMagenta,
            '6' => ConsoleColor.DarkYellow,
            '7' => ConsoleColor.Gray,
            '8' => ConsoleColor.DarkGray,
            '9' => ConsoleColor.Blue,
            'a' => ConsoleColor.Green,
            'b' => ConsoleColor.Cyan,
            'c' => ConsoleColor.Red,
            'd' => ConsoleColor.Magenta,
            'e' => ConsoleColor.Yellow,
            'f' => ConsoleColor.White,
            _ => ConsoleColor.White
        };
    }
}
