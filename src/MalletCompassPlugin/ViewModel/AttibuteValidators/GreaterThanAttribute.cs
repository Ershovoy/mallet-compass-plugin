namespace ViewModel.AttibuteValidators
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Проверяет, что введённое значение больше заданного.
    /// </summary>
    public class GreaterThanAttribute : ValidationAttribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        public GreaterThanAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Имя свойства.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Метод вызывается для проверки, что введённое значение больше значения,
        /// хранящегося в <see cref="PropertyName"/>.
        /// </summary>
        /// <param name="value">Введенное значение для сравнения.</param>
        /// <param name="validationContext">Контекст валидатора.</param>
        /// <returns>Результат.</returns>
        protected override ValidationResult IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var otherValue = instance.GetType().GetProperty(PropertyName)?.GetValue(instance);

            if (((IComparable)value!).CompareTo(otherValue) > 0)
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}