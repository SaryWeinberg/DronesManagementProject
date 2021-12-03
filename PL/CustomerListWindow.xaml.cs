﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {        
        IBL.IBL bl;

        public CustomerListWindow(IBL.IBL blMain)
        {
            InitializeComponent();
            bl = blMain;
            CustomerListView.ItemsSource = bl.GetCustomersBL();
        }

        private void ViewCustomerWindow(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl).Show();
        }

        private void UpdateCustomer(object sender, MouseButtonEventArgs e)
        {
            IBL.BO.CustomerBL customer = (sender as ListView).SelectedValue as IBL.BO.CustomerBL;
            new CustomerWindow(bl, customer).Show();
        }
    }
}
