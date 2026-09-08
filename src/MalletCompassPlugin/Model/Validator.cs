namespace Model
{
    /// <summary>
    /// Класс для базовой валидации значений.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Проверяет входит ли значение в заданный диапазон.
        /// </summary>
        /// <param name="value">Значение для проверки.</param>
        /// <param name="min">Минимум.</param>
        /// <param name="max">Максимум.</param>
        /// <returns>Возвращает true если значение входит в диапазон, иначе false.</returns>
        public static bool IsValueInRange(double value, double min, double max)
        {
            return min <= value && value <= max;
        }
    }
}