using System.Collections.Generic;
using demo.Acme.Models;

namespace demo.Acme.Seeding
{
    public static class ProductsSeeder
    {
        public static class Id
        {
            public const string OneGenuineBoomerang = "khTM7wH6GX8P17hJ";
            public const string BirdSeedTrap = "NLecOowRusE4fbDR";
            public const string HitchHikerThumb = "UkwU7smTTKZUD4uq";
            public const string RocketPoweredRollerSkates = "hVFIplsCzedrPh58";
            public const string LegMuscleVitamins = "kOBty8EoLDMdddb9";
            public const string GiantMouseTrap = "X991s3bswHD5JZEl";
            public const string ArtificialRock = "fEMAbaT8hGYlzB6u";
            public const string ExplodingGolfBalls = "OoD6oyCdATHrTGEL";
            public const string DiyTornadoKit = "J1bQjFFEmpKOIXmS";
            public const string DehydratedBoulders = "wnO5YnTEZq8ujIFj";
            public const string RocketUnicycle = "K0akPEN4YLxvhYvs";
        }
        
        public static class Categories
        {
            public const string Traps = nameof(Traps);
            public const string Health = nameof(Health);
            public const string Supplements = nameof(Supplements);
            public const string Explosives = nameof(Explosives);
            public const string Weapons = nameof(Weapons);
            public const string Disguises = nameof(Disguises);
            public const string Vehicles = nameof(Vehicles);
            public const string Sport = nameof(Sport);
            public const string Gear = nameof(Gear);
        }
        
        public static IEnumerable<Product> GetProductsSeed()
            => new Product[]
            {
                new(Id.OneGenuineBoomerang)
                {
                    Name = "One Genuine Boomerang",
                    Description = "Guaranteed to always return to the hunter",
                    ProductCategories = new[] { Categories.Weapons },
                    Price = 19.95f,
                    AssetIds = new [] { AssetsSeeder.Id.OneGenuineBoomerang }
                },
                    
                new(Id.BirdSeedTrap)
                {
                    Name = "Bird Seed Trap",
                    Description = "A very attractive stop for hungry road runners!",
                    ProductCategories = new[] { Categories.Traps },
                    Price = 75,
                    AssetIds = new [] { AssetsSeeder.Id.BirdSeedTrap }
                },
                    
                new(Id.HitchHikerThumb)
                {
                    Name = "Hitch Hiker Thumb",
                    Description = "Increases your chances for getting a lift with 316%*",
                    ProductCategories = new[] { Categories.Traps },
                    Price = 9.95f,
                    AssetIds = new [] { AssetsSeeder.Id.HitchHikerThumb }
                },
                    
                new(Id.RocketPoweredRollerSkates)
                {
                    Name = "Rocket Powered Roller Skates",
                    Description = "Never be left in the dust again!",
                    ProductCategories = new[] { Categories.Sport, Categories.Gear },
                    Price = 129.95f,
                    AssetIds = new [] 
                    { 
                        AssetsSeeder.Id.RocketPoweredRollerSkates1, 
                        AssetsSeeder.Id.RocketPoweredRollerSkates2 
                    }
                },
                    
                new(Id.LegMuscleVitamins)
                {
                    Name = "Leg Muscle Vitamins",
                    Description = "Allows for some serious leg work",
                    ProductCategories = new[] { Categories.Health, Categories.Supplements },
                    Price = 39.99f,
                    AssetIds = new []
                    {
                        AssetsSeeder.Id.LegMuscleVitamins2, 
                        AssetsSeeder.Id.LegMuscleVitamins2
                    }
                },
                    
                new(Id.GiantMouseTrap)
                {
                    Name = "Giant Mouse Trap",
                    Description = "For even the largest of prey. Fits most types of bait",
                    ProductCategories = new[] { Categories.Traps },
                    Price = 89.99f,
                    AssetIds = new [] { AssetsSeeder.Id.GiantMouseTrap }
                },
                    
                new(Id.ArtificialRock)
                {
                    Name = "Artificial Rock",
                    Description = "Never stick out in rocky terrain again. Just add patience",
                    ProductCategories = new[] { Categories.Disguises, Categories.Traps },
                    Price = 89.99f,
                    AssetIds = new [] { AssetsSeeder.Id.ArtificialRock1 }
                },
                    
                new(Id.ExplodingGolfBalls)
                {
                    Name = "Exploding Golf Balls",
                    Description = "Invite prey to a day of leisure and let these babies do the rest",
                    ProductCategories = new[] { Categories.Weapons, Categories.Explosives },
                    Price = 29.99f,
                    AssetIds = new [] { AssetsSeeder.Id.ExplodingGolfBalls }
                },
                
                new(Id.DiyTornadoKit)
                {
                    Name = "Do-it-yourself Tornado Kit",
                    Description = "Take control of any situation with this amazing weather appliance",
                    ProductCategories = new[] { Categories.Weapons, Categories.Traps },
                    Price = 179f,
                    AssetIds = new [] { AssetsSeeder.Id.DiyTornadoKit }
                },
                
                new(Id.DehydratedBoulders)
                {
                    Name = "Dehydrated Boulders",
                    Description = "What the name says. Just add water",
                    ProductCategories = new[] { Categories.Disguises, Categories.Traps },
                    Price = 99.95f,
                    AssetIds = new [] { AssetsSeeder.Id.DehydratedBoulders }
                },
                
                new(Id.RocketUnicycle)
                {
                    Name = "Rocket Unicycle",
                    Description = "Very fast. Very nimble. Very dangerous",
                    ProductCategories = new[] { Categories.Vehicles, Categories.Explosives },
                    Price = 139,
                    AssetIds = new [] { AssetsSeeder.Id.RocketUnicycle }
                }      
            };

        public static IEnumerable<Category> GetProductCategoriesSeed()
        {
            yield return Categories.Traps;
            yield return Categories.Health;
            yield return Categories.Supplements;
            yield return Categories.Explosives;
            yield return Categories.Weapons;
            yield return Categories.Disguises;
            yield return Categories.Vehicles;
            yield return Categories.Sport;
            yield return Categories.Gear;
        }
    }
}