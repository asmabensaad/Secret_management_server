namespace Core.Security;

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