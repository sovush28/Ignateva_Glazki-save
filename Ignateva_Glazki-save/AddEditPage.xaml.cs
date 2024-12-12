using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent _currentAgent = new Agent();

        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();

            if(SelectedAgent != null)
            {
                _currentAgent = SelectedAgent;
                ComboType.SelectedIndex = _currentAgent.AgentTypeID - 1;
            }
            DataContext = _currentAgent;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog myOpenFileDialog = new OpenFileDialog();

            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");

            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (_currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            else
            {
                if (_currentAgent.INN.Length != 10)
                    errors.AppendLine("Длина ИНН должна быть равна 10 символам");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            else
            {
                if (_currentAgent.KPP.Length != 9)
                    errors.AppendLine("Длина КПП должна быть равна 9 символам");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = _currentAgent.Phone.
                    Replace("(", "").Replace("-", "").Replace("+", "").
                    Replace(")","").Replace(" ","");
                if (ph.Length < 11 || ph.Length > 12)
                {
                    errors.AppendLine("Длина телефона должна быть равна 11 или 12 символам");
                }
                else
                {
                    if ((ph.Length < 11 || ph.Length > 12) &&
                    ((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) ||
                    (ph[1] == '3' && ph.Length != 12))
                        errors.AppendLine("Укажите правильно телефон агента");
                }
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
           
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentAgent.AgentTypeID = ComboType.SelectedIndex + 1;

            if (_currentAgent.ID == 0)
                ИгнатьеваГлазкиSaveEntities.GetContext().Agent.Add(_currentAgent);

            try
            {
                ИгнатьеваГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentRealizedProduct = ИгнатьеваГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentRealizedProduct = currentRealizedProduct.Where(p => p.AgentID == _currentAgent.ID).ToList();

            var currentAgentPriorityHistory = ИгнатьеваГлазкиSaveEntities.GetContext().AgentPriorityHistory.ToList();
            currentAgentPriorityHistory = currentAgentPriorityHistory.Where(p => p.AgentID == _currentAgent.ID).ToList();

            var currentShop = ИгнатьеваГлазкиSaveEntities.GetContext().Shop.ToList();
            currentShop = currentShop.Where(p => p.AgentID == _currentAgent.ID).ToList();

            if (currentRealizedProduct.Count != 0)
            {
                MessageBox.Show("Удаление агента запрещено, так как у агента есть информация о реализации продукции.");
            }
            else
            {
                if(MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ИгнатьеваГлазкиSaveEntities.GetContext().Agent.Remove(_currentAgent);

                        if (currentAgentPriorityHistory.Count > 0)
                        {
                            for(int i = 0; i == currentAgentPriorityHistory.Count; i++)
                            {
                                ИгнатьеваГлазкиSaveEntities.GetContext().AgentPriorityHistory.Remove(currentAgentPriorityHistory[i]);
                            }
                        }

                        if (currentShop.Count > 0)
                        {
                            for(int i=0;i==currentShop.Count; i++)
                            {
                                ИгнатьеваГлазкиSaveEntities.GetContext().Shop.Remove(currentShop[i]);
                            }
                        }

                        ИгнатьеваГлазкиSaveEntities.GetContext().SaveChanges();

                        Manager.MainFrame.GoBack();

                    }

                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
    }
}
