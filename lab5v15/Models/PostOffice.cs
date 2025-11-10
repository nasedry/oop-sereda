using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5v15.Models
{
    public class PostOffice
    {
        public string Name { get; set; }
        private List<Shipment> _shipments = new List<Shipment>();
        public IReadOnlyList<Shipment> Shipments => _shipments.AsReadOnly();

        public PostOffice(string name)
        {
            Name = name;
        }

        public void AddShipment(Shipment shipment) => _shipments.Add(shipment);

        public double AverageDeliveryDays()
        {
            if (!_shipments.Any()) return 0;
            return _shipments.Average(s => s.TransitDays);
        }

        public double LostPercentage()
        {
            if (!_shipments.Any()) return 0;
            return 100.0 * _shipments.Count(s => s.IsLost) / _shipments.Count;
        }

        public double DamagedPercentage()
        {
            if (!_shipments.Any()) return 0;
            return 100.0 * _shipments.Count(s => s.IsDamaged) / _shipments.Count;
        }

        public IEnumerable<string> TopDestinations(int topN)
        {
            return _shipments
                .GroupBy(s => s.Destination)
                .OrderByDescending(g => g.Count())
                .Take(topN)
                .Select(g => g.Key);
        }
    }
}
