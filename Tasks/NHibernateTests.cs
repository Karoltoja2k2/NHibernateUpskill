using System;
using FluentAssertions;
using NHibernate.Transform;
using NHibernateUpskill.Domain;
using Xunit;

namespace NHibernateUpskill.Tests
{
    public class NHibernateTests : NHibernateTestsBase
    {
        public NHibernateTests()
        {
            SeedDb();
        }

        [Fact]
        public void ShouldFilterCarByType()
        {
            var result = Session.QueryOver<Car>()
                .Where(x => x.Type == (int)CarTypeKeys.Honda)
                .List<Car>();

            result.Should().HaveCount(1);
            result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByType()
        {
            var query = $@"
select _car.PlateNumber
from Car _car
where _car.Type = {(int)CarTypeKeys.Honda}
";
            var result = Session.CreateQuery(query).List<string>();

            result.Should().HaveCount(1);
            result[0].Should().Be(HondaPlate);
        }

        [Fact]
        public void ShouldFilterCarByMileage()
        {
            const int kilometersLessThan = 250;
            var result = Session.QueryOver<Car>()
                .JoinQueryOver(x => x.Mileage)
                .Where(x => (x.Value < kilometersLessThan && x.UnitOfMeasureId == (int)UnitOfMeasureKeys.Kilometers) ||
                            (x.Value < kilometersLessThan / KmToMile && x.UnitOfMeasureId == (int)UnitOfMeasureKeys.Miles))
                .List<Car>();

            result.Should().HaveCount(1);
            result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByMileage()
        {
            var kilometersLessThan = 250;
            var query = $@"
select _car.PlateNumber
from Car _car
    inner join _car.Mileage _mileage
        with _mileage.Value < {kilometersLessThan}
group by _car.PlateNumber
";
            var result = Session.CreateQuery(query).List<string>();
            result.Should().HaveCount(1);
            result[0].Should().Be(HondaPlate);
        }

        [Fact]
        public void ShouldFilterCarByJourneyAndSegments()
        {
            var journeyStartedAfter = new DateTime(2021, 12, 13);
            var anySegmentLengthGreaterThan = 150_000;

            var result = Session.QueryOver<Car>()
                .JoinQueryOver<Journey>(x => x.Journeys)
                .Where(x => x.StartedAt > journeyStartedAfter)
                .JoinQueryOver<Segment>(x => x.Segments)
                .Where(x => x.LengthMeters > anySegmentLengthGreaterThan)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List<Car>();

            result.Should().HaveCount(1);
            result[0].PlateNumber.Should().Be(BmwPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByJourneyAndSegments()
        {
            var journeyStartedAfter = "2021-12-13 00:00:00";
            var anySegmentLengthGreaterThan = 150_000;
            var query = $@"
select _car.PlateNumber
from Car _car
    inner join _car.Journeys _journey
    inner join _journey.Segments _segment
where _journey.StartedAt > '{journeyStartedAfter}'
and _segment.LengthMeters > {anySegmentLengthGreaterThan}
group by _car.PlateNumber
";

            var result = Session.CreateQuery(query).List<string>();
            result.Should().HaveCount(1);
            result[0].Should().Be(BmwPlate);
        }

        [Fact]
        public void ShouldFilterBySegmentsAndFeatures()
        {
            var hasFeature = (int) FeatureKeys.SeatBelts;
            var anySegmentLengthGreaterThan = 150_000;

            Car carAlias = null;
            Feature featureAlias = null;
            Journey journeyAlias = null;
            Segment segmentAlias = null;

            var result = Session.QueryOver(() => carAlias)
                .JoinAlias(() => carAlias.Journeys, () => journeyAlias)
                .JoinAlias(() => journeyAlias.Segments, () => segmentAlias)
                .JoinAlias(() => carAlias.Features, () => featureAlias)
                .Where(x => featureAlias.Id == hasFeature)
                .Where(x => segmentAlias.LengthMeters > anySegmentLengthGreaterThan)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List<Car>();

            result.Should().HaveCount(1);
            result[0].PlateNumber.Should().Be(BmwPlate);
        }

        [Fact]
        public void HQL_ShouldFilterBySegmentsAndFeatures()
        {
            var hasFeature = (int)FeatureKeys.SeatBelts;
            var anySegmentLengthGreaterThan = 150_000;
            var query = $@"
select _car.PlateNumber
from Car _car
    inner join _car.Features _feature
        with _feature.Id = {hasFeature}
    inner join _car.Journeys _journey
    inner join _journey.Segments _segment
        with _segment.LengthMeters > {anySegmentLengthGreaterThan}
group by _car.PlateNumber
";
            var result = Session.CreateQuery(query).List<string>();
            result.Should().HaveCount(1);
            result[0].Should().Be(BmwPlate);
        }

        [Fact]
        public void HQL_ShouldSelectCarsWithMoreThanOneJourney()
        {
            var query = $@"
select car.PlateNumber
from Car as car
    left join car.Journeys as Journeys
group by car.PlateNumber
having count(Journeys) > 1
";
            var result = Session.CreateQuery(query).List<string>();

            result.Should().HaveCount(1);
            result[0].Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterWhereTotalLengthOfSegmentsInRouteExceedsValue()
        {
            var totalSegmentsLengthInCityIsGreaterThan = 150_000;
            var query = $@"
select _car.PlateNumber
from Car _car
    left join _car.Journeys _journeys
    left join _journeys.Segments _segments
        with _segments.IsCity = false
group by _car.PlateNumber
having sum(_segments.LengthMeters) > {totalSegmentsLengthInCityIsGreaterThan}
";

            var result = Session.CreateQuery(query).List<string>();
            result.Should().HaveCount(1);
            result[0].Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByMileage_AndShouldSelectOnlyCarWithoutReferences()
        {
            var mileageKilometersLessThan = 250;
            var query = $@"
";

            var result = Session.CreateQuery(query).List<Car>();
            result.Should().HaveCount(1);
            result[0].PlateNumber.Should().Be(HondaPlate);
            result[0].Mileage.Should().BeNull();
        }
    }
}
