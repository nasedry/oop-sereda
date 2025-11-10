using System;

namespace lab5v15.Models
{
    public class Shipment
    {
        public string Id { get; set; }
        public string Destination { get; set; }
        public DateTime DispatchDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsLost { get; set; }
        public bool IsDamaged { get; set; }

        public Shipment(string id, string destination, DateTime dispatch, DateTime delivery, bool lost = false, bool damaged = false)
        {
            if (delivery < dispatch)
                throw new Exceptions.InvalidShipmentDatesException("Дата доставки не може бути раніше дати відправлення.");

            Id = id;
            Destination = destination;
            DispatchDate = dispatch;
            DeliveryDate = delivery;
            IsLost = lost;
            IsDamaged = damaged;
        }

        public int TransitDays => (DeliveryDate - DispatchDate).Days;
    }
}
