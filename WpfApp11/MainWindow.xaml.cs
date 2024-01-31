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

namespace WpfApp11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Manager manager = new Manager();
        private List<Person> personDataBase;
        private Button[] buttons;

        public MainWindow()
        {
            InitializeComponent();
            RefreshDataGrid();
            FillArray();
            ButtonSwitcher(false, 1, 2, 3);
        }

        private void FillArray()
        {
            buttons = new Button[5]
            {
                CreatePerson,          //0
                ConfirmButton,         //1
                SimpleAccountButton,   //2
                DepositAccountButton,  //3
                ConfirmTransfer        //4
            };
        }

        private void ButtonSwitcher(bool needToDisable, params int[] exceptions)
        {
            bool temp = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                for (int j = 0; j < exceptions.Length; j++)
                {
                    if (i == exceptions[j])
                    {
                        temp = true;
                        buttons[i].IsEnabled = needToDisable;
                    }
                }
                if (!temp) buttons[i].IsEnabled = !needToDisable;
                temp = false;
            }
        }

        private void TextBoxesCleaner()
        {
            FirstNameValue.Text = String.Empty;
            MiddleNameValue.Text = String.Empty;
            LastNameValue.Text = String.Empty;
            MobilePhoneValue.Text = String.Empty;
            PassportIDValue.Text = String.Empty;
            SimpleAccountText.Text = String.Empty;
            SimpleBalanceText.Text = String.Empty;
            DepositAccountText.Text = String.Empty;
            DepositBalanceText.Text = String.Empty;
            //OutAccountText.Text = String.Empty;
            //creditsValueText.Text = String.Empty;
            //InAccountText.Text = String.Empty;
        }

        private void RefreshDataGrid()
        {
            RefreshData();
            DataBaseGrid.ItemsSource = personDataBase;
        }

        private void RefreshData()
        {
            personDataBase = manager.GetPersonDataBase();
        }

        private void CreatePerson_Click(object sender, RoutedEventArgs e)
        {
            TextBoxesCleaner();
            ButtonSwitcher(true, 1);            

        }

        private void SimpleAccountButton_Click(object sender, RoutedEventArgs e)
        {
            manager.OpenBankAccount<SimpleBankAccount>(personDataBase[DataBaseGrid.SelectedIndex].ID);
            TextBoxesCleaner();
            ButtonSwitcher(false, 1, 2, 3);
            DataBaseGrid.SelectedIndex = -1;
            Tooltip.Text = "Успешно создано!";
        }

        private void DepositAccountButton_Click(object sender, RoutedEventArgs e)
        {
            manager.OpenBankAccount<DepositBankAccount>(personDataBase[DataBaseGrid.SelectedIndex].ID);
            TextBoxesCleaner();
            ButtonSwitcher(false, 1, 2, 3);
            DataBaseGrid.SelectedIndex = -1;
            Tooltip.Text = "Успешно создано!";
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(
                $"{FirstNameValue.Text}#" +
                $"{MiddleNameValue.Text}#" +
                $"{LastNameValue.Text}#" +
                $"{MobilePhoneValue.Text}#" +
                $"{PassportIDValue.Text}");
            string[] dataSpl = stringBuilder.ToString().Split('#');
            manager.CreatePerson(dataSpl);
            ButtonSwitcher(false, 1);
            RefreshDataGrid();
            Tooltip.Text = "Успешно создано!";
            TextBoxesCleaner();
            DataBaseGrid.SelectedIndex = -1;
        }

        private void DataBaseGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataBaseGrid.SelectedIndex > -1 && DataBaseGrid.SelectedIndex < personDataBase.Count && personDataBase.Count > 0)
            {
                TextBoxesCleaner();
                Tooltip.Text = String.Empty;
                FirstNameValue.Text = personDataBase[DataBaseGrid.SelectedIndex].FirstName;
                MiddleNameValue.Text = personDataBase[DataBaseGrid.SelectedIndex].MiddleName;
                LastNameValue.Text = personDataBase[DataBaseGrid.SelectedIndex].LastName;
                MobilePhoneValue.Text = personDataBase[DataBaseGrid.SelectedIndex].MobilePhone;
                PassportIDValue.Text = personDataBase[DataBaseGrid.SelectedIndex].PassportID;
                FillAccountFields();
                //SimpleAccountText.Text = manager.GetAccountByIDAndType
                //    (personDataBase[DataBaseGrid.SelectedIndex].ID, 0).GetValue.AccountNumber;
                //DepositAccountText.Text = manager.GetAccountByIDAndType
                //    (personDataBase[DataBaseGrid.SelectedIndex].ID, 1).GetValue.AccountNumber;
                SimpleAccountButton.IsEnabled =
                    !manager.IsAccountExists<SimpleBankAccount>(personDataBase[DataBaseGrid.SelectedIndex].ID);
                DepositAccountButton.IsEnabled =
                    !manager.IsAccountExists<DepositBankAccount>(personDataBase[DataBaseGrid.SelectedIndex].ID);
            }
            
        }

        private void FillAccountFields()
        {
            if (manager.GetAccountByIDAndType
                    (personDataBase[DataBaseGrid.SelectedIndex].ID, 0) != null)
            {
                SimpleAccountText.Text =
                    manager.GetAccountByIDAndType
                    (personDataBase[DataBaseGrid.SelectedIndex].ID, 0).GetValue.AccountNumber;
                SimpleBalanceText.Text =
                    manager.GetAccountByIDAndType
                    (personDataBase[DataBaseGrid.SelectedIndex].ID, 0).GetValue.AccountBalance.ToString();
            }
                
            if (manager.GetAccountByIDAndType
                    (personDataBase[DataBaseGrid.SelectedIndex].ID, 1) != null)
            {
                DepositAccountText.Text = manager.GetAccountByIDAndType
                    (personDataBase[DataBaseGrid.SelectedIndex].ID, 1).GetValue.AccountNumber;
                DepositBalanceText.Text =
                    manager.GetAccountByIDAndType
                    (personDataBase[DataBaseGrid.SelectedIndex].ID, 1).GetValue.AccountBalance.ToString();
            }
                

        }

        private void SimpleAccountText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DepositAccountText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OutAccountText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void creditsValueText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InAccountText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ConfirmTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(OutAccountText.Text) && 
                !String.IsNullOrWhiteSpace(InAccountText.Text) &&
                !String.IsNullOrWhiteSpace(creditsValueText.Text))
            {
                if (float.TryParse(creditsValueText.Text, out float result))
                {
                    if (manager.CashTransfer(OutAccountText.Text, InAccountText.Text, result))
                        Tooltip.Text = "Перевод успешно завершен!";
                    else Tooltip.Text = "Недостаточно средств!";
                }
                else Tooltip.Text = "Неверная сумма перевода!";
            }
            else
            {
                Tooltip.Text = "Не все поля заполнены!";
            }
        }

        private void ConfirmAddCredits_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(InAccountText.Text) &&
                !String.IsNullOrWhiteSpace(creditsValueText.Text))
            {
                if (float.TryParse(creditsValueText.Text, out float result))
                {
                    if (manager.CashTransfer(String.Empty, InAccountText.Text, result))
                        Tooltip.Text = "Перевод успешно завершен!";
                    else Tooltip.Text = "Недостаточно средств!";
                }
                else Tooltip.Text = "Неверная сумма перевода!";
            }
            else
            {
                Tooltip.Text = "Не все поля заполнены!";
            }
        }

        private void CloseSimpleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(SimpleAccountText.Text))
            {
                manager.CloseBankAccount(SimpleAccountText.Text);
                SimpleAccountText.Text = String.Empty;
                SimpleBalanceText.Text = String.Empty;
                Tooltip.Text = "Счет успешно закрыт!";
            }
        }

        private void CloseDepositButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(DepositAccountText.Text))
            {
                manager.CloseBankAccount(DepositAccountText.Text);
                DepositAccountText.Text = String.Empty;
                DepositBalanceText.Text = String.Empty;
                Tooltip.Text = "Депозит успешно закрыт!";
            }
        }
    }
}
