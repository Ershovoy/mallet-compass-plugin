namespace MalletCompassPlugin
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Текстовое поле с функцией проверки, что введено число.
    /// </summary>
    public class NumericTextBox : TextBox
    {
        /// <summary>
        /// Свойство зависимости для контроля появления ошибки ввода.
        /// </summary>
        public static readonly DependencyProperty HasTextInputErrorProperty
            = DependencyProperty.Register(
                nameof(HasTextInputError),
                typeof(bool),
                typeof(NumericTextBox),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Валидатор для проверки, что введено число.
        /// </summary>
        private static readonly NumericValidator _numericValidator = new ();

        /// <summary>
        /// Сведение о привязках этого экземпляра контрола.
        /// </summary>
        private BindingExpression? _bindingExpression;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public NumericTextBox()
        {
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Позволяет узнать, возникла ли ошибка ввода.
        /// </summary>
        public bool HasTextInputError
        {
            get => (bool)GetValue(HasTextInputErrorProperty);
            set => SetValue(HasTextInputErrorProperty, value);
        }

        /// <summary>
        /// Обработчик изменения текста.
        /// </summary>
        /// <param name="e">Атрибуты.</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_bindingExpression == null)
            {
                return;
            }

            HasTextInputError = _bindingExpression.HasValidationError;
        }

        /// <summary>
        /// Действия после полной загрузки ресурсов контрола.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _bindingExpression = GetBindingExpression(TextProperty);
            _bindingExpression?.ParentBinding.ValidationRules.Add(_numericValidator);
        }
    }
}