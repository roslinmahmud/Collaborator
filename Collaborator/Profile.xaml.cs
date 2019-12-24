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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public string username = User.Instance.UserName;
        public string fullname = User.Instance.Name;
        public Dashboard dashboard;

        public Profile(Dashboard d)
        {
            dashboard=d;
            InitializeComponent();
            Show_Details();
        }
        public void Show_Details()
        {
            UserName.Text = username;
            Profile_Name.Text = fullname;


        }
        private void Change_Photo_Button_Entered(object sender, MouseEventArgs e)
        {
            ChangePictureButton.Background = new SolidColorBrush(Colors.Gray);
            ChangePictureButton.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Change_Photo_Button_Left(object sender, MouseEventArgs e)
        {
            Color new_color = (Color)ColorConverter.ConvertFromString("#008080");
            ChangePictureButton.Background = new SolidColorBrush(new_color);
            ChangePictureButton.Foreground = new SolidColorBrush(Colors.White);
        }
        private void Submit_Button_Entered(object sender, MouseEventArgs e)
        {
            SubmitButton.Background = new SolidColorBrush(Colors.Gray);
            SubmitButton.Foreground = new SolidColorBrush(Colors.Black);

        }

        private void Submit_Button_Left(object sender, MouseEventArgs e)
        {
            Color new_color = (Color)ColorConverter.ConvertFromString("#008080");
            SubmitButton.Background = new SolidColorBrush(new_color);
            SubmitButton.Foreground = new SolidColorBrush(Colors.White);
        }
        private void Set_Password_Entered(object sender, MouseEventArgs e)
        {
            Set_Password.Background = new SolidColorBrush(Colors.Gray);
            Set_Password.Foreground = new SolidColorBrush(Colors.Black);

        }

        private void Set_Password_Left(object sender, MouseEventArgs e)
        {
            Color new_color = (Color)ColorConverter.ConvertFromString("#008080");
            Set_Password.Background = new SolidColorBrush(new_color);
            Set_Password.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Submit_Button_Clicked(object sender, RoutedEventArgs e)
        {
            String New_Username = UserName.Text;
            String New_Fullname = Profile_Name.Text;
            bool validity = true;

            if (New_Username == "" || New_Fullname == "")
            {
                validity = false;
                Update_Message.Text = "Username field or full name field can't be empty!";
                Update_Message.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (New_Username != username && validity)
            {
                Update_Username(New_Username);
            }
            if (New_Fullname != fullname && validity)
            {
                Update_Fullname(New_Fullname);
            }




        }
        public void Update_Fullname(String New_Fullname)
        {
            DBConnection.Instance.Connect();
            // String query="UPDATE USER SET id, username, name, photo_path, ip FROM USER WHERE USERNAME='" + username + "' AND PASSWORD='" + password + "';";


            String query = "UPDATE USER SET name='" + New_Fullname + "' WHERE username='" + username + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    mySqlCommand.ExecuteNonQuery();
                    Update_Message.Text = "Successfully Updated!";
                    Update_Message.Foreground = new SolidColorBrush(Colors.Green);


                }
            }
            catch (Exception e)
            {
                DBConnection.Instance.Connection = null;
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }
        public void Update_Username(String New_Username)
        {
            DBConnection.Instance.Connect();

            String query = "UPDATE USER SET username='" + New_Username + "' WHERE username='" + username + "';";
            try
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(query, DBConnection.Instance.Connection))
                {
                    if (mySqlCommand.ExecuteNonQuery() > 0)
                    {

                        Update_Message.Text = "Successfully Updated!";
                        Update_Message.Foreground = new SolidColorBrush(Colors.Green);
                    }


                }
            }
            catch (Exception e)
            {
                DBConnection.Instance.Connection = null;
                MessageBox.Show(e.Message, this.ToString() + " Perform() Exception", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void Set_Password_Clicked(object sender, RoutedEventArgs e)
        {
            Password obj = new Password(this);
            obj.Show();
            this.Hide();
        }
    }
}
