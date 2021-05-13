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
using Engine.Models;
using Engine.ViewModels;
using Engine.Factories;

namespace SOSCSRPG
{
    /// <summary>
    /// Interakční logika pro CustomizationScreen.xaml
    /// </summary>
    public partial class CustomizationScreen : Window
    {

        public GameSession Session => DataContext as GameSession;

        public CustomizationScreen()
        {
            InitializeComponent();
            Classy.ItemsSource = ClassFactory.classes;
        }


        private void OnClick_SaveName(object sender, RoutedEventArgs e)
        {
            Session.CurrentPlayer.Rename(NameBox.Text);
        }

        private void OnClick_SaveClass(object sender, RoutedEventArgs e)
        {
            if (Classy.Text != null)
            {
                Session.CurrentPlayer.ChangeClass(ClassFactory.GetClassByName(Classy.Text));
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
