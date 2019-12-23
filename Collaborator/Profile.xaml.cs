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

namespace Collaborator
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        public Profile()
        {
            InitializeComponent();
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
    }
}
