namespace Model
{
    /// <summary>
    /// Содержит параметры для модели киянки.
    /// </summary>
    public class MalletParameters
    {
        /// <summary>
        /// Минимальная ширина бойка.
        /// </summary>
        public const double MinHeadWidth = 50.0;

        /// <summary>
        /// Максимальная ширина бойка.
        /// </summary>
        public const double MaxHeadWidth = 100.0;

        /// <summary>
        /// Минимальная высота бойка.
        /// </summary>
        public const double MinHeadHeight = 50.0;

        /// <summary>
        /// Максимальная высота бойка.
        /// </summary>
        public const double MaxHeadHeight = 100.0;

        /// <summary>
        /// Минимальная длина бойка.
        /// </summary>
        public const double MinHeadLength = 50.0;

        /// <summary>
        /// Максимальная длина бойка.
        /// </summary>
        public const double MaxHeadLength = 200.0;

        /// <summary>
        /// Минимальная высота рукоятки.
        /// </summary>
        public const double MinHandleHeight = 100.0;

        /// <summary>
        /// Максимальная высота рукоятки.
        /// </summary>
        public const double MaxHandleHeight = 250.0;

        /// <summary>
        /// Минимальный диаметр рукоятки.
        /// </summary>
        public const double MinHandleDiameter = 25.0;

        /// <summary>
        /// Максимальный диаметр рукоятки.
        /// </summary>
        public const double MaxHandleDiameter = 100.0;

        /// <summary>
        /// Ширина бойка.
        /// </summary>
        private double _headWidth;

        /// <summary>
        /// Высота бойка.
        /// </summary>
        private double _headHeight;

        /// <summary>
        /// Длина бойка.
        /// </summary>
        private double _headLength;

        /// <summary>
        /// Высота рукоятки.
        /// </summary>
        private double _handleHeight;

        /// <summary>
        /// Диаметр рукоятки.
        /// </summary>
        private double _handleDiameter;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MalletParameters()
        {
            var random = new Random();
            HeadWidth = random.Next((int)MinHeadWidth, (int)MaxHeadWidth);
            HeadHeight = random.Next((int)MinHeadHeight, (int)MaxHeadHeight);
            var minHeadLength = HeadWidth < MinHeadLength ? MinHeadLength : HeadWidth;
            HeadLength = random.Next((int)minHeadLength, (int)MaxHeadLength);
            HandleHeight = random.Next((int)MinHandleHeight, (int)MaxHandleHeight);
            HandleDiameter = random.Next((int)MinHandleDiameter, (int)(HeadWidth / 2.0));
        }

        /// <summary>
        /// Свойство для <see cref="_headWidth"/>.
        /// </summary>
        public double HeadWidth
        {
            get => _headWidth;
            set
            {
                if (Validator.IsValueInRange(value, MinHeadWidth, MaxHeadWidth))
                {
                    _headWidth = value;
                }
                else
                {
                    throw new Exception(
                        $"Ширина бойка должна быть задана в следующем диапазоне: [{MinHeadWidth} - {MaxHeadWidth}]");
                }
            }
        }

        /// <summary>
        /// Свойство для <see cref="_headHeight"/>.
        /// </summary>
        public double HeadHeight
        {
            get => _headHeight;
            set
            {
                if (Validator.IsValueInRange(value, MinHeadHeight, MaxHeadHeight))
                {
                    _headHeight = value;
                }
                else
                {
                    throw new Exception(
                        $"Высота бойка должна быть задана в следующем диапазоне: [{MinHeadHeight} - {MaxHeadHeight}]");
                }
            }
        }

        /// <summary>
        /// Свойство для <see cref="_headLength"/>.
        /// </summary>
        public double HeadLength
        {
            get => _headLength;
            set
            {
                if (Validator.IsValueInRange(value, MinHeadLength, MaxHeadLength)
                    && Validator.IsValueInRange(value, HeadWidth, MaxHeadLength))
                {
                    _headLength = value;
                }
                else
                {
                    throw new Exception(
                        $"Длина бойка должна быть задана в следующем диапазоне: [{HeadWidth} - {MaxHeadLength}]");
                }
            }
        }

        /// <summary>
        /// Свойство для <see cref="_handleHeight"/>.
        /// </summary>
        public double HandleHeight
        {
            get => _handleHeight;
            set
            {
                if (Validator.IsValueInRange(value, MinHandleHeight, MaxHandleHeight))
                {
                    _handleHeight = value;
                }
                else
                {
                    throw new Exception(
                        $"Высота рукоятки должен быть задана в следующем диапазоне: [{MinHandleHeight} - {MaxHandleHeight}]");
                }
            }
        }

        /// <summary>
        /// Свойство для <see cref="_handleDiameter"/>.
        /// </summary>
        public double HandleDiameter
        {
            get => _handleDiameter;
            set
            {
                if (Validator.IsValueInRange(value, MinHandleDiameter, MaxHandleDiameter)
                    && Validator.IsValueInRange(value, MinHandleDiameter, HeadWidth))
                {
                    _handleDiameter = value;
                }
                else
                {
                    throw new Exception(
                        $"Диаметр рукоятки должен быть задан в следующем диапазоне: [{MinHandleDiameter} - {HeadWidth}]");
                }
            }
        }
    }
}