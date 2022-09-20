using System;

namespace Scuti.GraphQL {
	public class GQLException : Exception {
        public long responseCode;
        public string error;
        public string response;

        public GQLException(long responseCode, string error, string response) : base(error) {
            this.responseCode = responseCode;
            this.error = error;
            this.response = response;
        }
    }
}
