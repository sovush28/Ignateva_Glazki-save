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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;

        private void UpdateAgents()
        {
            var currentAgents = ИгнатьеваГлазкиSaveEntities.GetContext().Agent.ToList();
            //var currentAgentTypes = ИгнатьеваГлазкиSaveEntities.GetContext().AgentType.ToList();

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
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentTypeStr) == "МФО")).ToList();
                    break;

                case 2:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentTypeStr) == "ООО")).ToList();
                    break;

                case 3:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentTypeStr) == "ЗАО")).ToList();
                    break;

                case 4:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentTypeStr) == "МКК")).ToList();
                    break;

                case 5:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentTypeStr) == "ОАО")).ToList();
                    break;

                case 6:
                    currentAgents = currentAgents.Where(p => (Convert.ToString(p.AgentTypeStr) == "ПАО")).ToList();
                    break;
            }

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.ToString().Replace(" ", "").Replace("-", "").Replace("+", "").Replace("(", "").Replace(")", "").
            Contains(TBoxSearch.Text.Replace(" ","").Replace("-","").Replace("+","").Replace("(","").Replace(")",""))
            || p.Email.ToLower().Replace(" ", "").Replace("@", "").Replace(".", "").
            Contains(TBoxSearch.Text.ToLower().Replace(" ", "").Replace("@", "").Replace(".", ""))
            ).ToList();

            AgentListView.ItemsSource = currentAgents;

            TableList = currentAgents;

            ChangePage(0, 0);
        }

        public AgentPage()
        {
            InitializeComponent();

            var currentAgents = ИгнатьеваГлазкиSaveEntities.GetContext().Agent.ToList();
            //var currentAgentTypes = ИгнатьеваГлазкиSaveEntities.GetContext().AgentType.ToList();
            AgentListView.ItemsSource = currentAgents;

            ComboSort.SelectedIndex = 0;
            ComboFilter.SelectedIndex = 0;

            UpdateAgents();
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

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean IfUpdate = true;

            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            IfUpdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            IfUpdate = false;
                        }
                        break;
                }
            }
            if (IfUpdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();

                AgentListView.ItemsSource = CurrentPageList;

                AgentListView.Items.Refresh();
            }

        }



        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent)); ;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility==Visibility.Visible)
            {
                ИгнатьеваГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().
                    ForEach(p => p.Reload());
                AgentListView.ItemsSource = ИгнатьеваГлазкиSaveEntities.GetContext().Agent.ToList();
                
                UpdateAgents();
            }
        }
    }
}
