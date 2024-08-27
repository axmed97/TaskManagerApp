        using Business.Utilities.StatusMessages;
using Business.Utilities.StatusMessages.Abstract;

namespace Business.Utilities.StatusMessages.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        public string GetLocalizedString(string key, string culture)
        {
            return LocalizationMessages.GetLocalizedString(key, culture);
        }
    }
}