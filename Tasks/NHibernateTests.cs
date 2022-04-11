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

            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByType()
        {
            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void ShouldFilterCarByMileage()
        {
            // mileage can be as KM or MILES - differentiate by UnitOfMeasureId
            const int kilometersLessThan = 250;

            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByMileage()
        {
            // mileage can be as KM or MILES - differentiate by UnitOfMeasureId
            var kilometersLessThan = 250;
            var query = $@"
";
            var result = Session.CreateQuery(query).List<string>();
            result.Should().HaveCount(1);
            result[0].Should().Be(HondaPlate);
        }


        [Fact]
        public void ShouldFilterCarByJourneyAndSegments()
        {
            var journeyStartedAfter = new DateTime(2021, 12, 11);
            var anySegmentLengthGreaterThan = 10_000;

            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void HQL_ShouldFilterCarByJourneyAndSegments()
        {
            var journeyStartedAfter = new DateTime(2021, 12, 11);
            var anySegmentLengthGreaterThan = 10_000;

            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(HondaPlate);
        }

        [Fact]
        public void ShouldFilterBySegmentsAndFeatures_UsingAliases()
        {
            var hasFeature = (int) FeatureKeys.SeatBelts;
            var anySegmentLengthGreaterThan = 150_000;

            Car carAlias = null;
            Feature featureAlias = null;
            Journey journeyAlias = null;
            Segment segmentAlias = null;

            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(BmwPlate);
        }

        [Fact]
        public void HQL_ShouldFilterBySegmentsAndFeatures()
        {
            var hasFeature = (int)FeatureKeys.SeatBelts;
            var anySegmentLengthGreaterThan = 150_000;

            //result.Should().HaveCount(1);
            //result[0].PlateNumber.Should().Be(BmwPlate);
        }

        [Fact]
        public void HQL_ShouldSelectCarsWithMoreThanOneJourney()
        {
            var query = $@"
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
