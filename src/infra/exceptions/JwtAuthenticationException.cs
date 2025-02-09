public class JwtAuthenticationException : AuthenticationException
{
    public JwtAuthenticationException(string message)
        : base(message) { }
}