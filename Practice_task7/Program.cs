using System;

namespace Labyrinth
{
    class Program
    {
        static char[,] map;
        static char[,] mapWay;
        static int performerX;
        static int performerY;
        static int playerHitPoint;
        static int[,] enemies;
        static bool playerNotDie = true;
        static bool isWayShow = false;
        static bool isFinish = false;

        static char[,] ReadMap(string mapName)
        {
            performerX = 0;
            performerY = 0;

            string[] newFile = File.ReadAllLines($"{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '■')
                    {
                        performerX = i;
                        performerY = j;
                    }
                }
            }

            return map;
        }

        static void DisplayingLabyrinth()
        {
            char[,] mapToShow;
            if (isWayShow)
            {
                mapToShow = mapWay;
            }
            else
            {
                mapToShow = map;
            }
            for (int i = 0; i < mapToShow.GetLength(0); i++)
            {
                for (int j = 0; j < mapToShow.GetLength(1); j++)
                {
                    Console.Write(mapToShow[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void MovementPlayer()
        {
            Console.SetCursorPosition(35, 0);
            Console.WriteLine("Игра лаберинт");
            Console.SetCursorPosition(35, 1);
            Console.WriteLine(" 1) Для передвидения используйте 'W' 'A' 'S' 'D'");
            Console.SetCursorPosition(35, 2);
            Console.WriteLine(" 2) Для просмотра пути используйте F1");
            Console.SetCursorPosition(35, 3);
            Console.WriteLine(" 3) Доберитесь до !");
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.W && map[performerX - 1, performerY] == ' ')
            {
                map[performerX, performerY] = ' ';
                map[--performerX, performerY] = '■';
            }
            else if (key.Key == ConsoleKey.A && map[performerX, performerY - 1] == ' ')
            {
                map[performerX, performerY] = ' ';
                map[performerX, --performerY] = '■';
            }
            else if (key.Key == ConsoleKey.S && map[performerX + 1, performerY] == ' ')
            {
                map[performerX, performerY] = ' ';
                map[++performerX, performerY] = '■';
            }
            else if (key.Key == ConsoleKey.D && map[performerX, performerY + 1] == ' ')
            {
                map[performerX, performerY] = ' ';
                map[performerX, ++performerY] = '■';
            }
            else if (key.Key == ConsoleKey.W && map[performerX - 1, performerY] == 'e')
            {
                map[performerX, performerY] = ' ';
                map[--performerX, performerY] = '■';
                playerHitPoint--;
            }
            else if (key.Key == ConsoleKey.A && map[performerX, performerY - 1] == 'e')
            {
                map[performerX, performerY] = ' ';
                map[performerX, --performerY] = '■';
                playerHitPoint--;
            }
            else if (key.Key == ConsoleKey.S && map[performerX + 1, performerY] == 'e')
            {
                map[performerX, performerY] = ' ';
                map[++performerX, performerY] = '■';
                playerHitPoint--;
            }
            else if (key.Key == ConsoleKey.D && map[performerX, performerY + 1] == 'e')
            {
                map[performerX, performerY] = ' ';
                map[performerX, ++performerY] = '■';
                playerHitPoint--;
            }
            else if (key.Key == ConsoleKey.A && map[performerX, performerY - 1] == '!')
            {
                isFinish = true;
                playerNotDie = false;
            }
            if (key.Key == ConsoleKey.F1)
            {
                isWayShow = !isWayShow;
            }
            if (playerHitPoint <= 0)
            {
                playerNotDie = false;
            }
        }

        static void AddEnemies()
        {
            int enemiesCount = 20;
            enemies = new int[enemiesCount, 2];
            Random random = new Random();
            for (int i = 0; i < enemiesCount; i++)
            {
                int horizon = random.Next(2, map.GetLength(0) - 1);
                int vertical = random.Next(2, map.GetLength(1) - 4);
                while (map[vertical, horizon] != ' ')
                {
                    horizon = random.Next(2, map.GetLength(0) - 1);
                    vertical = random.Next(2, map.GetLength(1) - 4);
                }
                enemies[i, 0] = vertical;
                enemies[i, 1] = horizon;
                map[vertical, horizon] = 'e';
            }
        }

        static void MoveEnemies()
        {
            Random random = new Random();
            for (int i = 0; i < enemies.GetLength(0); i++)
            {
                int x = enemies[i, 0];
                int y = enemies[i, 1];
                int[,] moving = new int[4, 2] { { x - 1, y }, { x, y - 1 }, { x + 1, y }, { x, y + 1 } };
                int count = 4;
                int direction = random.Next(0, 4);
                while (map[moving[direction, 0], moving[direction, 1]] != ' ' && count > 0)
                {
                    direction = random.Next(0, 4);
                    count--;
                }
                if (count != 0)
                {
                    if (map[x, y] != '■')
                    {
                        map[x, y] = ' ';
                    }
                    map[moving[direction, 0], moving[direction, 1]] = 'e';
                    enemies[i, 0] = moving[direction, 0];
                    enemies[i, 1] = moving[direction, 1];
                }
            }
        }

        static void ShowBar()
        {
            Console.SetCursorPosition(35, 5);
            Console.WriteLine("Оставшееся здоровье игрока:");
            Console.SetCursorPosition(35, 6);
            Console.Write("  [");
            for (int i = 0; i < playerHitPoint; i++)
            {
                Console.Write("#");
            }
            for (int j = 10 - playerHitPoint; j > 0; j--)
            {
                Console.Write("_");
            }
            Console.Write("]");
        }

        static void Main()
        {
            mapWay = ReadMap("levelWay");
            map = ReadMap("level");
            playerHitPoint = 10;
            AddEnemies();
            Console.SetCursorPosition(0, 0);
            while (playerNotDie)
            {
                DisplayingLabyrinth();
                ShowBar();
                MovementPlayer();
                MoveEnemies();
                Console.SetCursorPosition(0, 0);
            }
            if (isFinish)
            {
                Console.Clear();
                Console.WriteLine("Вы дошли до конца");
                Thread.Sleep(1000);
                return;
            }
            Console.Clear();
            Console.WriteLine("Ваше здоровье закончилось");
            Thread.Sleep(1000);
        }
    }
}