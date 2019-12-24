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
using MySql.Data.MySqlClient;

namespace Collaborator
{
    /// <summary>
    /// Interaction logic for Password.xaml
    /// </summary>
    public partial class Password : Window
    {
        public Profile profile;
        String username = User.Instance.Name;
        public Password(Profile p)
        {
            profile=p;
            InitializeComponent();
        }
        private void Change_Password_Entered(object sender, MouseEventArgs e)
        {
            Change_Password.Background = new SolidColorBrush(Colors.Gray);
            Change_Password.Foreground = new SolidColorBrush(Colors.Black);

        }

        private void Change_Password_Left(object sender, MouseEventArgs e)
        {
            Color new_color = (Color)ColorConverter.ConvertFromString("#008080");
            Change_Password.Background = new SolidColorBrush(new_color);
            Change_Password.Foreground = new SolidColorBrush(Colors.White);
        }
        public void Update_Password(String New_Password)
        {
            DBConnection.Instance.Connect();


            String query = "UPDATE USER SET password='" + New_Password + "' WHERE username='" + username + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    mySqlCommand.ExecuteNonQuery();
                    Password_Update_Message.Text = "Successfully Updated!";
                    Password_Update_Message.Foreground = new SolidColorBrush(Colors.Green);

                }
            }
            catch (Exception e)
            {
                DBConnection.Instance.Connection = null;
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        public bool Check_Password(String Given_Password)
        {
            DBConnection.Instance.Connect();
            string query = "SELECT password FROM USER WHERE USERNAME='" + username + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                    if (dataReader.Read())
                    {
                        if (dataReader.GetString(0) == Given_Password)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                DBConnection.Instance.Connection = null;
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


        }
        private void Change_Password_Clicked(object sender, RoutedEventArgs e)
        {

            Password_Update_Message.Text = "";
            String Current_Password = Cur_Password.Password;
            String New_Password = N_Password.Password;
            String Confirm_Password = C_Password.Password;
            bool valid = Check_Password(Current_Password); ;

            if (New_Password == "")
            {
                Password_Update_Message.Text = "New password field can not be empty!";
                Password_Update_Message.Foreground = new SolidColorBrush(Colors.Red);
            }
            /* else if (New_Password!=Confirm_Password)
             {
                 Password_Update_Message.Text = "Sorry, confirm password and new password has not been matched!";
                 Password_Update_Message.Foreground = new SolidColorBrush(Colors.Red);

             }
             */
            else if (valid == false)
            {

                Password_Update_Message.Text = "Your password does not match with current password!";
                Password_Update_Message.Foreground = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Update_Password(New_Password);

            }
        }
        private void Back_Button_Clicked(object sender, RoutedEventArgs e)
        {

            profile.Show();
            this.Hide();
        }
    }
        
    }
