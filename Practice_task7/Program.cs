using System;

namespace Labyrinth
{
    class Program
    {
        static char[,] map;
        static int performerX;
        static int performerY;


        static char[,] ReadMap(string mapName, out int performerX, out int performerY)
        {
            performerX = 0;
            performerY = 0;

            string[] newFile = File.ReadAllLines($"{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '■')
                    {
                        performerX = i;
                        performerY = j;
                    }
                    Console.Write(map[i, j]);
                }
            }

            return map;
        }

        static void DisplayingLabyrinth()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '■')
                    {
                        performerX = i;
                        performerY = j;
                    }

                    Console.Write(map[i, j]);
                }
            }
        }

        static void Movement()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.W)
            {
                map[performerX, performerY] = ' ';
                map[performerX - 1, performerY] = '■';
            }
            else if (key.Key == ConsoleKey.A)
            {
                map[performerX, performerY] = ' ';
                map[performerX, performerY - 1] = '■';
            }
            else if (key.Key == ConsoleKey.S)
            {
                map[performerX, performerY] = ' ';
                map[performerX + 1, performerY] = '■';
            }
            else if (key.Key == ConsoleKey.D)
            {
                map[performerX, performerY] = ' ';
                map[performerX, performerY + 1] = '■';
            }

        }
        static void Main()
        {
            map = ReadMap("level", out performerX, out performerY);
            while (true)
            {
                DisplayingLabyrinth();
                Movement();
            }
        }
    }
}