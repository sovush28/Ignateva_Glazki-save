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

namespace Ignateva_Glazki_save
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();

            var currentAgents = ИгнатьеваГлазкиSaveEntities.GetContext().Agent.ToList();
            var currentAgentTypes = ИгнатьеваГлазкиSaveEntities.GetContext().AgentType.ToList();
            AgentListView.ItemsSource = currentAgents;

            ComboSort.SelectedIndex = 0;
            ComboFilter.SelectedIndex = 0;
        }

        private void UpdateAgents()
        {
            var currentAgents = ИгнатьеваГлазкиSaveEntities.GetContext().Agent.ToList();
            var currentAgentTypes = ИгнатьеваГлазкиSaveEntities.GetContext().AgentType.ToList();

            switch (ComboSort.SelectedIndex)
            {
                case 1:
                    currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
                    break;

                case 2:
                    currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
                    break;

                    //скидка по возрастанию
                case 3:
                    break;

                //скидка по убыванию
                case 4:
                    break;

                case 5:
                    currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
                    break;

                case 6:
                    currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
                    break;
            }

            switch (ComboFilter.SelectedIndex)
            {
                case 0:
                    currentAgents = currentAgents.Where(p => (Convert.ToInt32(p.Priority) >= 0)).ToList();
                    break;

                case 1:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentType.Title) == "МФО")).ToList();
                    break;

                case 2:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentType.Title) == "ООО")).ToList();
                    break;

                case 3:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentType.Title) == "ЗАО")).ToList();
                    break;

                case 4:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentType.Title) == "МКК")).ToList();
                    break;

                case 5:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentType.Title) == "ОАО")).ToList();
                    break;

                case 6:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentType.Title) == "ПАО")).ToList();
                    break;
            }

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            AgentListView.ItemsSource = currentAgents.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
