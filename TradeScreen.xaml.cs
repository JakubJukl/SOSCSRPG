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
    /// Interakční logika pro TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        public GameSession Session => DataContext as GameSession;
        public TradeScreen()
        {
            InitializeComponent();
        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                Sell(groupedInventoryItem);
            }
        }

        private void OnClick_SellAll(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                while (1 < groupedInventoryItem.Quantity)
                {
                    Sell(groupedInventoryItem);
                }
                Sell(groupedInventoryItem);
            }
        }

        private void Sell(GroupedInventoryItem groupedInventoryItem)
        {
            Session.CurrentPlayer.ReceiveGold(groupedInventoryItem.Item.Price);
            Session.CurrentTrader.AddItemToInventory(groupedInventoryItem.Item);
            Session.CurrentPlayer.RemoveItemFromInventory(groupedInventoryItem.Item);
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                if (Session.CurrentPlayer.Gold >= groupedInventoryItem.Item.Price)
                {
                    Session.CurrentPlayer.SpendGold(groupedInventoryItem.Item.Price);
                    Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);
                    if (groupedInventoryItem.Item.Category == GameItem.ItemCategory.Spell) 
                    {
                        Session.CurrentPlayer.AddItemToInventory(SpellFactory.CreateSpell(groupedInventoryItem.Item.ItemTypeID));
                    }
                    else
                    {
                        Session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.Item);
                    }
                }
                else
                {
                    MessageBox.Show("Nemáš dost zlatých.");
                }
            }
        }

        private void OnClick_BuyAll(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
                if (Session.CurrentPlayer.Gold >= (groupedInventoryItem.Item.Price * groupedInventoryItem.Quantity))
                {
                    Session.CurrentPlayer.SpendGold(groupedInventoryItem.Item.Price * groupedInventoryItem.Quantity);
                    int quantita = groupedInventoryItem.Quantity;
                    while(1 < groupedInventoryItem.Quantity)
                    {
                        Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);
                    }
                    Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);

                    if (groupedInventoryItem.Item.Category == GameItem.ItemCategory.Spell)
                    {
                        Session.CurrentPlayer.AddItemToInventory(SpellFactory.CreateSpell(groupedInventoryItem.Item.ItemTypeID));
                    }
                    else
                    {
                        for (int i = 0; i < quantita; i++)
                        {
                            Session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.Item);
                        }       
                    }
                }
                else
                {
                    MessageBox.Show("Nemáš dost zlatých.");
                }
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
