using FluentNHibernate.Mapping;
using NHibernateUpskill.Domain;

namespace NHibernateUpskill.Mapping
{
    public class CarMapping : ClassMap<Car>
    {
        public CarMapping()
        {
            Id(x => x.Id);
            Map(x => x.Type).Not.Nullable();
            Map(x => x.PlateNumber).Not.Nullable().Length(9).Unique();
            Map(x => x.FuelIntakeCity).Not.Nullable();
            Map(x => x.FuelIntakeRoute).Not.Nullable();

            References(x => x.Mileage).Cascade.All().Column("MileageId");
            HasMany(x => x.Journeys).Cascade.All().KeyColumn("CarId");
            HasManyToMany(x => x.Features).Table("CarFeature").ChildKeyColumn("FeatureId").ParentKeyColumn("CarId");
        }
    }

    public class CarMileageMapping : ClassMap<CarMileage>
    {
        public CarMileageMapping()
        {
            Id(x => x.Id);
            Map(x => x.UnitOfMeasureId);
            Map(x => x.Value);
        }
    }

    public class JourneyMapping : ClassMap<Journey>
    {
        public JourneyMapping()
        {
            Id(x => x.Id);
            Map(x => x.StartedAt).Not.Nullable();
            HasMany(x => x.Segments).Cascade.All().KeyColumn("JourneyId");
        }
    }

    public class SegmentMapping : ClassMap<Segment>
    {
        public SegmentMapping()
        {
            Id(x => x.Id);
            Map(x => x.LengthMeters).Not.Nullable();
            Map(x => x.IsCity).Not.Nullable();
        }
    }

    public class FeatureMapping : ClassMap<Feature>
    {
        public FeatureMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name).Length(255).Not.Nullable();
        }
    }
}
