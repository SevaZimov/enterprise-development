using CarWash.Domain;

namespace CarWash.Infrastructure.InMemory;

/// <summary>Примеры данных автомойки.</summary>
public sealed class CarWashSeed
{
    private static readonly TimeSpan WashOffset = TimeSpan.FromHours(4);

    public DateTimeOffset ReferenceTime { get; } = new(2026, 9, 14, 10, 0, 0, WashOffset);

    public IReadOnlyList<Client> Clients { get; } =
    [
        Client(1, "Иванов Алексей Сергеевич"),
        Client(2, "Петрова Мария Андреевна"),
        Client(3, "Сидоров Дмитрий Олегович"),
        Client(4, "Смирнова Анна Игоревна"),
        Client(5, "Кузнецов Павел Викторович"),
        Client(6, "Попова Елена Михайловна"),
        Client(7, "Васильев Артём Николаевич"),
        Client(8, "Фёдорова Ольга Романовна"),
        Client(9, "Михайлов Илья Денисович"),
        Client(10, "Новикова Дарья Алексеевна")
    ];

    public IReadOnlyList<Vehicle> Vehicles { get; } =
    [
        Vehicle(1, 1, "А101АА163", "Lada", VehicleCategory.Car),
        Vehicle(2, 1, "В202ВВ163", "Toyota", VehicleCategory.Car),
        Vehicle(3, 2, "Е303ЕЕ163", "GAZ", VehicleCategory.Truck),
        Vehicle(4, 3, "4004АВ163", "Honda", VehicleCategory.Motorcycle),
        Vehicle(5, 4, "К505КК163", "Kia", VehicleCategory.Car),
        Vehicle(6, 5, "М606ММ163", "Skoda", VehicleCategory.Car),
        Vehicle(7, 5, "Н707НН163", "Kamaz", VehicleCategory.Truck),
        Vehicle(8, 6, "8008АС163", "Yamaha", VehicleCategory.Motorcycle),
        Vehicle(9, 7, "С909СС163", "Renault", VehicleCategory.Car),
        Vehicle(10, 8, "Т010ТТ163", "Hyundai", VehicleCategory.Car)
    ];

    public IReadOnlyList<Service> Services { get; } =
    [
        Service(1, "Мойка кузова легкового автомобиля", 600m, 30, VehicleCategory.Car),
        Service(2, "Уборка салона легкового автомобиля", 800m, 20, VehicleCategory.Car),
        Service(3, "Комплексная мойка легкового автомобиля", 1500m, 60, VehicleCategory.Car),
        Service(4, "Мойка кузова грузового автомобиля", 1200m, 45, VehicleCategory.Truck),
        Service(5, "Комплексная мойка грузового автомобиля", 2500m, 90, VehicleCategory.Truck),
        Service(6, "Мойка мотоцикла", 400m, 20, VehicleCategory.Motorcycle),
        Service(7, "Мойка мотоцикла с защитным покрытием", 700m, 30, VehicleCategory.Motorcycle),
        Service(8, "Нанесение воска на легковой автомобиль", 500m, 15, VehicleCategory.Car),
        Service(9, "Мойка колёс легкового автомобиля", 300m, 15, VehicleCategory.Car),
        Service(10, "Уборка кабины грузового автомобиля", 900m, 30, VehicleCategory.Truck)
    ];

    public IReadOnlyList<Order> Orders { get; } =
    [
        Order(1, 5, 6, 3, new(2026, 9, 14, 10, 30, 0, WashOffset), 1),
        Order(2, 3, 4, 6, new(2026, 9, 14, 9, 0, 0, WashOffset), 3),
        Order(3, 1, 1, 1, new(2026, 9, 13, 9, 0, 0, WashOffset), 1),
        Order(4, 6, 8, 7, new(2026, 9, 14, 9, 50, 0, WashOffset), 4),
        Order(5, 2, 3, 4, new(2026, 9, 14, 8, 0, 0, WashOffset), 2),
        Order(6, 4, 5, 2, new(2026, 9, 14, 10, 0, 0, WashOffset), 1),
        Order(7, 1, 1, 1, new(2026, 9, 14, 9, 30, 0, WashOffset), 1),
        Order(8, 2, 3, 4, new(2026, 9, 14, 9, 40, 0, WashOffset), 2),
        Order(9, 1, 2, 1, new(2026, 9, 13, 10, 0, 0, WashOffset), 1),
        Order(10, 3, 4, 6, new(2026, 9, 14, 10, 0, 0, WashOffset), 3)
    ];

    private static Client Client(int id, string name) => new()
    {
        Id = id,
        FullName = name,
        PhoneNumber = $"+790000000{id:00}"
    };

    private static Vehicle Vehicle(int id, int clientId, string number, string brand, VehicleCategory category) => new()
    {
        Id = id,
        ClientId = clientId,
        GovNumber = number,
        Brand = brand,
        Category = category
    };

    private static Service Service(int id, string name, decimal price, int duration, VehicleCategory category) => new()
    {
        Id = id,
        Name = name,
        Price = price,
        DurationMinutes = duration,
        Category = category
    };

    private static Order Order(int id, int clientId, int vehicleId, int serviceId, DateTimeOffset start, int box) => new()
    {
        Id = id,
        ClientId = clientId,
        VehicleId = vehicleId,
        ServiceId = serviceId,
        ServiceStartTime = start,
        BoxNumber = box
    };
}
