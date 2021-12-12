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


            Button CancelButton = new Button();
            CancelButton.Content = "Cancel";
            CancelButton.VerticalAlignment = VerticalAlignment.Top;
            CancelButton.Margin = new Thickness(43, x + 30, 0, 0);
            CancelButton.Click += DataWindowClosing;
            DroneData.Children.Add(CancelButton);
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
                    Close();
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

            string[] dataArr = { "ID", "max_weight", "battery_status", "status", "longitude", "latitude" };
            int x = 100;


            Label DroneModel = new Label();
            DroneModel.Content = $"Upadate drone model:";
            DroneModel.VerticalAlignment = VerticalAlignment.Top;
            DroneModel.Margin = new Thickness(43, 70, 0, 0);
            DroneData.Children.Add(DroneModel);
            TextBox dronemodel = new TextBox();
            dronemodel.Name = "dronemodel";
            dronemodel.VerticalAlignment = VerticalAlignment.Top;
            dronemodel.Margin = new Thickness(199, 70, 0, 0);
            dronemodel.Text = drone.Model.ToString();
            dronemodel.TextChanged += AddUpdateBTN;
            DroneData.Children.Add(dronemodel);
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
            position = 400;
            AddBTN("close window");
            if (drone.Status == DroneStatus.Available)
            {
                AddBTN("send drone to charge");
                AddBTN("assign Parcel To Drone");
            }
            else if (drone.Status == DroneStatus.Maintenance)
            {
                AddBTN("release Drone from Charge");

            }
            else if (drone.Status == DroneStatus.Delivery)
            {
                if (bl.GetSpesificParcelBL(drone.Parcel.ID).PickedUp == null)
                {
                    AddBTN("collect Parcel By Drone");
                }
                else
                {
                    AddBTN("delivery Parcel By Drone");
                }
            }
        }
        static int position = 400;
        private void AddBTN(string item)
        {

            Button botton = new Button();
            botton.Content = item;
            botton.VerticalAlignment = VerticalAlignment.Top;
            switch (item)
            {
                case "close window": botton.Click += DataWindowClosing; break;
                case "send drone to charge": botton.Click += SendDroneToCharge; break;
                case "release Drone from Charge":
                    botton.Click += ReleaseDronefromCharge;

                    timecharge.Text = "write time of charge here:";
                    timecharge.Margin = new Thickness(19, position, 0, 0);
                    timecharge.Visibility = Visibility.Visible;

                    break;
                case "assign Parcel To Drone": botton.Click += AssignParcelToDrone; break;
                case "collect Parcel By Drone": botton.Click += CollectParcelByDrone; break;
                case "delivery Parcel By Drone": botton.Click += DeliveryParcelByDrone; break;
            }
            botton.Margin = new Thickness(43, position, 0, 0);
            DroneData.Children.Add(botton);
            position += 30;
        }


        private void AddUpdateBTN(object sender, RoutedEventArgs e)
        {
            Button botton = new Button();
            botton.Content = "update";
            botton.VerticalAlignment = VerticalAlignment.Top;
            botton.Click += UpdateDrone;
            botton.Margin = new Thickness(43, 250, 0, 0);
            DroneData.Children.Add(botton);
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
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                    Close();
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
                MessageBoxResult result =
                MessageBox.Show(
                  bl.SendDroneToCharge(int.Parse(ID)),
                  $"Send drone ID - {ID} to charge",
                  MessageBoxButton.OK,
                  MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                    Close();
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
        private void ReleaseDronefromCharge(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBoxResult result =
                MessageBox.Show(
                  bl.ReleaseDroneFromCharge(int.Parse(ID), int.Parse(timecharge.Text)),
                  $"Release drone ID - {ID} from charge",
                  MessageBoxButton.OK,
                  MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                    Close();
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
        private void AssignParcelToDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBoxResult result =
                 MessageBox.Show(
                 bl.AssignParcelToDrone(int.Parse(ID)),
                 $"Assign parcel",
                 MessageBoxButton.OK,
                 MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                    Close();
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
        private void CollectParcelByDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBoxResult result =
                MessageBox.Show(
                bl.CollectParcelByDrone(int.Parse(ID)),
                $"Collect parcel",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                    Close();
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
        private void DeliveryParcelByDrone(object sender, RoutedEventArgs e)
        {
            string ID = DroneData.Children.OfType<TextBox>().First(txt => txt.Name == "droneID").Text;
            try
            {
                MessageBoxResult result =
               MessageBox.Show(
               bl.DeliveryParcelByDrone(int.Parse(ID)),
               $"Delivery parcel",
               MessageBoxButton.OK,
               MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    new DroneListWindow(bl).Show();
                    Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

    }
}
