namespace Business.Utilities.StatusMessages.Abstract
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string key, string culture);
    }
}
