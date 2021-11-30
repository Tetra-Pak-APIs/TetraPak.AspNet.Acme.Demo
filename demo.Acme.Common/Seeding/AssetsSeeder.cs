using System.Collections.Generic;
using System.Net.Mime;
using demo.Acme.Models;

namespace demo.Acme.Seeding
{
    public static class AssetsSeeder
    {
        public static class Id
        {
            public const string OneGenuineBoomerang = "SrCG2QVUSaxFCUe1";
            public const string BirdSeedTrap = "WFbtaK2GbPqqbYxv";
            public const string HitchHikerThumb = "xxNzTLhkfjGeK4Hr";
            public const string RocketPoweredRollerSkates1 = "zO870aFXp468SRnw";
            public const string RocketPoweredRollerSkates2 = "17LYIRa3AToJbkDJ";
            public const string LegMuscleVitamins1 = "emkh3TbxCRe4rORv";
            public const string LegMuscleVitamins2 = "W6UsM9Wkg3rQZ4WH";
            public const string GiantMouseTrap = "8bN3sv6aRXbdEvFr";
            public const string ArtificialRock1 = "ssXy5eCMJcERgZxF";
            public const string ArtificialRock2 = "bReoBu2QQAECLhez";
            public const string ExplodingGolfBalls = "If1EmvOpRtfdcb78";
            public const string DiyTornadoKit = "WXvqigweIFd3V0NS";
            public const string DehydratedBoulders = "E7SQw0WEGimRdqnt";
            public const string RocketUnicycle = "sKioM3mRxc9jNGJV";
        }

        public const string MediaTypePng = "image/png";
        
        public static IEnumerable<Asset> GetAssetsSeed() =>
            new Asset[]
            {
                new(Id.OneGenuineBoomerang)
                {
                    Description = "Mr. Coyote about to throw One genuine boomerang",
                    Url = "https://4.bp.blogspot.com/-wxpq35CE6P0/Vv-KRGzcznI/AAAAAAABAvk/4uv48AmxYFMjAUy08vf3lfwYyoJ5zE_PQ/s1600/WILE%2BE%2BCOYOTE%2B%25281%2529.png",
                    MimeType = MediaTypePng
                },
                new(Id.BirdSeedTrap)
                {
                    Description = "Bird seed trap rigged on road, next to tree",
                    Url = "https://pbs.twimg.com/media/Df95CBrX0AA4sb5?format=jpg",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.HitchHikerThumb)
                {
                    Description = "Hitch hiker thumb on in box, marked as 'Approved'",
                    Url = "https://storage.googleapis.com/thehundreds/media/2018/09/acme-giant-hitch-hiker-thumb.jpg",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.RocketPoweredRollerSkates1)
                {
                    Description = "Product on furry feet, ready for use",
                    Url = "https://static.wikia.nocookie.net/looneytunes/images/7/74/RocketSkates.png/revision/latest/scale-to-width-down/1000?cb=20150112173336",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.RocketPoweredRollerSkates2)
                {
                    Description = "Mr. Coyote donning product, looking pleased",
                    Url = "https://pbs.twimg.com/media/DZ9ZNTrUMAANpEz?format=jpg",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.LegMuscleVitamins1)
                {
                    Description = "Predatory customer holding product package (opened)",
                    Url = "https://dyn1.heritagestatic.com/lf?set=path%5B1%2F8%2F3%2F0%2F0%2F18300759%5D%2Csizedata%5B850x600%5D&call=url%5Bfile%3Aproduct.chain%5D",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.LegMuscleVitamins2)
                {
                    Description = "Predatory customer flashing very strong legs",
                    Url = "https://i.ytimg.com/vi/kM-91sthsaE/hqdefault.jpg",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.GiantMouseTrap)
                {
                    Description = "Product rigged with bait",
                    Url = "https://storage.googleapis.com/thehundreds/media/2018/09/acme-looney-tunes-Giant_Mouse_Trap-1024x672.png",
                    MimeType = MediaTypePng
                },
                new(Id.ArtificialRock1)
                {
                    Description = "Opened product crate",
                    Url = "https://storage.googleapis.com/thehundreds/media/2018/09/artifial-rock-acme.png",
                    MimeType = MediaTypePng
                },
                new(Id.ArtificialRock2)
                {
                    Description = "Customer wearing product",
                    Url = "https://acme.com/catalog/acmeartificial.jpg",
                    MimeType = MediaTypeNames.Image.Jpeg
                }, 
                new(Id.ExplodingGolfBalls)
                {
                    Description = "Product explosion",
                    Url = "https://storage.googleapis.com/thehundreds/media/2018/09/Raw_Raw_Rooster_1956.jpg",
                    MimeType = MediaTypeNames.Image.Jpeg
                },
                new(Id.DiyTornadoKit)
                {
                    Description = "Opened product crate",
                    Url = "https://static.wikia.nocookie.net/looneytunes/images/0/0b/Tornado_Kit.png",
                    MimeType = MediaTypePng
                },
                new(Id.DehydratedBoulders)
                {
                    Description = "Opened product box",
                    Url = "https://static.wikia.nocookie.net/looneytunes/images/7/76/Dehydrated_Boudlers.png",
                    MimeType = MediaTypePng
                },
                new(Id.RocketUnicycle)
                {
                    Description = "Customer ready to light it up",
                    Url = "https://static.wikia.nocookie.net/looneytunes/images/1/13/Jet-Propelled_Unicycle.png",
                    MimeType = MediaTypePng
                }
            };
    }
}