namespace FlipazonApi.Utility
{
   public class HttpResponseException(int statusCode, string message) : Exception(message)
   {
        public int StatusCode { get; } = statusCode;
    }
}

