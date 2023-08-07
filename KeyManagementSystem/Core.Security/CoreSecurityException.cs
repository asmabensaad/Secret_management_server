namespace Core.Security;

//TODO: Document the code.
[Serializable]
public class CoreSecurityException : Exception
{
    /// <summary>
    /// Generate Exception
    /// </summary>
    public CoreSecurityException()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public CoreSecurityException(string message) : base(message)
    {
    }
}