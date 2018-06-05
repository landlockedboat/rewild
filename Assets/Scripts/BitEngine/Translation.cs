using System.Collections.Generic;

public class Translation
{
    private readonly Dictionary<Language, string> _translations = 
        new Dictionary<Language, string>();

    public void AddTranslation(Language language, string translation)
    {
        _translations.Add(language, translation);
    }

    public string GetTranslation(Language language)
    {
        return _translations[language];
    }
}