namespace Matrix.Core.Exceptions;

public class BikeNotFoundException : BaseMatrixException
{
    public BikeNotFoundException(string message) : base(message)
    {
    }

    public BikeNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}