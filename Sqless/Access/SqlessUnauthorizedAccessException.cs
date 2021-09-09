using System;

namespace Sqless.Access {
    public class SqlessUnauthorizedAccessException : UnauthorizedAccessException {
        public SqlessUnauthorizedAccessException() : base() { }
        public SqlessUnauthorizedAccessException(string message) : base(message) { }
        public SqlessUnauthorizedAccessException(string message, Exception inner) : base(message, inner) { }
    }
}
