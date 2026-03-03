using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickHouseApp.DbService.Exceptions
{
    public class ClickHouseException : Exception
    {
        public ClickHouseException(string msg) : base(msg, null)
        {
        }

        public ClickHouseException() : base()
        {
        }

        public ClickHouseException(string? message, Exception? innerException) : base(message, innerException)
        {
            _ = $"ClickHouse: {message}";
        }

        protected ClickHouseException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
