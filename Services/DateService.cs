namespace PNC.Services;

public interface IDateService
{
    void UpdateDateComponents(DateTime date, out string jour, out string mois, out string annee);
    DateTime? ParseDateComponents(string jour, string mois, string annee);
    int CalculateAge(DateTime dateNaissance, DateTime dateReference);
    bool IsValidDateRange(DateTime date, DateTime minDate, DateTime maxDate);
    bool IsDateInFuture(DateTime date);
    bool IsDateInPast(DateTime date);
}

public class DateService : IDateService
{
    public void UpdateDateComponents(DateTime date, out string jour, out string mois, out string annee)
    {
        jour = date.Day.ToString("00");
        mois = date.Month.ToString("00");
        annee = date.Year.ToString();
    }
    
    public DateTime? ParseDateComponents(string jour, string mois, string annee)
    {
        if (string.IsNullOrEmpty(jour) || string.IsNullOrEmpty(mois) || string.IsNullOrEmpty(annee))
            return null;
            
        if (!int.TryParse(jour, out int j) || !int.TryParse(mois, out int m) || !int.TryParse(annee, out int a))
            return null;
            
        try
        {
            return new DateTime(a, m, j);
        }
        catch
        {
            return null;
        }
    }
    
    public int CalculateAge(DateTime dateNaissance, DateTime dateReference)
    {
        var age = dateReference.Year - dateNaissance.Year;
        if (dateReference < dateNaissance.AddYears(age))
        {
            age--;
        }
        return age;
    }
    
    public bool IsValidDateRange(DateTime date, DateTime minDate, DateTime maxDate)
    {
        return date >= minDate && date <= maxDate;
    }
    
    public bool IsDateInFuture(DateTime date)
    {
        return date > DateTime.Today;
    }
    
    public bool IsDateInPast(DateTime date)
    {
        return date < DateTime.Today;
    }
}
