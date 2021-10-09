using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();

            if (App.CurrentUser.RoleId == 1)
            {
                BtnCreateService.Visibility = Visibility.Visible;
            }
            else
            {
                BtnCreateService.Visibility = Visibility.Collapsed;
            }

            ComboSortBy.SelectedIndex = 0;
            ComboDiscount.SelectedIndex = 0;
        }

        private void BtnCreateService_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrEditServicePage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentService = button.DataContext as Entities.Service;

            NavigationService.Navigate(new AddOrEditServicePage(currentService));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentService = button.DataContext as Entities.Service;
            if (MessageBox.Show($"Вы уверены, что хотите удалить услугу: {currentService.Title}?",
            "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            App.Context.Services.Remove(currentService);
            App.Context.SaveChanges(); UpdateServices();
            // Сообщение об успешном удалении.
        }

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void UpdateServices()
        {
            var services = App.Context.Services.ToList();

            if (ComboSortBy.SelectedIndex==0)
            {
                services = services.OrderBy(p => p.CostWithDiscount).ToList();
            }
            else
            {
                services = services.OrderByDescending(p => p.CostWithDiscount).ToList();
            }
            
            if (ComboDiscount.SelectedIndex==1)
                services = services.Where(p => p.Discount >= 0 && p.Discount < 0.05).ToList();

            if (ComboDiscount.SelectedIndex == 2)
                services = services.Where(p => p.Discount >= 0.05 && p.Discount < 0.15).ToList();
            if (ComboDiscount.SelectedIndex == 3)
                services = services.Where(p => p.Discount >= 0.15 && p.Discount < 0.30).ToList();
            if (ComboDiscount.SelectedIndex == 4)
                services = services.Where(p => p.Discount >= 0.30 && p.Discount < 0.70).ToList();
            if (ComboDiscount.SelectedIndex == 5)
                services = services.Where(p => p.Discount >= 0.70 && p.Discount <= 1).ToList();

            services = services.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            //LViewService.ItemsSource = null;
            LViewService.ItemsSource = services;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
    }
}
