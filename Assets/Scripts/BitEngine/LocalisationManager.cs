using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LocalisationManager : BitGameManager<LocalisationManager>
{
    private readonly string[] _textsToTranslate = {"gameover", "victory"};
    private readonly Dictionary<string, Translation> _textTranslations = new Dictionary<string, Translation>();
    private readonly string[] _keywords = {"#VILLAGERS", "#DAYS"};


    private readonly Dictionary<string, string[]> _labelTranslations = new Dictionary<string, string[]>();

    private const string TranslationsDir = "Translations";
    private string _pathToDir;

    private int _translatedTexts;
    private bool _areLabelsTranslated;
    public bool AreTranslationsLoaded;

    private void Awake()
    {
        _pathToDir = Path.Combine(Application.streamingAssetsPath, TranslationsDir);
        InitLabelTranslations();
        InitTextTranslations();
    }

    private void InitTextTranslations()
    {
        foreach (var textName in _textsToTranslate)
        {
            foreach (Language language in Enum.GetValues(typeof(Language)))
            {
                var dim = GetDiminutive(language);
                var pathToText = Path.Combine(_pathToDir, $"{textName}_{dim}.txt");
                FileManager.Instance.ReadText(pathToText,
                    res =>
                    {
                        if (!_textTranslations.ContainsKey(textName))
                        {
                            var translation = new Translation();
                            translation.AddTranslation(language, res);
                            _textTranslations.Add(textName, translation);
                        }
                        else
                        {
                            _textTranslations[textName].AddTranslation(language, res);
                        }

                        ++_translatedTexts;
                        CheckIfFinishedTranslating();
                    });
            }
        }
    }

    private void CheckIfFinishedTranslating()
    {
        if (!_areLabelsTranslated ||
            _translatedTexts < _textsToTranslate.Length * GameApplication.Instance.LanguagesCount) return;

        AreTranslationsLoaded = true;
        TriggerCallback(NotificationType.OnFinishedTranslating);
    }

    private string SubstituteKeywords(string text)
    {
        var res = text;
        foreach (var keyword in _keywords)
        {
            var sub = GetSubstitution(keyword);
            res = res.Replace(keyword, sub);
        }

        return res;
    }

    private string GetSubstitution(string keyword)
    {
        switch (keyword)
        {
            case "#VILLAGERS":
                return TownModel.Instance.VillagersInIsland.ToString();
            case "#DAYS":
                return TimeController.Instance.CurrentDay.ToString();
            default:
                throw new IndexOutOfRangeException($"No keyword equivalence to {keyword}");
        }
    }

    private string GetDiminutive(Language language)
    {
        switch (language)
        {
            case Language.English:
                return "en";
            case Language.Spanish:
                return "es";
            default:
                throw new ArgumentOutOfRangeException(nameof(language), language, null);
        }
    }

    private void InitLabelTranslations()
    {
        var pathToTranslations = Path.Combine(_pathToDir, "translations.csv");
        FileManager.Instance.ReadText(pathToTranslations,
            res =>
            {
                var lines = res.Split(new[]
                    {
                        Environment.NewLine
                    },
                    StringSplitOptions.None
                );

                foreach (var line in lines)
                {
                    if (line.StartsWith("#"))
                    {
                        // Comment
                        continue;
                    }

                    var tokens = line.Split(',');
                    try
                    {
                        if (!string.IsNullOrEmpty(tokens[0]))
                        {
                            _labelTranslations.Add(tokens[0], tokens.Skip(1).ToArray());
                        }
                    }
                    catch (ArgumentException e)
                    {
                        Debug.LogError($"{tokens[0]} already in the collection: " + e);
                    }
                }

                _areLabelsTranslated = true;
            });
    }

    public string GetTranslation(string s)
    {
        try
        {
            return _labelTranslations[s][(int) GameApplication.Instance.CurrentLanguage];
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError("Translation for " + s + " not found.");
            return s;
        }
    }

    public string GetTextTranslation(string textName)
    {
        var language = GameApplication.Instance.CurrentLanguage;
        var translation = _textTranslations[textName].GetTranslation(language);
        return SubstituteKeywords(translation);
    }
}

public enum Language
{
    English,
    Spanish
}