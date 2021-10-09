using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF6.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditServicePage.xaml
    /// </summary>
    public partial class AddOrEditServicePage : Page
    {
        private Entities.Service _currentService = null;

        private byte[] _mainImageData = null;

        public AddOrEditServicePage()
        {
            InitializeComponent();
        }
        public AddOrEditServicePage(Entities.Service service)
        {
            InitializeComponent();
            Title = "Редактировать услугу";
            _currentService = service;
            TBoxTitle.Text = _currentService.Title;
            TBoxCost.Text = _currentService.Cost.ToString("N2");
            TBoxDuration.Text = (_currentService.DurationInSeconds / 60).ToString();
            TBoxDescription.Text = _currentService.Description;
            
            if (_currentService.Discount > 0)
            { 
                TBoxDiscount.Text = (_currentService.Discount.Value * 100).ToString();
            }
            
            if (_currentService.MainImage != null)
            {
                ImageService.Source = new ImageSourceConverter()
                  .ConvertFrom(_currentService.MainImage) as ImageSource;
            }
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image | *.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageService.Source = new ImageSourceConverter().ConvertFrom(_mainImageData) as ImageSource;
            }
               
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckError();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            }
            else
            {
                if (_currentService == null)
                {
                    var service = new Entities.Service
                    {
                        Title = TBoxTitle.Text,
                        Cost = decimal.Parse(TBoxCost.Text),
                        DurationInSeconds = int.Parse(TBoxDuration.Text) * 60,
                        Discount = (string.IsNullOrWhiteSpace(TBoxDiscount.Text))
                        ? 0
                        : double.Parse(TBoxDiscount.Text) / 100, MainImage = _mainImageData
                    };

                    App.Context.Services.Add(service);
                    App.Context.SaveChanges();
                }
                else
                {
                    _currentService.Title = TBoxTitle.Text;
                    _currentService.Cost = decimal.Parse(TBoxCost.Text);
                    _currentService.DurationInSeconds = int.Parse(TBoxDuration.Text) * 60;
                    _currentService.Discount = (string.IsNullOrWhiteSpace(TBoxDiscount.Text))
                    ? 0 
                    : double.Parse(TBoxDiscount.Text) / 100;
                    if (_mainImageData != null)
                    {
                        _currentService.MainImage = _mainImageData;
                    }

                    App.Context.SaveChanges();
                    //Сообщение об успешном редактировании
                }


                NavigationService.GoBack();

            }
        }

        private string CheckError()
        {
            var errorBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TBoxTitle.Text))
                errorBuilder.AppendLine("- название услуги обязательно для заполнения;");

            var currentService = App.Context.Services.ToList()
                .FirstOrDefault(p => p.Title.ToLower() == TBoxTitle.Text.ToLower());
            if (currentService != null && currentService != _currentService)
            { errorBuilder.AppendLine(" - название услуги должно быть уникальным");}
            decimal cost = 0;
            
            if (decimal.TryParse(TBoxCost.Text, out cost) == false
                ||cost<=0)
            { errorBuilder.AppendLine("- стоимость услуги должна быть числом"); }
            
            int durationInMinutes = 0;
            if (int.TryParse(TBoxDuration.Text, out durationInMinutes) == false
                || durationInMinutes <= 0 || durationInMinutes > 240)
            {
                errorBuilder.AppendLine("- длительность оказания услуги должна быть положительным числом (не более, чем 4 часа)");
            }

            if (string.IsNullOrWhiteSpace(TBoxDiscount.Text) == false)
            {
                int discount = 0;
                if (int.TryParse(TBoxDiscount.Text, out discount) == false
                    || discount < 0 || discount > 100)
                {
                    errorBuilder.AppendLine("- процент скидки - целое число в диапазоне от 0 до 100;");
                }
            }

            if (errorBuilder.Length > 0)
            {
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");
            }


                return errorBuilder.ToString();
        }
    }
}
