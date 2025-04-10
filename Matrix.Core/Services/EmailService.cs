using System.Text.RegularExpressions;

namespace Matrix.Core.Services;

public class EmailService : BaseService
{
    public bool IsValidEmail(string email)
    {
        //We may want to expand this further like checking they are not a spam email or blocked etc
        //For now we just check if the email is in a correct format
        const string regex = "^[^@]+@[^@]+\\.[^@]+$";
        return Regex.IsMatch(email, regex);
    }
}