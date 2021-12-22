﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        public void AddStation(Station station);
        public void AddDrone(Drone drone);
        public void AddCustomer(Customer customer);
        public void AddParcel(Parcel parcel);
        public void AddDroneCharge(DroneCharge droneCharge);
        public void RemoveDroneInCharge(int droneId);
        public Station GetSpesificStation(int stationId);
        public Customer GetSpesificCustomer(int customerId);
        public Parcel GetSpesificParcel(int parcelId);
        public IEnumerable<Station> GetStationLists();
        public double[] ElectricalPowerRequest();
        List<Drone> GetDrones();
        List<Station> GetStations();
        List<Parcel> GetParcels();
        List<Customer> GetCustomers();
        public List<DroneCharge> GetDroneCharges();
        public void UpdateDrone(Drone drone);
        public void UpdateCustomer(Customer customer);
        public void UpdateStation(Station station);
        public void UpdateParcel(Parcel parcel);
        public IEnumerable<Parcel> GetParcelByCondition(Predicate<Parcel> conditin);
    }
}


