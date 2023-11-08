using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CompassWrapper;
using Model;
using System;
using System.ComponentModel.DataAnnotations;
using ViewModel.AttibuteValidators;

namespace ViewModel;

/// <summary>
/// Основная модель представления, связанная с <see cref="MalletParameters"/>
/// </summary>
public partial class MainVM : ObservableValidator
{
    /// <summary>
    /// Ширина бойка.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyDataErrorInfo]
    [Range(50.0, 100.0, ErrorMessage = "Ширина бойка должна быть задана в следующем диапазоне: [50 - 100]")]
    [GreaterThan(nameof(HandleDiameter), ErrorMessage = "Ширина бойка должна быть больше диаметра его рукоятки")]
    [Required]
    private double _headWidth;

    /// <summary>
    /// Высота бойка.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyDataErrorInfo]
    [Range(50.0, 100.0, ErrorMessage = "Высота бойка должна быть задана в следующем диапазоне: [50 - 100]")]
    [Required]
    private double _headHeight;

    /// <summary>
    /// Длина бойка.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyDataErrorInfo]
    [Range(100.0, 150.0, ErrorMessage = "Длина бойка должна быть задана в следующем диапазоне: [100 - 150]")]
    [GreaterThan(nameof(HeadWidth), ErrorMessage = "Длина бойка должена быть больше его ширины")]
    [Required]
    private double _headLength;

    /// <summary>
    /// Высота рукоятки.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyDataErrorInfo]
    [Range(100.0, 250.0, ErrorMessage = "Высота рукоятки должнен быть задана в следующем диапазоне: [100 - 250]")]
    [Required]
    private double _handleHeight;

    /// <summary>
    /// Диаметр рукоятки.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(BuildCommand))]
    [NotifyDataErrorInfo]
    [Range(25.0, 50.0, ErrorMessage = "Диаметр рукоятки должнен быть задан в следующем диапазоне: [25 - 50]")]
    [Required]
    private double _handleDiameter;

    /// <summary>
    /// Обёртка для компаса.
    /// </summary>
    public CompassWrapper.CompassWrapper CompassWrapper { get; set; }

    /// <summary>
    /// Параметры киянки.
    /// </summary>
    public MalletParameters MalletParameters { get; set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainVM()
    {
        MalletParameters = new();
        CompassWrapper = new();

        HeadWidth = MalletParameters.HeadWidth;
        HeadHeight = MalletParameters.HeadHeight;
        HeadLength = MalletParameters.HeadLength;
        HandleHeight = MalletParameters.HandleHeight;
        HandleDiameter = MalletParameters.HandleDiameter;
    }

    /// <summary>
    /// Метод вызывается после изменения ширины бойка.
    /// </summary>
    /// <param name="value">Новое значение для ширины бойка.</param>
    partial void OnHeadWidthChanged(double value)
    {
        ValidateProperty(HandleDiameter, nameof(HandleDiameter));
        if (!HasErrors)
        {
            MalletParameters._headWidth = value;
        }
    }

    /// <summary>
    /// Метод вызывается после изменения высоты бойка.
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
        if (!HasErrors)
        {
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
        ValidateProperty(HeadWidth, nameof(HeadWidth));
        if (!HasErrors)
        {
            MalletParameters.HandleDiameter = value;
        }
    }

    /// <summary>
    /// Команда для построения модели киянки в компасе.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanBuild))]
    private void Build()
    {
        MalletBuilder.Build(CompassWrapper, MalletParameters);
    }

    /// <summary>
    /// Метод вызывается после изменения какого-либо параметра киянки и отвечается за
    /// выключение кнопки <see cref="Build"/> для построения модели.
    /// </summary>
    /// <returns>Если параметры модели заданны правильно вернёт true, иначе false.</returns>
    private bool CanBuild()
    {
        return !HasErrors;
    }
}
