using System;

namespace EtronServer.Domain.Exceptions
{
    public class PlayerOfftrackException : Exception
    {
        public PlayerOfftrackException(string message) : base(message)
        {
        }
    }

    public class PlayerCollisionException : Exception
    {
        public PlayerCollisionException(string message) : base(message)
        {
        }
    }

    public class PlayerInvalidDirectionException : Exception
    {
        public PlayerInvalidDirectionException(string message) : base(message)
        {
        }
    }
}