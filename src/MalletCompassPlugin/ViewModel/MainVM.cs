namespace ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CompassWrapper;
    using Model;
    using ViewModel.AttributeValidators;

    /// <summary>
    /// Основная модель представления, связанная с <see cref="MalletParameters"/>.
    /// </summary>
    public partial class MainVM : ObservableValidator
    {
        /// <summary>
        /// Через это поле View сообщает, что одно из полей содержит недопустимые для ввода символы.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        private bool _hasInvalidInput;

        /// <summary>
        /// Форма бойка.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        private HeadType _headType;

        /// <summary>
        /// Радиус скругления цилиндрической формы бойка.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            0.0,
            10.0,
            ErrorMessage =
                "Радиус скругления цилиндрической формы бойка должна быть задан в следующем диапазоне: [0 - 10]")]
        [Required]
        private double _headChamferRadius;

        /// <summary>
        /// Диаметр цилиндрической формы бойка.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            50.0,
            100.0,
            ErrorMessage = "Диаметр бойка должна быть задан в следующем диапазоне: [50 - 100]")]
        [GreaterThan(
            nameof(HandleDiameter),
            ErrorMessage =
                "Диаметр цилиндрической формы бойка должна быть больше диаметра его рукоятки")]
        [Required]
        private double _headDiameter;

        /// <summary>
        /// Ширина прямоугольной формы бойка.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            50.0,
            100.0,
            ErrorMessage =
                "Ширина прямоугольной формы бойка должна быть задана в следующем диапазоне: [50 - 100]")]
        [GreaterThan(
            nameof(HandleDiameter),
            ErrorMessage = "Ширина бойка должна быть больше диаметра его рукоятки")]
        [LessThan(
            nameof(HeadLength),
            ErrorMessage = "Ширина бойка должна быть меньше его длины")]
        [Required]
        private double _headWidth;

        /// <summary>
        /// Высота прямоугольной формы бойка.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            50.0,
            100.0,
            ErrorMessage =
                "Высота прямоугольной формы бойка должна быть задана в следующем диапазоне: [50 - 100]")]
        [Required]
        private double _headHeight;

        /// <summary>
        /// Длина бойка.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            75.0,
            200.0,
            ErrorMessage = "Длина бойка должна быть задана в следующем диапазоне: [75 - 200]")]
        [Required]
        private double _headLength;

        /// <summary>
        /// Высота рукоятки.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            100.0,
            250.0,
            ErrorMessage =
                "Высота рукоятки должен быть задана в следующем диапазоне: [100 - 250]")]
        [Required]
        private double _handleHeight;

        /// <summary>
        /// Диаметр рукоятки.
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuildCompassCommand))]
        [NotifyCanExecuteChangedFor(nameof(BuildOpenGLCommand))]
        [NotifyDataErrorInfo]
        [Range(
            25.0,
            75.0,
            ErrorMessage = "Диаметр рукоятки должен быть задан в следующем диапазоне: [25 - 75]")]
        [Required]
        private double _handleDiameter;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainVM()
        {
            MalletParameters = new MalletParameters();
            CompassWrapper = new CompassWrapper();

            _headType = MalletParameters.HeadType;
            _headDiameter = MalletParameters.HeadDiameter;
            _headWidth = MalletParameters.HeadWidth;
            _headHeight = MalletParameters.HeadHeight;
            _headLength = MalletParameters.HeadLength;
            _handleHeight = MalletParameters.HandleHeight;
            _handleDiameter = MalletParameters.HandleDiameter;
        }

        /// <summary>
        /// Обёртка для компаса.
        /// </summary>
        public CompassWrapper CompassWrapper { get; set; }

        /// <summary>
        /// Параметры киянки.
        /// </summary>
        public MalletParameters MalletParameters { get; set; }

        /// <summary>
        /// Метод вызывается после изменения формы бойка.
        /// </summary>
        /// <param name="value">Новое значение для формы бойка.</param>
        partial void OnHeadTypeChanged(HeadType value)
        {
            HasInvalidInput = false;
            switch (value)
            {
                case HeadType.Rectangle:
                {
                    ClearErrors(nameof(HeadDiameter));
                    ValidateProperty(HeadWidth, nameof(HeadWidth));
                    ValidateProperty(HeadHeight, nameof(HeadHeight));
                    ValidateProperty(HandleDiameter, nameof(HandleDiameter));

                    if (!HasErrors)
                    {
                        MalletParameters.HeadWidth = HeadWidth;
                        MalletParameters.HeadHeight = HeadHeight;
                        MalletParameters.HandleDiameter = HandleDiameter;
                    }

                    break;
                }

                case HeadType.Cylinder:
                {
                    ClearErrors(nameof(HeadWidth));
                    ClearErrors(nameof(HeadHeight));
                    ValidateProperty(HeadDiameter, nameof(HeadDiameter));
                    ValidateProperty(HandleDiameter, nameof(HandleDiameter));

                    if (!HasErrors)
                    {
                        MalletParameters.HeadDiameter = HeadDiameter;
                        MalletParameters.HandleDiameter = HandleDiameter;
                    }

                    break;
                }
            }

            MalletParameters.HeadType = value;
        }

        /// <summary>
        /// Метод вызывается после изменения радиуса скругления у цилиндрической формы бойка.
        /// </summary>
        /// <param name="value">Новое значение для радиуса скругления.</param>
        partial void OnHeadChamferRadiusChanged(double value)
        {
            if (!HasErrors)
            {
                MalletParameters.HeadChamferRadius = value;
            }
        }

        /// <summary>
        /// Метод вызывается после изменения диаметра цилиндрической формы бойка.
        /// </summary>
        /// <param name="value">Новое значение для диаметра бойка.</param>
        partial void OnHeadDiameterChanged(double value)
        {
            ValidateProperty(HandleDiameter, nameof(HandleDiameter));

            if (!HasErrors)
            {
                MalletParameters.HandleDiameter = HandleDiameter;

                MalletParameters.HeadDiameter = value;
            }
        }

        /// <summary>
        /// Метод вызывается после изменения ширины прямоугольной формы бойка.
        /// </summary>
        /// <param name="value">Новое значение для ширины бойка.</param>
        partial void OnHeadWidthChanged(double value)
        {
            ValidateProperty(HandleDiameter, nameof(HandleDiameter));

            if (!HasErrors)
            {
                MalletParameters.HandleDiameter = HandleDiameter;
                MalletParameters.HeadLength = HeadLength;

                MalletParameters.HeadWidth = value;
            }
        }

        /// <summary>
        /// Метод вызывается после изменения высоты прямоугольной формы бойка.
        /// </summary>
        /// <param name="value">Новое значение для высоты бойка.</param>
        partial void OnHeadHeightChanged(double value)
        {
            if (!HasErrors)
            {
                MalletParameters.HeadHeight = value;
            }
        }

        /// <summary>
        /// Метод вызывается после изменения длины бойка.
        /// </summary>
        /// <param name="value">Новое значение для длина бойка.</param>
        partial void OnHeadLengthChanged(double value)
        {
            if (HeadType == HeadType.Rectangle)
            {
                ValidateProperty(HeadWidth, nameof(HeadWidth));
            }

            if (!HasErrors)
            {
                if (HeadType == HeadType.Rectangle)
                {
                    MalletParameters.HeadWidth = HeadWidth;
                }

                MalletParameters.HeadLength = value;
            }
        }

        /// <summary>
        /// Метод вызывается после изменения высоты рукоятки.
        /// </summary>
        /// <param name="value">Новое значение для высоты рукоятки.</param>
        partial void OnHandleHeightChanged(double value)
        {
            if (!HasErrors)
            {
                MalletParameters.HandleHeight = value;
            }
        }

        /// <summary>
        /// Метод вызывается после изменения диаметра рукоятки.
        /// </summary>
        /// <param name="value">Новое значение для диаметра рукоятки.</param>
        partial void OnHandleDiameterChanged(double value)
        {
            if (HeadType == HeadType.Rectangle)
            {
                ValidateProperty(HeadWidth, nameof(HeadWidth));
            }
            else
            {
                ValidateProperty(HeadDiameter, nameof(HeadDiameter));
            }

            if (!HasErrors)
            {
                if (HeadType == HeadType.Rectangle)
                {
                    MalletParameters.HeadWidth = HeadWidth;
                }
                else
                {
                    MalletParameters.HeadDiameter = HeadDiameter;
                }

                MalletParameters.HandleDiameter = value;
            }
        }

        /// <summary>
        /// Команда для построения модели киянки используя OpenGL и сохранения её параметров в файл.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanBuild))]
        private void BuildOpenGL()
        {
            var processes = Process.GetProcessesByName("mallet_by_opengl");
            if (processes.Length == 0)
            {
                Process.Start("../../../../../MalletOpenGLRenderer/build/mallet_by_opengl.exe");
            }

            List<byte[]> bytes = new ()
            {
                BitConverter.GetBytes(MalletParameters.HeadWidth),
                BitConverter.GetBytes(MalletParameters.HeadHeight),
                BitConverter.GetBytes(MalletParameters.HeadLength),
                BitConverter.GetBytes(MalletParameters.HandleHeight),
                BitConverter.GetBytes(MalletParameters.HandleDiameter),
                BitConverter.GetBytes(MalletParameters.HeadDiameter),
                BitConverter.GetBytes((int)MalletParameters.HeadType)
            };

            using var file = File.Create("mallet_parameters");
            foreach (var parameter in bytes)
            {
                file.Write(parameter);
            }
        }

        /// <summary>
        /// Команда для построения модели киянки в компасе.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanBuild))]
        private void BuildCompass()
        {
            MalletBuilder.Build(CompassWrapper, MalletParameters);
        }

        /// <summary>
        /// Метод вызывается после изменения какого-либо параметра киянки и отвечает за
        /// выключение кнопки <see cref="BuildCompass"/> для построения модели.
        /// </summary>
        /// <returns>Если параметры модели заданны правильно вернёт true, иначе false.</returns>
        private bool CanBuild()
        {
            return !HasErrors && !HasInvalidInput;
        }
    }
}