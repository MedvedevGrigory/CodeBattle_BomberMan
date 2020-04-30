/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2018 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Bomberman.Api;

namespace Demo
{
	/// <summary>
	/// This is BombermanAI client demo.
	/// </summary>
	internal class YourSolver : AbstractSolver
	{
        private static Direction _lastMove;

        public YourSolver(string server)
			: base(server)
		{
		}

		/// <summary>
		/// Calls each move to make decision what to do (next move)
		/// </summary>
		protected override string Get(Board board)
        {
            try
            {
                return Move(board);

            }
            catch (Exception e)
            {
                return Direction.Stop.ToString();
            }
        }

        private string Move(Board board)
        {
            int[,] map = GetNextCells(board);

            string move = GetMove(map);

            return move;
        }

        private string GetMove(int[,] map)
        {
            string move = string.Empty;

            List<Direction> availableDirections = new List<Direction> {Direction.Left, Direction.Right, Direction.Up, Direction.Down};

            if (map[3, 4] != 2 ||
                IsManyBombsOrEnemyOnLeft(map))
            {
                availableDirections.Remove(Direction.Left);
            }

            if (map[5, 4] != 2 ||
                IsManyBombsOrEnemyOnRight(map))
            {
                availableDirections.Remove(Direction.Right);
            }

            if (map[4, 3] != 2 ||
                IsManyBombsOrEnemyOnDown(map))
            {
                availableDirections.Remove(Direction.Down);
            }

            if (map[4, 5] != 2 ||
                IsManyBombsOrEnemyOnUp(map))
            {
                availableDirections.Remove(Direction.Up);
            }

            var random = new Random();

            var direct = availableDirections.Any() ? availableDirections[random.Next(availableDirections.Count)] : Direction.Stop;

            bool isPutBomb = PutBomb(map);

            if (isPutBomb)
            {
                move = Direction.Act.ToString();
            }

            for (int i = 0; i < availableDirections.Count; i++)
            {
                if (availableDirections[i] == _lastMove && _lastMove != Direction.Stop && random.Next(100) < (isPutBomb ? 40 : 80))
                {
                    direct = _lastMove;
                }
            }

            _lastMove = direct;

            move += direct;

            return move;
        }

        private bool IsManyBombsOrEnemyOnLeft(int[,] map)
        {
            var countBombs = 0;
            var countEnemy = 0;

            for (int x = 2; x <= 3; x++)
            {
                for (int y = 3; y <= 5; y++)
                {
                    if (map[x, y] == -1 ||
                        map[x, y] == -2)
                    {
                        countBombs++;
                    }

                    if (map[x, y] == 1)
                    {
                        countEnemy++;
                    }
                }
            }

            return countBombs >= 2 || countEnemy >= 2;
        }

        private bool IsManyBombsOrEnemyOnRight(int[,] map)
        {
            var countBombs = 0;
            var countEnemy = 0;

            for (int x = 5; x <= 6; x++)
            {
                for (int y = 3; y <= 5; y++)
                {
                    if (map[x, y] == -1 ||
                        map[x, y] == -2)
                    {
                        countBombs++;
                    }

                    if (map[x, y] == 1)
                    {
                        countEnemy++;
                    }
                }
            }

            return countBombs >= 2 || countEnemy >= 2;
        }

        private bool IsManyBombsOrEnemyOnDown(int[,] map)
        {
            var countBombs = 0;
            var countEnemy = 0;

            for (int x = 3; x <= 5; x++)
            {
                for (int y = 2; y <= 3; y++)
                {
                    if (map[x, y] == -1 ||
                        map[x, y] == -2)
                    {
                        countBombs++;
                    }

                    if (map[x, y] == 1)
                    {
                        countEnemy++;
                    }
                }
            }

            return countBombs >= 2 || countEnemy >= 2;
        }

        private bool IsManyBombsOrEnemyOnUp(int[,] map)
        {
            var countBombs = 0;
            var countEnemy = 0;

            for (int x = 3; x <= 5; x++)
            {
                for (int y = 5; y <= 6; y++)
                {
                    if (map[x, y] == -1 ||
                        map[x, y] == -2)
                    {
                        countBombs++;
                    }

                    if (map[x, y] == 1)
                    {
                        countEnemy++;
                    }
                }
            }

            return countBombs >= 2 || countEnemy >= 2;
        }

        private bool PutBomb(int[,] map)
        {
            if (map[4, 4] != 3)
            {
                if (CheckDiagonal(map)) return true;

                if (CheckLeft(map)) return true;

                if (CheckRight(map)) return true;

                if (CheckDown(map)) return true;

                if (CheckUp(map)) return true;
            }

            return false;
        }

