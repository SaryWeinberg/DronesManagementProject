﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// Adding new station to DataBase
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(Station station)
        {
            DataSource.Stations.Add(station);
        }

        /// <summary>
        /// Update station in DataBase
        /// </summary>
        /// <param name="station"></param>
        public void UpdateStation(Station station)
        {
            int index = DataSource.Stations.FindIndex(d => d.id == station.id);
            DataSource.Stations[index] = station;
        }

        /// <summary>
        /// Returns a specific station by ID number
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station GetSpesificStation(int stationId)
        {
            try
            {
                return DataSource.Stations.First(station => station.id == stationId);
            }
            catch
            {
                throw new ObjectDoesNotExist("Station", stationId);
            }
        }

        /// <summary>
        /// Returns the list of stations one by one
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStationLists()
        {
            foreach (Station station in DataSource.Stations)
            {
                yield return station;
            }
        }

        /// <summary>
        /// Returns the list of available stations one by one
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetAvailableStationLists()
        {
            foreach (Station station in DataSource.Stations)
            {
                int counter = 0;
                foreach (DroneCharge droneCharge in DataSource.DroneCharges)
                {
                    if (station.id == droneCharge.StationId)
                        counter++;
                }
                if (station.chargeSlots - counter > 0)
                {
                    yield return station;
                }
            }
        }

        /// <summary>
        /// Returns the station list
        /// </summary>
        /// <returns></returns>
        public List<Station> GetStations()
        {
            return DataSource.Stations;
        }
    }
}