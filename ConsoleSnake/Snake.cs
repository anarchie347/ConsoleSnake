﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleSnake
{
    internal class Snake
    {
        //public delegate void SnakeMoveEventHandler(object? sender, SnakeMovedEventArgs args);
        public event EventHandler SnakeMove;

        public const ConsoleColor SNAKE_BODY_COLOUR = ConsoleColor.Blue;
        public const ConsoleColor SNAKE_HEAD_COLOUR = ConsoleColor.DarkBlue;
        public const string FACE_TEXT = "ಠ_ಠ";

        //[0] in list is tail, [count - 1] is head
        public ReadOnlyCollection<Point> Coords { get { return Array.AsReadOnly(coords.ToArray()); } }
        public ReadOnlyCollection<Direction> MoveList { get { return Array.AsReadOnly(moveList.ToArray()); } }

        private List<Point> coords;
        private List<Direction> moveList;

        public int Length { get; private set; }
        public Point BehindSnakeCoords { get; private set; }
        public int MoveDelay { get; private set; }
        public Corner FacePosition { get; private set; }
        public Direction Direction { get; private set; }
 


        private System.Timers.Timer timer;

        public Snake(List<Point> initalSnake, int moveDelay)
        {
            coords = initalSnake;
            moveList = new List<Direction>();
            Length = initalSnake.Count;
            Direction = Direction.Right;
            FacePosition = Corner.TopRight;
            

            for (int i = 0; i < initalSnake.Count - 1; i++)
                moveList.Add(Direction.Right);

            timer = new System.Timers.Timer()
            {
                Interval = moveDelay,
                AutoReset = true,
                Enabled = false
            };
            MoveDelay = moveDelay;
            timer.Elapsed += SnakeTimerTick;
            timer.Start();
        }

        public bool ChangeDirection(Direction newDirection)
        {
            if ((int)Direction == ((int)newDirection + 2) % 4)
            {
                return false;
            }
            Direction = newDirection;
            return true;
        }

        private void SnakeTimerTick(object? sender, ElapsedEventArgs e)
        {
            BehindSnakeCoords = coords[0];
            coords.RemoveAt(0);

            Point currentHeadPos = coords.Last();
            switch (Direction)
            {
                case Direction.Up:
                    coords.Add(new Point(currentHeadPos.X, currentHeadPos.Y - 1));

                    if (FacePosition == Corner.BottomLeft)
                        FacePosition = Corner.TopLeft;
                    else if (FacePosition == Corner.BottomRight)
                        FacePosition = Corner.TopRight;

                    break;
                case Direction.Right:
                    coords.Add(new Point(currentHeadPos.X + 1, currentHeadPos.Y));

                    if (FacePosition == Corner.BottomLeft)
                        FacePosition = Corner.BottomRight;
                    else if (FacePosition == Corner.TopLeft)
                        FacePosition = Corner.TopRight;

                    break;
                case Direction.Down:
                    coords.Add(new Point(currentHeadPos.X, currentHeadPos.Y + 1));

                    if (FacePosition == Corner.TopLeft)
                        FacePosition = Corner.BottomLeft;
                    else if (FacePosition == Corner.TopRight)
                        FacePosition = Corner.BottomRight;

                    break;
                case Direction.Left:
                    coords.Add(new Point(currentHeadPos.X - 1, currentHeadPos.Y));

                    if (FacePosition == Corner.BottomRight)
                        FacePosition = Corner.BottomLeft;
                    else if (FacePosition == Corner.TopRight)
                        FacePosition = Corner.TopLeft;

                    break;
            }


            SnakeMove?.Invoke(this, EventArgs.Empty);
        }
    }

    //internal class SnakeMovedEventArgs : EventArgs
    //{
    //    public List<Point> Coords { get; set; }
    //    public Point BehindSnake { get; set; }
    //    public SnakeMovedEventArgs(List<Point> coords, Point behindSnake)
    //    {
    //        Coords = coords;
    //        BehindSnake = behindSnake;
    //    }
    //}
}