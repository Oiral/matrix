namespace Matrix.Core.Exceptions;

public abstract class BaseMatrixException : Exception
{
    protected BaseMatrixException(string message) : base(message)
    {
        
    }

    protected BaseMatrixException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}