        private bool CheckUp(int[,] map)
        {
            for (int y = 5; y < 8; y++)
            {
                if (map[4, y] == 1 ||
                    map[4, y] == 4)
                {
                    if (!CheckUpWall(map, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckDown(int[,] map)
        {
            for (int y = 1; y < 4; y++)
            {
                if (map[4, y] == 1 ||
                    map[4, y] == 4)
                {
                    if (!CheckDownWall(map, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckRight(int[,] map)
        {
            for (int x = 5; x < 8; x++)
            {
                if (map[x, 4] == 1 ||
                    map[x, 4] == 4)
                {
                    if (!CheckRightWall(map, x))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool CheckLeft(int[,] map)
        {
            for (int x = 1; x < 4; x++)
            {
                if (map[x, 4] == 1 ||
                    map[x, 4] == 4)
                {
                    if (!CheckLeftWall(map, x))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool CheckDiagonal(int[,] map)
        {
            for (int x = 3; x <= 5; x++)
            {
                for (int y = 3; y <= 5; y++)
                {
                    if (map[x, y] == 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckDownWall(int[,] map, int y)
        {
            for (int dy = y + 1; dy < 4; dy++)
            {
                if (map[4, dy] == 0 ||
                    map[4, dy] == -2)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckUpWall(int[,] map, int y)
        {
            for (int dy = 5; dy < y; dy++)
            {
                if (map[4, dy] == 0 ||
                    map[4, dy] == -2)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckRightWall(int[,] map, int x)
        {
            for (int dx = 5; dx < x; dx++)
            {
                if (map[dx, 4] == 0 ||
                    map[dx, 4] == -2)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckLeftWall(int[,] map, int x)
        {
            for (int dx = x + 1; dx < 4; dx++)
            {
                if (map[dx, 4] == 0 ||
                    map[dx, 4] == -2)
                {
                    return true;
                }
            }

            return false;
        }

        private int[,] GetNextCells(Board board)
        {
            var map = new int[9,9];

            int dx = board.GetBomberman().X - 4;
            int dy = board.GetBomberman().Y - 4;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board[dx + x, dy + y] == Element.OTHER_BOMBERMAN ||
                        board[dx + x, dy + y] == Element.MEAT_CHOPPER)
                    {
                        map[x, y] = 1;
                    }

                    if (board[dx + x, dy + y] == Element.Space)
                    {
                        map[x, y] = 2;
                    }

                    if (board[dx + x, dy + y] == Element.DESTROYABLE_WALL)
                    {
                        map[x, y] = 4;
                    }

                    if (board[dx + x, dy + y] == Element.BOMB_TIMER_1)
                    {
                        map[x, y] = -1;
                    }

                    if (board[dx + x, dy + y] >= Element.BOMB_TIMER_2 && 
                        board[dx + x, dy + y] <= Element.BOMB_TIMER_5 || 
                        board[dx + x, dy + y] == Element.BOMB_BOMBERMAN)
                    {
                        map[x, y] = -2;
                    }
                }
            }

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (map[x,y] == -1)
                    {
                        Boom(map, x, y);
                    }
                }
            }

            return map;
        }


        private void Boom(int[,] map, int dx, int dy)
        {
            if (map[dx, dy] == 3)
            {
                return;
            }
            
            map[dx, dy] = 3;

            for (int x = -1; x > -4; x--)
            {
                if (!ProcessBoom(map, dx + x, dy))
                {
                    break;
                }

                map[dx + x, dy] = 3;
            }

            for (int x = 1; x < 4; x++)
            {
                if (!ProcessBoom(map, dx + x, dy))
                {
                    break;
                }

                map[dx + x, dy] = 3;
            }

            for (int y = -1; y > -4; y--)
            {
                if (!ProcessBoom(map, dx, dy + y))
                {
                    break;
                }

                map[dx, dy + y] = 3;
            }

            for (int y = 1; y < 4; y++)
            {
                if (!ProcessBoom(map, dx, dy + y))
                {
                    break;
                }

                map[dx, dy + y] = 3;
            }
        }

        private bool ProcessBoom(int[,] map, int x, int y)
        {
            if (x < 0 || y < 0 || x >= 9 || y >= 9)
            {
                return false;
            }

            if (map[x, y] == -2)
            {
                Boom(map, x, y);
            }

            return map[x, y] == 2 || map[x, y] == map[4,4];
        }
    }
}