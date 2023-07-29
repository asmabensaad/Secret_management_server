namespace Core.Security;

//TODO: Document the code.
[Serializable]
public class CoreSecurityException : Exception
{
    public CoreSecurityException()
    {
    }

    public CoreSecurityException(string message) : base(message)
    {
    }
}