namespace PNC.Services;

public class ValidationResult
{
    private readonly Dictionary<string, List<string>> _errors = new();
    
    public bool IsValid => !_errors.Any();
    
    public IReadOnlyDictionary<string, List<string>> Errors => _errors;
    
    public void AddError(string field, string message)
    {
        if (!_errors.ContainsKey(field))
        {
            _errors[field] = new List<string>();
        }
        
        if (!_errors[field].Contains(message))
        {
            _errors[field].Add(message);
        }
    }
    
    public void AddErrors(string field, IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            AddError(field, message);
        }
    }
    
    public void ClearErrors()
    {
        _errors.Clear();
    }
    
    public void ClearErrors(string field)
    {
        if (_errors.ContainsKey(field))
        {
            _errors.Remove(field);
        }
    }
    
    public bool HasError(string field)
    {
        return _errors.ContainsKey(field) && _errors[field].Any();
    }
    
    public string GetFirstError(string field)
    {
        return _errors.ContainsKey(field) && _errors[field].Any() 
            ? _errors[field].First() 
            : string.Empty;
    }
    
    public List<string> GetErrors(string field)
    {
        return _errors.ContainsKey(field) ? _errors[field] : new List<string>();
    }
    
    public void Merge(ValidationResult other)
    {
        foreach (var kvp in other._errors)
        {
            foreach (var error in kvp.Value)
            {
                AddError(kvp.Key, error);
            }
        }
    }
    
    public override string ToString()
    {
        if (IsValid)
            return "Validation successful";
            
        var errorMessages = _errors
            .SelectMany(kvp => kvp.Value.Select(error => $"{kvp.Key}: {error}"))
            .ToList();
            
        return string.Join("; ", errorMessages);
    }
}
