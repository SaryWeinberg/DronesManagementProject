﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Drone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL bl;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blMain"></param>
        public DroneWindow(IBL.IBL blMain)
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            bl = blMain;
            string[] dataArr = { "ID", "max_weight", "model", "station_ID" };

            int x = 70;
            foreach (string item in dataArr)
            {
                Label droneItemT = new Label();
                droneItemT.Content = $"Enter drone {item}:";
                droneItemT.VerticalAlignment = VerticalAlignment.Top;
                droneItemT.Margin = new Thickness(43, x, 0, 0);
                DroneData.Children.Add(droneItemT);
                if (item == "max_weight")
                {
                    ComboBox droneItemC = new ComboBox();
                    droneItemC.Name = $"drone{item}";
                    droneItemC.VerticalAlignment = VerticalAlignment.Top;
                    droneItemC.Margin = new Thickness(199, x, 0, 0);
                    droneItemC.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                    DroneData.Children.Add(droneItemC);
                }
                else
                {
                    TextBox droneItem = new TextBox();
                    droneItem.Name = $"drone{item}";
                    droneItem.VerticalAlignment = VerticalAlignment.Top;
                    droneItem.Margin = new Thickness(199, x, 0, 0);
                    DroneData.Children.Add(droneItem);
                }
                x += 30;
            }

            Button sendNewDrone = new Button();
            sendNewDrone.Content = "Send";
            sendNewDrone.VerticalAlignment = VerticalAlignment.Top;
            sendNewDrone.Margin = new Thickness(43, x, 0, 0);
            sendNewDrone.Click += AddNewDrone;
            DroneData.Children.Add(sendNewDrone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            int weight = DroneData.Children.OfType<ComboBox>().First(txt => txt.Name == "dronemax_weight").SelectedIndex;
            string model = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "dronemodel").Text;
            string stationID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "dronestation_ID").Text;
            try
            {
                MessageBoxResult result =
                 MessageBox.Show(
                   bl.AddDroneBL(int.Parse(ID), model, (WeightCategories)weight, int.Parse(stationID)),
                   $"Add drone ID - {ID}",
                   MessageBoxButton.OK,
                   MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blMain"></param>
        /// <param name="drone"></param>
        public DroneWindow(IBL.IBL blMain, IBL.BO.DroneBL drone)
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;

            bl = blMain;

            string[] dataArr = { "ID", "max_weight", "model", "battery_status", "status", "longitude", "latitude" };
            int x = 70;
            foreach (string item in dataArr)
            {
                Label droneItemT = new Label();
                droneItemT.Content = $"Upadate drone {item}:";
                droneItemT.VerticalAlignment = VerticalAlignment.Top;
                droneItemT.Margin = new Thickness(43, x, 0, 0);
                DroneData.Children.Add(droneItemT);
                if (item == "max_weight" || item == "status")
                {
                    ComboBox droneItemC = new ComboBox();
                    droneItemC.Name = $"drone{item}";
                    droneItemC.VerticalAlignment = VerticalAlignment.Top;
                    droneItemC.Margin = new Thickness(199, x, 0, 0);
                    switch (item)
                    {
                        case "max_weight":
                            droneItemC.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                            droneItemC.Text = drone.MaxWeight.ToString();
                            droneItemC.IsEnabled = false;
                            droneItemC.IsEditable = true; break;
                        case "status":
                            droneItemC.ItemsSource = Enum.GetValues(typeof(Status));
                            droneItemC.Text = drone.Status.ToString();
                            droneItemC.IsEnabled = false;
                            droneItemC.IsEditable = true; break;
                    }
                    DroneData.Children.Add(droneItemC);
                }
                else
                {
                    TextBox droneItem = new TextBox();
                    droneItem.Name = $"drone{item}";
                    droneItem.VerticalAlignment = VerticalAlignment.Top;
                    droneItem.Margin = new Thickness(199, x, 0, 0);
                    switch (item)
                    {
                        case "ID":
                            droneItem.Text = drone.ID.ToString(); droneItem.IsEnabled = false; break;
                        case "model":
                            droneItem.Text = drone.Model.ToString(); break;
                        case "battery_status":
                            droneItem.Text = drone.BatteryStatus.ToString(); droneItem.IsEnabled = false; break;
                        case "longitude":
                            droneItem.Text = drone.Location.Longitude.ToString(); droneItem.IsEnabled = false; break;
                        case "latitude":
                            droneItem.Text = drone.Location.Latitude.ToString(); droneItem.IsEnabled = false; break;
                    }
                    DroneData.Children.Add(droneItem);
                }
                x += 30;
            }

            string[] bottons = { "close window", "update drone", "send drone to charge", "release Drone from Charge", "assign Parcel To Drone", "collect Parcel By Drone", "delivery Parcel By Drone" };
            foreach (string item in bottons)
            {
                Button botton = new Button();
                botton.Content = item;
                botton.VerticalAlignment = VerticalAlignment.Top;
                switch (item)
                {
                    case "close window": botton.Click += DataWindowClosing; break;
                    case "update drone": botton.Click += UpdateDrone; break;
                    case "send drone to charge": botton.Click += SendDroneToCharge; break;
                    case "release Drone from Charge": botton.Click += ReleaseDronefromCharge; break;
                    case "assign Parcel To Drone": botton.Click += AssignParcelToDrone; break;
                    case "collect Parcel By Drone": botton.Click += CollectParcelByDrone; break;
                    case "delivery Parcel By Drone": botton.Click += DeliveryParcelByDrone; break;
                }
                botton.Margin = new Thickness(43, x, 0, 0);
                DroneData.Children.Add(botton);
                x += 30;
            }
        }

        private void DataWindowClosing(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            string model = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "dronemodel").Text;
            try
            {
                MessageBoxResult result =
                  MessageBox.Show(
                    bl.UpdateDroneData(int.Parse(ID), model),
                    $"Update drone ID - {ID}",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                if(result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneToCharge(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBox.Show(bl.SendDroneToCharge(int.Parse(ID)));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseDronefromCharge(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBox.Show(bl.ReleaseDroneFromCharge(int.Parse(ID), 7));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignParcelToDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBox.Show(bl.AssignParcelToDrone(int.Parse(ID)));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectParcelByDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBox.Show(bl.CollectParcelByDrone(int.Parse(ID)));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeliveryParcelByDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBox.Show(bl.DeliveryParcelByDrone(int.Parse(ID)));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
