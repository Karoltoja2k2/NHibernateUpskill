using System;
using System.Collections.Generic;

namespace NHibernateUpskill.Domain
{
    public enum CarTypeKeys
    {
        None = 0,

        Audi,
        Honda,
        Bmw
    }

    public enum FeatureKeys
    {
        None = 0,

        SeatBelts,
        Aux,
        AirConditioning,
        ElectricWindows
    }

    public enum UnitOfMeasureKeys
    {
        None = 0,

        Kilometers,
        Miles
    }

    public class Car
    {
        public Car()
        {
            
        }

        public Car(Guid id, string plateNumber, int type, decimal fuelIntakeCity, decimal fuelIntakeRoute)
        {
            Id = id;
            PlateNumber = plateNumber;
            Type = type;
            FuelIntakeCity = fuelIntakeCity;
            FuelIntakeRoute = fuelIntakeRoute;
        }

        public virtual Guid Id { get; set; }

        public virtual string PlateNumber { get; set; }

        public virtual int Type { get; set; }

        public virtual decimal FuelIntakeCity { get; set; }

        public virtual decimal FuelIntakeRoute { get; set; }

        public virtual CarMileage Mileage { get; set; }

        public virtual IList<Feature> Features { get; set; }

        public virtual IList<Journey> Journeys { get; set; }

        public virtual IDictionary<string, string> Metadata { get; set; }
    }

    public class CarMileage
    {
        public virtual Guid Id { get; set; }

        public virtual decimal Value { get; set; }

        public virtual int UnitOfMeasureId { get; set; }
    }

    public class Feature
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }

    public class Journey
    {
        public virtual Guid Id { get; set; }

        public virtual DateTime StartedAt { get; set; }

        public virtual IList<Segment> Segments { get; set; }

        public virtual Car Car { get; set; }
    }

    public class Segment
    {
        public virtual Guid Id { get; set; }

        public virtual int LengthMeters { get; set; }

        public virtual bool IsCity { get; set; }

        public virtual Journey Journey { get; set; }
    }
}
