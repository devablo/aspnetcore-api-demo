using Devable.Demo.AspNetCore.API.Models.Domain;
using GenFu;
using System.Collections.Generic;
using System.Linq;

namespace Devable.Demo.AspNetCore.API.Services
{
    /// <summary>
    /// Dummy Station Service for sample API
    /// </summary>
    public class StationService : IStationService
    {
        private List<Station> Stations { get; set; }

        public StationService()
        {
            var i = 0;
            Stations = A.ListOf<Station>(50);
            Stations.ForEach(s =>
            {
                i++;
                s.Id = i;
            });
        }

        public IEnumerable<Station> GetAll()
        {
            return Stations;
        }

        public Station Get(int id)
        {
            return Stations.First(_ => _.Id == id);
        }

        public Station Add(Station station)
        {
            var newid = Stations.OrderBy(_ => _.Id).Last().Id + 1;
            station.Id = newid;

            Stations.Add(station);

            return station;
        }

        public void Update(int id, Station station)
        {
            var existing = Stations.First(_ => _.Id == id);
            existing.CallSign = station.CallSign;
            existing.City = station.City;
            existing.Code = station.Code;
            existing.State = station.State;
        }

        public void Delete(int id)
        {
            var existing = Stations.First(_ => _.Id == id);
            Stations.Remove(existing);
        }
    }

    public interface IStationService
    {
        IEnumerable<Station> GetAll();
        Station Get(int id);
        Station Add(Station station);
        void Update(int id, Station station);
        void Delete(int id);
    }
}
