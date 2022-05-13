using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateUpskill.Domain;

namespace NHibernateUpskill.Tests
{
    public abstract class NHibernateTestsBase : IDisposable
    {
        protected static readonly Random Random = new();
        protected const int KmToMeterMultiplier = 1000;
        protected const decimal KmToMile = 1.6m;

        protected const string HondaPlate = "BI 570CN";
        protected const string BmwPlate = "WWL 123XX";

        protected ISession Session;

        protected void SeedDb(bool restartDb = true)
        {
            if (restartDb is false) return;

            var sessionFactory = CreateSessionFactory();
            var session = Session = sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            var features = new List<Feature>()
            {
                new () { Id  = (int)FeatureKeys.AirConditioning, Name = "Air conditioning" },
                new () { Id  = (int)FeatureKeys.SeatBelts, Name = "Seat belts" },
                new () { Id  = (int)FeatureKeys.Aux, Name = "AUX" },
                new () { Id  = (int)FeatureKeys.ElectricWindows, Name = "Electric windows" },
            };

            foreach (var feature in features)
                session.SaveOrUpdate(feature);

            var featuresDict = features.ToDictionary(x => (FeatureKeys)x.Id);

            var car1 = new Car
            {
                Type = (int)CarTypeKeys.Honda,
                FuelIntakeCity = 10,
                FuelIntakeRoute = 7,
                PlateNumber = HondaPlate,
                Features = new List<Feature>
                {
                    featuresDict[FeatureKeys.Aux],
                    featuresDict[FeatureKeys.AirConditioning],
                },
                Mileage = new()
                {
                    Value = 200,
                    UnitOfMeasureId = (int)UnitOfMeasureKeys.Kilometers,
                },
                Journeys = new List<Journey>
                {
                    new()
                    {
                        StartedAt = new DateTime(2021, 12, 15)
                    },
                    new()
                    {
                        StartedAt = new DateTime(2020, 1, 1)
                    },
                    new()
                    {
                        StartedAt = new DateTime(2021, 12, 12),
                        Segments = new List<Segment>
                        {
                            new()
                            {
                                LengthMeters = 6_000,
                                IsCity = true,
                            },
                            new()
                            {
                                LengthMeters = 180_000,
                                IsCity = false,
                            },
                            new()
                            {
                                LengthMeters = 15_000,
                                IsCity = true,
                            },
                            new()
                            {
                                LengthMeters = 30_000,
                                IsCity = false,
                            },
                        }
                    }
                },
                Metadata = new Dictionary<string, string>
                {
                    { "niCategory", "A" },
                    { "test", "hehe" }
                }
            };

            var car2 = new Car
            {
                Type = (int)CarTypeKeys.Bmw,
                FuelIntakeCity = 15,
                FuelIntakeRoute = 11,
                PlateNumber = BmwPlate,
                Features = new List<Feature>
                {
                    featuresDict[FeatureKeys.Aux],
                    featuresDict[FeatureKeys.SeatBelts],
                    featuresDict[FeatureKeys.ElectricWindows],
                },
                Mileage = new()
                {
                    Value = 200,
                    UnitOfMeasureId = (int)UnitOfMeasureKeys.Miles,
                },
                Journeys = new List<Journey>
                {
                    new Journey()
                    {
                        StartedAt = new DateTime(2022, 1, 1),
                        Segments = new List<Segment>
                        {
                            new()
                            {
                                LengthMeters = 156_000,
                                IsCity = true,
                            }
                        }
                    }
                }
            };

            session.SaveOrUpdate(car1);
            session.SaveOrUpdate(car2);

            var commentFiles = new List<CommentFile>
            {
                new()
                {
                    FolderId = 1,
                    Name = "photo1.png",
                    Path = "/1/"
                },
                new()
                {
                    FolderId = 1,
                    Name = "photo2.png",
                    Path = "/1/"
                },
                new ()
                {
                    FolderId = 2,
                    Name = "dog.png",
                    Path = "/Desktop/2/"
                }
            };

            var articleFiles = new List<ArticleFile>()
            {
                new()
                {
                    FolderId = 1,
                    Name = "article.png",
                    Path = "/1/"
                },
                new()
                {
                    FolderId = 1,
                    Name = "dog.png",
                    Path = "/1/"
                },
            };

            foreach (var item in commentFiles)
                session.SaveOrUpdate(item);

            foreach (var item in articleFiles)
                session.SaveOrUpdate(item);

            transaction.Commit();
        }

        private static ISessionFactory CreateSessionFactory(bool exportSchema = true)
        {
            var connString = "Data Source=(LocalDB)\\MSSQLLocalDB;database=NHibernateFluentTests;Integrated Security=True";
            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connString))
                .Mappings(config => config.FluentMappings.AddFromAssemblyOf<Car>());

            if (exportSchema)
            {
                config.ExposeConfiguration(BuildSchema);
            }

            return config.BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            config.Cache(cfg => cfg.UseQueryCache = false);
            var export = new SchemaExport(config);
            export.Drop(false, true);
            export.Create(false, true);
        }

        ~NHibernateTestsBase()
        {
            Dispose();
        }

        public void Dispose()
        {
            Session?.Dispose();
        }
    }
}