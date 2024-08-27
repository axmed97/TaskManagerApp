namespace Business.Utilities.StatusMessages
{
    public static class LocalizationMessages
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _messages = new()
        {
            {"az", new Dictionary<string, string>
                {
                    { "UserNotFound", "İstifadəçi tapılmadı!" },
                    { "EmailAlreadyExists", "E-poçt artıq mövcuddur!" },
                    { "RegistrationSuccess", "Uğurla qeydiyyatdan keçdiniz!" },
                    { "EmailNotConfirmed", "E-poçtunuz təsdiqlənmiyib!" },
                }
            },
            {"ru-RU", new Dictionary<string, string>
                {
                    { "UserNotFound", "Пользователь не найден!" },
                    { "EmailAlreadyExists", "Email уже используется!" },
                    { "RegistrationSuccess", "Регистрация прошла успешно!" },
                    { "EmailNotConfirmed", "Ваш адрес электронной почты не подтвержден!" },
                }
            },
            {"en-US", new Dictionary<string, string>
                {
                    { "UserNotFound", "User not found!" },
                    { "EmailAlreadyExists", "Email is already in use!" },
                    { "RegistrationSuccess", "Registration successful!" },
                    { "EmailNotConfirmed", "Your email has not been verified!" },
                }
            }
        };

        public static string GetLocalizedString(string key, string culture)
        {
            if (_messages.TryGetValue(culture, out var localizedMessages) && localizedMessages.TryGetValue(key, out var message))
            {
                return message;
            }
            // Default to Aerbaijan if the specified culture or key is not found
            return _messages["az"][key];
        }
    }
}
