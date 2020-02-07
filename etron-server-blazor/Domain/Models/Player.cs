using System;
using System.Collections.Generic;
using EtronServer.Domain.Exceptions;

namespace EtronServer.Domain.Models
{
    public class Player
    {
        private Stack<Direction> _moveStack;

        public Player(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} is null or empty.", nameof(name));
            }

            Name = name;
            _moveStack = new Stack<Direction>();
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Direction Direction { get; private set; }

        public void SetDirection(Direction direction)
        {
            if (Direction == Direction.None)
                throw new PlayerInvalidDirectionException($"Invalid direction {direction}");

            Direction = direction;
            _moveStack.Push(Direction);
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal void Move(int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }

    public enum Direction
    {
        None = 0,
        Left = 1,
        Up = 2,
        Right = 3,
        Down = 4
    }
}