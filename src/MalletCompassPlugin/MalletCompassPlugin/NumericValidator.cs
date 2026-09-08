namespace MalletCompassPlugin
{
    using System.Globalization;
    using System.Windows.Controls;

    /// <summary>
    /// Проверяет введено ли число.
    /// </summary>
    public class NumericValidator : ValidationRule
    {
        /// <summary>
        /// Метод вызывается после изменении свойства для его дальнейшей валидации.
        /// </summary>
        /// <param name="value">Значение для валидации.</param>
        /// <param name="cultureInfo">Информация о региональных параметрах.</param>
        /// <returns>Результат валидации.</returns>
        public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
        {
            var valueStr = (string?)value;

            if (string.IsNullOrEmpty(valueStr))
            {
                return new ValidationResult(
                    false,
                    "Числовое поле не может быть пустым");
            }

            try
            {
                double.Parse(valueStr, CultureInfo.InvariantCulture);
            }
            catch
            {
                return new ValidationResult(
                    false,
                    "Числовое поле содержит недопустимый символ");
            }

            return ValidationResult.ValidResult;
        }
    }
}