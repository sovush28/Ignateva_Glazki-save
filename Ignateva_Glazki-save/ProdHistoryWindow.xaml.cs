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
    /// Логика взаимодействия для ProdHistoryWindow.xaml
    /// </summary>
    public partial class ProdHistoryWindow : Window
    {
        private Agent _currentAgent = new Agent();

        private ProductSale _currentSale = new ProductSale();

        public ProdHistoryWindow(Agent SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null)
            {
                _currentAgent = SelectedAgent;
                //ComboType.SelectedIndex = _currentAgent.AgentTypeID - 1;
            }
            DataContext = _currentAgent;


            var currentAgentProdHistory = ИгнатьеваГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentAgentProdHistory = currentAgentProdHistory.Where(p => p.AgentID == _currentAgent.ID).ToList();

            ProdHistoryListview.ItemsSource = currentAgentProdHistory;

            DeleteBtn.Visibility = Visibility.Hidden;
            CancelSelectionBtn.Visibility = Visibility.Hidden;

            UpdateHeaderAgent();
        }

        void UpdateHeaderAgent()
        {
            ProdHistoryHeader.Text = _currentAgent.Title.ToString();
        }

        private void QuitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ИгнатьеваГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ProdHistoryListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdHistoryListview.SelectedItems.Count >= 1)
            {
                CancelSelectionBtn.Visibility = Visibility.Visible;
                AddBtn.Visibility = Visibility.Hidden;
                DeleteBtn.Visibility = Visibility.Visible;
            }

            if (ProdHistoryListview.SelectedItem != null)
            {
                _currentSale = (ProductSale)ProdHistoryListview.SelectedItem;
            }
            else
                _currentSale = null;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            //var currentProdHistory = ИгнатьеваГлазкиSaveEntities.GetContext().ProductSale.ToList();
            //currentProdHistory = currentProdHistory.Where(p => p.AgentID == _currentAgent.ID).ToList();

            //_currentSale.ID = 

            //DataContext = _currentSale;

            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    ИгнатьеваГлазкиSaveEntities.GetContext().ProductSale.Remove(_currentSale);

                    ИгнатьеваГлазкиSaveEntities.GetContext().SaveChanges();

                    MessageBox.Show("Позиция удалена");

                    this.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddProdHistoryWindow addProdHistoryWindow = new AddProdHistoryWindow((sender as Button).DataContext as Agent);
            addProdHistoryWindow.ShowDialog();
        }

        private void CancelSelectionBtn_Click(object sender, RoutedEventArgs e)
        {
            ProdHistoryListview.SelectedItems.Clear();
            DeleteBtn.Visibility = Visibility.Hidden;
            AddBtn.Visibility = Visibility.Visible;
            CancelSelectionBtn.Visibility = Visibility.Hidden;

        }
        //private void UpdateProdHistory()
        //{
        //    var currentAgentProdHistory = ИгнатьеваГлазкиSaveEntities.GetContext().ProductSale.ToList();
        //    currentAgentProdHistory = currentAgentProdHistory.Where(p => p.AgentID == _currentAgent.ID).ToList();

        //    ProdHistoryListview.ItemsSource = currentAgentProdHistory;
        //}

    }
}
