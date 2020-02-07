using System;
using System.Collections.Generic;
using EtronServer.Domain.Exceptions;

namespace EtronServer.Domain.Models
{
    public class Game
    {
        private GameState _state;
        private RaceTrack _raceTrack = new RaceTrack();
        private IList<Player> _players = new List<Player>();

        protected IMessageBus MessageBus { get; }

        public Game(IMessageBus messageBus)
        {
            _state = GameState.New;
            MessageBus = messageBus;
        }

        public void AddPlayer(Player player)
        {
            if (_state != GameState.Awaiting)
                throw new InvalidOperationException($"Unable to add player, invalid state ({_state}).");

            _players.Add(player);
        }

        public void Start()
        {
            if (_state != GameState.New)
                throw new InvalidOperationException($"Unable to start game, invalid state ({_state}).");

            _state = GameState.Started;
            InitializeGame();
        }

        private void InitializeGame()
        {
            foreach (var player in _players)
            {
                _raceTrack.AddPlayer(player);
            }

            SendMessage(new SendMove());
        }

        private void SendMessage(Message message)
        {
            MessageBus.SendMessage(message);
        }
    }

    public class SendMove : Message
    {
        public SendMove() : base(new Guid("eced56ba-771c-44b9-83f5-03ad8ec4c26c"))
        {

        }
    }

    public abstract class Message
    {
        public Message(Guid id)
        {
            Id = Id;
        }

        public Guid Id { get; }
    }

    public interface IMessageBus
    {
        void SendMessage(Message message);
    }

    public enum GameState
    {
        New = 0,
        Awaiting = 1,
        Started = 2,
        Finished = 3
    }

    public class RaceTrack: IGameComponent
    {
        private const  int DEFAULT_RACE_TRACK_WIDTH = 30;
        private const int DEFAULT_RACE_TRACK_HEIGHT = 30;

        private Guid? [,] _trackMatrix;
        private IList<Player> _players;

        public RaceTrack()
            :this(DEFAULT_RACE_TRACK_WIDTH, DEFAULT_RACE_TRACK_HEIGHT)
        {

        }

        public RaceTrack(int width, int height)
        {
            Width = width;
            Height = height;
            _trackMatrix = new Guid?[Width, Height];
            _players = new List<Player>();
        }

        public int Width { get; } = DEFAULT_RACE_TRACK_WIDTH;
        public int Height { get; } = DEFAULT_RACE_TRACK_HEIGHT;

        public void Update()
        {
            foreach (var player in _players)
            {
                MovePlayer(player);
            }
        }

        public void AddPlayer(Player player)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            _players.Add(player);

            SetRandomPosition(player);
        }

        private void MovePlayer(Player player)
        {
            switch(player.Direction)
            {
                case Direction.Left:
                    SetPlayerPosition(player, player.X - 1, player.Y);
                    break;
                case Direction.Up:
                    SetPlayerPosition(player, player.X, player.Y - 1);
                    break;
                case Direction.Right:
                    SetPlayerPosition(player, player.X + 1, player.Y);
                    break;
                case Direction.Down:
                    SetPlayerPosition(player, player.X - 1, player.Y + 1);
                    break;
            }
        }

        private void SetPlayerPosition(Player player, int x, int y)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            lock(_trackMatrix)
            {
                Guid? playerId;
                try
                {
                    playerId = _trackMatrix[x, y];
                }
                catch(IndexOutOfRangeException e)
                {
                    throw new PlayerOfftrackException($"Player [{player.Id}] off track ({player.X},{player.Y})");
                }

                if (playerId != null)
                {
                    throw new PlayerCollisionException($"Player [{player.Id}] collision ({player.X},{player.Y})");
                }

                player.SetPosition(x, y);
                _trackMatrix[x, y] = player.Id;
            }
        }

        private void SetRandomPosition(Player player)
        {
            var random  = new Random();
            var retry = false;
            do
            {
                var x = random.Next(0, Width - 1);
                var y = random.Next(0, Height - 1);
                
                try
                {
                    SetPlayerPosition(player, x, y);
                }
                catch (PlayerCollisionException e)
                {
                    retry = true;
                }
                catch (PlayerOfftrackException e)
                {
                    retry = true;
                }
            } while (!retry);
        }
    }

    public interface IGameComponent
    {
        void Update();
    }

    public interface  IGameCompositeComponent: IGameComponent
    {
        void Add (IGameComponent child);
        void Remove(IGameComponent child);
        IEnumerable<IGameComponent> GetChilds();
    }

    public class CompositeComponent : IGameCompositeComponent
    {
        public void Add(IGameComponent child)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameComponent> GetChilds()
        {
            throw new NotImplementedException();
        }

        public void Remove(IGameComponent child)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}