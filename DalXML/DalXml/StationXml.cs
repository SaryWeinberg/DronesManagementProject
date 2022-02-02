﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    public partial class DalXml : IDal
    {
        /// <summary>
        /// Adding new station to DataBase
        /// </summary>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            List<Station> stationList = GetStations().ToList();
            stationList.Add(station);
            XmlTools.SaveListToXmlSerializer(stationList, direction + stationFilePath);
        }

        /// <summary>
        /// Update station in DataBase
        /// </summary>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            List<Station> stationList = GetStations().ToList();
            stationList[stationList.FindIndex(s => s.ID == station.ID)] = station;
            XmlTools.SaveListToXmlSerializer(stationList, direction + stationFilePath);
        }

        /// <summary>
        /// Returns a specific station by ID number
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetSpesificStation(int stationId)
        {
            try { return GetStations(s => s.ID == stationId).First(); }
            catch (Exception) { throw new ObjectDoesNotExist("Station", stationId); }
        }

        /// <summary>
        /// Returns the station list
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> condition = null)
        {
            condition ??= (s => true);
            return from station in XmlTools.LoadListFromXmlSerializer<Station>(direction + stationFilePath)
                   where condition(station)
                   select station;
        }        
    }
}