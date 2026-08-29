using Voyago.Models.Entities;
using Voyago.Models.Enums;
namespace Voyago.Data.Seed;
internal static class DemoData
{
    public static IReadOnlyList<Destination> Destinations() =>
    [
        D(1,"Costa Amalfitana","Italia","Campania","Costa y gastronomía",40.6340,14.6027,"Mayo a septiembre",7,true),
        D(2,"Kioto","Japón","Kansai","Cultura y bienestar",35.0116,135.7681,"Marzo a mayo",6,true),
        D(3,"Zermatt","Suiza","Valais","Montaña y esquí",46.0207,7.7491,"Diciembre a abril",5,true),
        D(4,"Serengeti","Tanzania","Mara","Safari privado",-2.3333,34.8333,"Junio a octubre",7,true),
        D(5,"Bora Bora","Polinesia Francesa","Islas de Sotavento","Islas y bienestar",-16.5004,-151.7415,"Mayo a octubre",6,true),
        D(6,"Patagonia","Chile","Magallanes","Aventura y naturaleza",-50.9423,-73.4068,"Noviembre a marzo",8,true),
        D(7,"Santorini","Grecia","Cícladas","Romance y gastronomía",36.3932,25.4615,"Abril a octubre",5,true),
        D(8,"Maldivas","Maldivas","Atolones centrales","Islas y buceo",3.2028,73.2207,"Noviembre a abril",7,true)
    ];
    private static Destination D(int id,string n,string c,string r,string cat,double lat,double lon,string season,int days,bool featured)=>new(){Id=id,Name=n,Country=c,Region=r,Category=cat,Tagline=$"Una experiencia privada en {n}",Description=$"Descubre {n} mediante una selección de experiencias, alojamientos y atención personalizada de nivel Voyago.",ImageUrl=$"/images/destinations/{id}.webp",Latitude=lat,Longitude=lon,BestSeason=season,RecommendedDuration=days,Featured=featured,Published=true};
    public static IReadOnlyList<TourPackage> Packages() => Enumerable.Range(1,10).Select(i=>new TourPackage{Id=i,DestinationId=((i-1)%8)+1,Title=$"Voyago Signature Journey {i}",DurationDays=5+(i%4),GroupSizeMax=8,TravelStyle=i%2==0?"Cultural Immersion":"Private Escape",PricePerPerson=6500m+i*725m,OriginalPrice=i%3==0?8000m+i*725m:null,Overview="Itinerario privado con asistencia especializada y servicios seleccionados.",ImageUrl=$"/images/packages/{i}.webp",AvailableSlots=4+i,Featured=i<=4,Published=true}).ToList();
    public static IReadOnlyList<PackageItineraryDay> ItineraryDays() => Enumerable.Range(1,10).SelectMany(p=>Enumerable.Range(1,3).Select(d=>new PackageItineraryDay{Id=(p-1)*3+d,TourPackageId=p,DayNumber=d,Title=$"Día {d}: experiencia seleccionada",Description="Programa privado con traslados coordinados y tiempo flexible."})).ToList();
    public static IReadOnlyList<PackageInclusion> Inclusions() => Enumerable.Range(1,10).SelectMany(p=>new[]{new PackageInclusion{Id=(p-1)*3+1,TourPackageId=p,Description="Traslados privados",IsIncluded=true,DisplayOrder=1},new PackageInclusion{Id=(p-1)*3+2,TourPackageId=p,Description="Asistencia concierge",IsIncluded=true,DisplayOrder=2},new PackageInclusion{Id=(p-1)*3+3,TourPackageId=p,Description="Gastos personales",IsIncluded=false,DisplayOrder=3}}).ToList();
    public static IReadOnlyList<Hotel> Hotels() => Enumerable.Range(1,8).Select(i=>new Hotel{Id=i,DestinationId=i,Name=$"Voyago Grand Retreat {i}",Description="Propiedad demostrativa de lujo con servicio personalizado.",Stars=5,Rating=4.5m+(i%5)*0.1m,PricePerNight=950m+i*175m,ImageUrl=$"/images/hotels/{i}.webp",Published=true}).ToList();
    public static IReadOnlyList<HotelRoomType> RoomTypes() => Enumerable.Range(1,8).SelectMany(h=>new[]{new HotelRoomType{Id=(h-1)*2+1,HotelId=h,Name="Deluxe Suite",Size=55m,BedType="King",Capacity=2,PricePerNight=1100m+h*150m},new HotelRoomType{Id=(h-1)*2+2,HotelId=h,Name="Private Villa",Size=120m,BedType="King and twin",Capacity=4,PricePerNight=2400m+h*200m}}).ToList();
    public static IReadOnlyList<FlightOffer> Flights() => Enumerable.Range(1,8).Select(i=>new FlightOffer{Id=i,FlightType=i%3==0?FlightType.PrivateCharter:FlightType.CommercialBusinessClass,Airline=$"Voyago Partner {i}",OriginCity="Santiago",OriginCode="SCL",DestinationCity=$"Destino {i}",DestinationCode=$"V{i:00}",Duration=TimeSpan.FromHours(6+i),Aircraft=i%3==0?"Gulfstream G650":"Boeing 787",CabinClass="Business",Baggage="2 piezas de 23 kg",Price=3200m+i*850m,CarbonOffsetIncluded=true,Published=true}).ToList();
    public static IReadOnlyList<Review> Reviews(string userId) => Enumerable.Range(1,10).Select(i=>new Review{Id=i,UserId=userId,DestinationId=((i-1)%8)+1,Rating=4+(i%2),Title=$"Experiencia Voyago {i}",Comment="Servicio atento, coordinación precisa y selección consistente con una experiencia de lujo.",VerifiedBooking=i%2==0,ModerationStatus=ReviewModerationStatus.Approved,CreatedAtUtc=new DateTime(2026,8,1,12,0,0,DateTimeKind.Utc).AddDays(i)}).ToList();
}
