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
using System.Windows.Shapes;

namespace Ignateva_Glazki_save
{
    /// <summary>
    /// Логика взаимодействия для AddProdHistoryWindow.xaml
    /// </summary>
    public partial class AddProdHistoryWindow : Window
    {
        private ProductSale _currentSale = new ProductSale();

        public AddProdHistoryWindow(Agent SelectedAgent)
        {
            InitializeComponent();
            _currentSale.AgentID = SelectedAgent.ID;
            DataContext = _currentSale;

            var Products = ИгнатьеваГлазкиSaveEntities.GetContext().Product.ToList();

            ComboProdName.ItemsSource = Products;
        }

        private void CancelAddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            //if(string.IsNullOrWhiteSpace(_currentSale.ProdName))
            //    errors.AppendLine("Укажите наименование продукции");

            //if (string.IsNullOrWhiteSpace(_currentSale.ProductCount.ToString()))
            //    errors.AppendLine("Укажите количество проданных единиц");

            //if (string.IsNullOrWhiteSpace(_currentSale.SaleDate.ToString()))
            //    errors.AppendLine("Укажите дату продажи");


            //if (TBProdName.Text.Length < 1)
            //    errors.AppendLine("Укажите наименование продукции");

            if (ComboProdName.SelectedItem == null)
            {
                errors.AppendLine("Укажите наименование продукции");
            }

            if (TBProdCount.Text.Length < 1)
                errors.AppendLine("Укажите количество проданных единиц");
            else
            {
                if (Convert.ToInt32(TBProdCount.Text) < 0)
                    errors.AppendLine("Количество продаж не может быть отрицательным числом");
            }

            if(SaleDatePicker.Text.Length < 1)
                errors.AppendLine("Укажите дату продажи");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentSale.ID == 0)
                ИгнатьеваГлазкиSaveEntities.GetContext().ProductSale.Add(_currentSale);

            _currentSale.ProductID = ComboProdName.SelectedIndex + 1;

            _currentSale.SaleDate = Convert.ToDateTime(SaleDatePicker.Text);

            _currentSale.ProductCount = Convert.ToInt32(TBProdCount.Text);


            try
            {
                ИгнатьеваГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }



        }
    }
}
