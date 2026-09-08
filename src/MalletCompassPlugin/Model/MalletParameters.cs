namespace Model
{
    /// <summary>
    /// Содержит параметры для модели киянки.
    /// </summary>
    public class MalletParameters
    {
        /// <summary>
        /// Радиус скругления цилиндрической формы бойка.
        /// </summary>
        private double _headChamferRadius;

        /// <summary>
        /// Диаметр цилиндрической формы бойка.
        /// </summary>
        private double _headDiameter;

        /// <summary>
        /// Ширина прямоугольной формы бойка.
        /// </summary>
        private double _headWidth;

        /// <summary>
        /// Высота прямоугольной формы бойка.
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
        /// Минимальный радиус скругления у цилиндрической формы бойка.
        /// </summary>
        public const double MinHeadChamferRadius = 0.0;

        /// <summary>
        /// Максимальный радиус скругления у цилиндрической формы бойка.
        /// </summary>
        public const double MaxHeadChamferRadius = 10.0;

        /// <summary>
        /// Минимальный диаметр цилиндрической формы бойка.
        /// </summary>
        public const double MinHeadDiameter = 50.0;

        /// <summary>
        /// Максимальный диаметр цилиндрической формы бойка.
        /// </summary>
        public const double MaxHeadDiameter = 100.0;

        /// <summary>
        /// Минимальная ширина прямоугольной формы бойка.
        /// </summary>
        public const double MinHeadWidth = 50.0;

        /// <summary>
        /// Максимальная ширина прямоугольной формы  бойка.
        /// </summary>
        public const double MaxHeadWidth = 100.0;

        /// <summary>
        /// Минимальная высота прямоугольной формы бойка.
        /// </summary>
        public const double MinHeadHeight = 50.0;

        /// <summary>
        /// Максимальная высота прямоугольной формы бойка.
        /// </summary>
        public const double MaxHeadHeight = 100.0;

        /// <summary>
        /// Минимальная длина бойка.
        /// </summary>
        public const double MinHeadLength = 75.0;

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
        public const double MaxHandleDiameter = 75.0;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MalletParameters()
        {
            var random = new Random();
            HeadType = HeadType.Rectangle;
            HeadChamferRadius = 0.0;
            HeadDiameter = random.Next((int)MinHeadDiameter, (int)MaxHeadDiameter);
            HeadWidth = HeadDiameter;
            HeadHeight = random.Next((int)MinHeadHeight, (int)MaxHeadHeight);
            var minHeadLength = HeadWidth < MinHeadLength ? MinHeadLength : HeadWidth + 25;
            HeadLength = random.Next((int)minHeadLength, (int)MaxHeadLength);
            var maxHandleDiameter = HeadWidth < MaxHandleDiameter ? HeadWidth : MaxHandleDiameter;
            HandleDiameter = random.Next((int)MinHandleDiameter, (int)maxHandleDiameter);
            HandleHeight = random.Next((int)MinHandleHeight, (int)MaxHandleHeight);
        }

        /// <summary>
        /// Конструктор с всеми параметрами.
        /// </summary>
        /// <param name="headType">Форма бойка.</param>
        /// <param name="headChamferRadius">Радиус скругления цилиндрической формы бойка.</param>
        /// <param name="headDiameter">Диаметр цилиндрической формы бойка.</param>
        /// <param name="headWidth">Ширина прямоугольной формы бойка.</param>
        /// <param name="headHeight">Высота прямоугольной формы бойка.</param>
        /// <param name="headLength">Длина бойка.</param>
        /// <param name="handleHeight">Высота рукоятки.</param>
        /// <param name="handleDiameter">Диаметр рукоятки.</param>
        public MalletParameters(
            HeadType headType,
            double headChamferRadius,
            double headDiameter,
            double headWidth,
            double headHeight,
            double headLength,
            double handleHeight,
            double handleDiameter)
        {
            HeadType = headType;
            _headChamferRadius = headChamferRadius;
            _headDiameter = headDiameter;
            _headWidth = headWidth;
            _headHeight = headHeight;
            _headLength = headLength;
            _handleHeight = handleHeight;
            _handleDiameter = handleDiameter;
        }

        /// <summary>
        /// Форма бойка.
        /// </summary>
        public HeadType HeadType { get; set; }

        /// <summary>
        /// Свойство для <see cref="_headChamferRadius"/>.
        /// </summary>
        public double HeadChamferRadius
        {
            get => _headChamferRadius;
            set
            {
                if (Validator.IsValueInRange(value, MinHeadChamferRadius, MaxHeadChamferRadius))
                {
                    _headChamferRadius = value;
                }
                else
                {
                    throw new Exception(
                        $"Радиус скругления цилиндрической бойка должен быть задана в следующем диапазоне: [{MinHeadChamferRadius} - {MaxHeadChamferRadius}]");
                }
            }
        }

        /// <summary>
        /// Свойство для <see cref="_headDiameter"/>.
        /// </summary>
        public double HeadDiameter
        {
            get => _headDiameter;
            set
            {
                var minHeadDiameter =
                    HandleDiameter < MinHeadDiameter ? MinHeadDiameter : HandleDiameter;

                if (Validator.IsValueInRange(value, minHeadDiameter, MaxHeadDiameter))
                {
                    _headDiameter = value;
                }
                else
                {
                    throw new Exception(
                        $"Диаметр цилиндрической формы бойка должен быть задана в следующем диапазоне: [{minHeadDiameter} - {MaxHeadDiameter}]");
                }
            }
        }

        /// <summary>
        /// Свойство для <see cref="_headWidth"/>.
        /// </summary>
        public double HeadWidth
        {
            get => _headWidth;
            set
            {
                var minHeadWidth =
                    HandleDiameter < MinHeadWidth ? MinHeadWidth : HandleDiameter;

                if (Validator.IsValueInRange(value, minHeadWidth, MaxHeadWidth))
                {
                    _headWidth = value;
                }
                else
                {
                    throw new Exception(
                        $"Ширина прямоугольной формы бойка должна быть задана в следующем диапазоне: [{minHeadWidth} - {MaxHeadWidth}]");
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
                        $"Высота прямоугольной формы бойка должна быть задана в следующем диапазоне: [{MinHeadHeight} - {MaxHeadHeight}]");
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
                var minHeadLength =
                    HeadWidth < MinHeadLength ? MinHeadLength : HeadWidth;

                if (Validator.IsValueInRange(value, minHeadLength, MaxHeadLength))
                {
                    _headLength = value;
                }
                else
                {
                    throw new Exception(
                        $"Длина бойка должна быть задана в следующем диапазоне: [{minHeadLength} - {MaxHeadLength}]");
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
                if (Validator.IsValueInRange(value, MinHandleDiameter, MaxHandleDiameter))
                {
                    _handleDiameter = value;
                }
                else
                {
                    throw new Exception(
                        $"Диаметр рукоятки должен быть задан в следующем диапазоне: [{MinHandleDiameter} - {MaxHandleDiameter}]");
                }
            }
        }
    }
}