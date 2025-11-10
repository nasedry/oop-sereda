using System;

namespace lab5v15.Exceptions
{
    public class InvalidShipmentDatesException : Exception
    {
        public InvalidShipmentDatesException() { }

        public InvalidShipmentDatesException(string message) : base(message) { }

        public InvalidShipmentDatesException(string message, Exception inner) : base(message, inner) { }
    }
}
