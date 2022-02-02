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
using System.Collections.Specialized;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneList.xaml
    /// </summary>
    /// 
    public partial class DroneListWindow : Window
    {
        BLApi.IBL bl;
        //DroneList droneList;
  /*      ListClass droneListClass;*/
        private ObservableCollection<BO.DroneToList> _myCollection = new ObservableCollection<BO.DroneToList>();


        /// <summary>
        /// 
        /// Ctor of Drone list window
        /// </summary>
        /// <param name="blMain"></param>
        public DroneListWindow(BLApi.IBL blMain)
        {

            InitializeComponent();
            WindowStyle = WindowStyle.None;
            bl = blMain;
           
            foreach (BO.DroneToList drone in bl.GetDronesToList())
            {
   
                _myCollection.Add(drone);
            }
            
            /* droneListClass = new ListClass(_myCollection);
             droneListClass.DroneList.CollectionChanged += ContentCollectionChanged;*/
            /*            _myCollection.CollectionChanged += ContentCollectionChanged;*/
            /*   DataContext = droneListClass.DroneList;*/
            DataContext = _myCollection;
            /*foreach (BO.DroneToList drone in _myCollection)
            {
                if (drone.Battery < 10) battery.Foreground = new SolidColorBrush(Colors.Red);
                else if (drone.Battery > 95) battery.Foreground = new SolidColorBrush(Colors.Green);
                else battery.Foreground = new SolidColorBrush(Colors.Yellow);
            }*/


            /*            droneList = new DroneList(bl.GetDronesToList());
 DroneListView.DataContext = droneList;*/
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
                DroneListView.ItemsSource = bl.GetDronesToList(drone => drone.Status == (DroneStatus)status.SelectedItem);
            }
            else
            {
                Sweight = (WeightCategories)WeightSelector.SelectedItem;
                DroneListView.ItemsSource = bl.GetDronesToList(drone => drone.Status == (DroneStatus)status.SelectedItem && drone.MaxWeight == Sweight);
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
                DroneListView.ItemsSource = bl.GetDronesToList(drone => drone.MaxWeight == (WeightCategories)weight.SelectedItem);
            }
            else
            {
                Sstatus = (DroneStatus)StatusSelector.SelectedItem;
                DroneListView.ItemsSource = bl.GetDronesToList(drone => drone.MaxWeight == (WeightCategories)weight.SelectedItem && drone.Status == Sstatus);
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
            DroneWindow open = new DroneWindow(bl, bl.GetSpesificDrone(drone.ID));
            open.SomeChangedHappened += UpdateObjectInTheList;
            open.Show();
            /*   Close();*/
        }

        /// <summary>
        /// Show drone window with adding ctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDrone(object sender, RoutedEventArgs e)
        {
            DroneWindow openWindow = new DroneWindow(bl);

            openWindow.SomeChangedHappened += AddDrone;
            openWindow.Show();


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
            new MainWindow(bl).Show();

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
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("MaxWeight");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ClearListView()
        {
            DroneListView.ItemsSource = bl.GetDronesToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
        }


        public void UpdateObjectInTheList(BO.Drone drone)
        {
            BO.DroneToList droneToList = _myCollection.First(d => d.ID == drone.ID);
            int idx = _myCollection.IndexOf(droneToList);
            _myCollection[idx] = new BO.DroneToList(drone);

            /*droneListClass.DroneList.First(d => d.ID == drone.ID).Model = drone.Model;*/
            /*    dtl = new BO.DroneToList(drone);*/
            /*   droneListClass.updateDroneList(drone);*/


        }
        public void AddDrone(BO.Drone drone)
        {
            /*droneListClass.DroneList.Add(new BO.DroneToList(drone));*/
            _myCollection.Add(new BO.DroneToList(drone));


        }



        public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            /*            {
                            foreach (BO.DroneToList item in e.OldItems)
                            {
                                //Removed items
                                item.PropertyChanged -= EntityViewModelPropertyChanged;
                            }
                        }
                        else if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach (EntityViewModel item in e.NewItems)
                            {
                                //Added items
                                item.PropertyChanged += EntityViewModelPropertyChanged;
                            }
                        }*/            /*if (e.Action == NotifyCollectionChangedAction.Remove)*/

        }
    }
}
/*    public class Log
    {
        private ObjectChanged objectChangedlll;

        public Log()
        {
            objectChangedlll = DroneListClass.UpdateList;
        }
        //public Log()
        //{
        //    ChangeColorType(true);
        //}
        public void update(bool update)
        {
            if (update)
            {
               
                objectChangedlll.Invoke("drone");


            }


        }

     

    }
}
/*public class CollectionViewModel : ViewModelBase
{
    public ObservableCollection<EntityViewModel> ContentList {
        get { return _contentList; }
    }*/

/*  public CollectionViewModel()
  {
      _contentList = new ObservableCollection<EntityViewModel>();
      _contentList.CollectionChanged += ContentCollectionChanged;
  }*/

/*    public void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (EntityViewModel item in e.OldItems)
            {
                //Removed items
                item.PropertyChanged -= EntityViewModelPropertyChanged;
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (EntityViewModel item in e.NewItems)
            {
                //Added items
                item.PropertyChanged += EntityViewModelPropertyChanged;
            }
        }
    }

    public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //This will get called when the property of an object inside the collection changes
    }
}*/