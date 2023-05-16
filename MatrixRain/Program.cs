using System.Text;

namespace MatrixRain
{
    internal class Program
    {
        /// <summary>
        /// Matrix Rain - console application, where the code falls on the screen like in the movie matrix
        /// </summary>
        /// <param name="directionUp">Change of direction of code fall</param>
        /// <param name="color" >Color of the code - Black, Blue ,Cyan ,DarkBIue ,DarkCyan ,DarkGray ,DarkGreen ,DarkMagenta 
        /// DarkRed, DarkYeIIow, Gray, Green, Magenta, Red, White, Yellow</param>
        /// <param name="delaySpeed">Falling code delay speed in miliseconds</param>
        /// <param name="characters">Set of characters to be displayed by the applicationL - Alpha, AlphaNumeric, Numeric </param>
        /// <param name="maxNewSpaces">The maximum parameter+1 of spaces that can be generated on the first line</param>
        static void Main(bool directionUp = false, string color = "Green", int delaySpeed = 1, string characters = "AlphaNumeric",int maxNewSpaces = 10)
        {
            if (!Enum.TryParse(char.ToUpper(color[0]) + color[1..].ToLower(), out ConsoleColor colour))
            {
                Console.Error.WriteLine("Invalid color");
                return;
            }
            if (delaySpeed < 0)
            {
                Console.Error.WriteLine("Invalid delay-speed");
                return;
            }
            characters = characters.ToUpper();
            if (!(characters.Equals("ALPHA") || characters.Equals("ALPHANUMERIC") || characters.Equals("NUMERIC")))
            {
                Console.Error.WriteLine("Ivalid character set");
                return;
            }
            if (maxNewSpaces < 1)
            {
                Console.Error.WriteLine("Ivalid number of spaces");
                return;
            }
            var builder = new StringBuilder();

            if (characters.Equals("NUMERIC")) for (char c = '0'; c <= '9'; c++) builder.Append(c);
            if (characters.Equals("ALPHA")) for (char c = 'A'; c <= 'Z'; c++) builder.Append(c);
            if (characters.Equals("ALPHANUMERIC")) for (char c = '0'; c <= 'Z'; c++) builder.Append(c);
            string charactersSet = builder.ToString();
            Console.Clear();
            Console.CursorVisible = false;
            Random random = new();
            Code[,] codes = new Code[Console.WindowHeight, Console.WindowWidth];


            int firstRow = directionUp ? Console.WindowHeight-1 : 0;
            //initializing the field, selecting the first element that will be the lead, the rest are ' ' character
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                for (int j = 0, n = random.Next(Console.WindowWidth); j < Console.WindowWidth; j++)
                {
                    if (i == firstRow && j == n)
                        codes[i, j] = new Code(true, charactersSet[random.Next(charactersSet.Length)]);
                    else
                        codes[i, j] = new Code(false, ' ');
                }
            }

            while (!Console.KeyAvailable)
            {
                for (int i = 0; i < Console.WindowHeight; i++)
                {
                    for (int j = 0; j < Console.WindowWidth; j++)
                    {
                        if (codes[i, j].C != ' ')
                        {
                            // if the character is not ' ' then change the character and print in white if it is lead
                            codes[i, j].C = charactersSet[random.Next(charactersSet.Length)];
                            if (!codes[i, j].Leading)
                                Console.ForegroundColor = colour;
                            else
                                Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(j, i);
                            Console.Write(codes[i, j].C);
                        }
                        else
                        {
                            //if the character is ' ' just write it out
                            Console.SetCursorPosition(j, i);
                            Console.Write(codes[i, j].C);
                        }
                    }
                }

                int increment = directionUp ? 1 : -1;
                int start = directionUp ? 0 : Console.WindowHeight - 1;
                int end = directionUp ? Console.WindowHeight - 1 : 0;

                    for (int i = start; i != end; i+= increment)
                    {
                        for (int j = 0; j < Console.WindowWidth; j++)
                        {
                            //movement on the console(array)
                            codes[i, j].Leading = codes[i + increment, j].Leading;
                            codes[i, j].C = codes[i + increment, j].C;

                            codes[i + increment, j].Leading = false;

                        }
                    }




                int newPosition = random.Next(Console.WindowWidth);
                while (codes[Math.Abs(firstRow - 1), newPosition].Leading)
                {
                    newPosition = random.Next(Console.WindowWidth); //security, if there are multiple leads in one column,
                                                                    //at least 2 characters long each
                }

                int numNewSpaces = random.Next(Console.WindowWidth) % maxNewSpaces + 1; // random number % parameter + 1 to generate spaces
                for (int i = 0; i < numNewSpaces; i++)
                {
                    int spacePosition = random.Next(Console.WindowWidth);
                    if (codes[Math.Abs(firstRow - 2), spacePosition].C == ' ' && codes[Math.Abs(firstRow - 1), spacePosition].C != ' ')
                    {
                        i--;
                        continue; //to ensure that a character does not appear between two spaces
                    }
                    else
                    {
                        codes[firstRow, spacePosition].C = ' ';
                    }
                }
                //novy veduci prvok
                Console.SetCursorPosition(newPosition, firstRow);
                Console.Write(codes[firstRow, newPosition].C);
                codes[firstRow, newPosition].C = charactersSet[random.Next(charactersSet.Length)];
                codes[firstRow, newPosition].Leading = true;

                Thread.Sleep(delaySpeed);




            }
            Console.Clear();
            Console.ResetColor();
            Console.CursorVisible = true;
            return;

        }

        class Code
        {
            public Code(bool leading, char c)
            {
                Leading = leading;
                C = c;
            }
            public bool Leading { get; set; }
            public char C { get; set; }
        }
    }
}
