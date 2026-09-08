namespace MalletCompassPlugin
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Model;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            var enumValues = Enum.GetValues(typeof(HeadType));
            HeadTypeComboBox.ItemsSource = enumValues.Cast<HeadType>();
        }

        /// <summary>
        /// Вызывается при изменении типа бойка киянки.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Аргументы.</param>
        private void HeadTypeComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            var selectedHeadType = (HeadType?)e.AddedItems[0];

            HeadDiameterTextBox.IsEnabled = selectedHeadType == HeadType.Cylinder;
            HeadChamferTextBox.IsEnabled = selectedHeadType == HeadType.Cylinder;
            HeadWidthTextBox.IsEnabled = selectedHeadType == HeadType.Rectangle;
            HeadHeightTextBox.IsEnabled = selectedHeadType == HeadType.Rectangle;

            RectangleMalletImage.Visibility = selectedHeadType == HeadType.Cylinder
                ? Visibility.Hidden
                : Visibility.Visible;
            CylinderMalletImage.Visibility = selectedHeadType == HeadType.Rectangle
                ? Visibility.Hidden
                : Visibility.Visible;
        }
    }
}