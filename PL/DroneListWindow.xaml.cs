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
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneList.xaml
    /// </summary>
    /// 
/*    public class Drone
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsLecturer { get; set; }
    }*/
    public partial class DroneListWindow : Window
    {


        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        /*
                private ObservableCollection<Drone> _myCollection =
                  new ObservableCollection<Drone>();*/

        /*        public DroneListWindow()
                {
                    InitializeComponent();

                    DataContext = _myCollection;
                    _myCollection.Add(
                      new Drone
                      {
                          FirstName = "Arik",
                          LastName = "Poznanski",
                          IsLecturer = true
                      });
                    _myCollection.Add(
                      new Drone
                      {
                          FirstName = "John",
                          LastName = "Smith",
                          IsLecturer = false
                      });
                }

                private int counter = 0;
                private void Button_Click(object sender, RoutedEventArgs e)
                {
                    ++counter;
                    _myCollection.Add(
                        new Drone()
                        {
                            FirstName = "item " + counter,
                            LastName = "item " + counter,
                            IsLecturer = counter % 3 == 0
                        });
                }
            }*/
        /*}*/
        BLApi.IBL bl;

        /// <summary>
        /// Ctor of Drone list window
        /// </summary>
        /// <param name="blMain"></param>
        public DroneListWindow(BLApi.IBL blMain)
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            bl = blMain;
            DroneListView.ItemsSource = bl.GetDronesToList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// Filter the list category status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox status = sender as ComboBox;
            WeightCategories Sweight = 0;
            if (WeightSelector.SelectedItem == null)
            {
                DroneListView.ItemsSource = bl.GetDronesToListByCondition(drone => drone.Status == (DroneStatus)status.SelectedItem);
            }
            else
            {
                Sweight = (WeightCategories)WeightSelector.SelectedItem;
                DroneListView.ItemsSource = bl.GetDronesToListByCondition(drone => drone.Status == (DroneStatus)status.SelectedItem && drone.MaxWeight == Sweight);
            }
        }

        /// <summary>
        /// Filter the list category weight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox weight = sender as ComboBox;
            DroneStatus Sstatus = 0;
            if (StatusSelector.SelectedItem == null)
            {
                DroneListView.ItemsSource = bl.GetDronesToListByCondition(drone => drone.MaxWeight == (WeightCategories)weight.SelectedItem);
            }
            else
            {
                Sstatus = (DroneStatus)StatusSelector.SelectedItem;
                DroneListView.ItemsSource = bl.GetDronesToListByCondition(drone => drone.MaxWeight == (WeightCategories)weight.SelectedItem && drone.Status == Sstatus);
            }
        }

        /// <summary>
        /// Show drone window with update ctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDrone(object sender, MouseButtonEventArgs e)
        {
            BO.DroneToList drone = (sender as ListView).SelectedValue as BO.DroneToList;
            new DroneWindow(bl, bl.GetSpesificDrone(drone.ID)).Show();
            Close();
        }

        /// <summary>
        /// Show drone window with adding ctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
            Close();
        }

        /// <summary>
        /// Closeing window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseingWindow(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// Show all drones without filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneAllList(object sender, RoutedEventArgs e) => DroneListView.ItemsSource = bl.GetDronesToList();

        /// <summary>
        /// Refresh the drone list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshWindow(object sender, RoutedEventArgs e) => DroneListView.Items.Refresh();

        private void ReturnWindow(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private CollectionView view;

        private void GroupByStatus(object sender, RoutedEventArgs e)
        {
            ClearListView();
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void GroupByWeight(object sender, RoutedEventArgs e)
        {
            ClearListView();
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Weight");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ClearListView()
        {
            DroneListView.ItemsSource = bl.GetDronesToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
        }
    }
}
