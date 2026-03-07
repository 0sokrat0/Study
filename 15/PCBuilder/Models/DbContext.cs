using Microsoft.EntityFrameworkCore;

namespace PCBuilder.Models;

public class Manufacturer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class PartType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class Socket
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class FormFactor
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class MemoryType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class CaseSize
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class FanDimension
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class GpuInterface
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class StorageDeviceInterface
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class StorageDeviceType
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class Certification
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class BasePart
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int ManufacturerId { get; set; }
    public Manufacturer? Manufacturer { get; set; }
    public int PartTypeId { get; set; }
    public PartType? PartType { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
}

public class Cpu
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int SocketId { get; set; }
    public Socket? Socket { get; set; }
    public int NumberOfCores { get; set; }
    public double BaseCoreFrequency { get; set; }
    public double MaxCoreFrequency { get; set; }
    public int CacheL3 { get; set; }
    public int ThermalPower { get; set; }
    public bool HasIGpu { get; set; }
}

public class Motherboard
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int SocketId { get; set; }
    public Socket? Socket { get; set; }
    public int FormFactorId { get; set; }
    public FormFactor? FormFactor { get; set; }
    public int MemorySlots { get; set; }
    public int MemoryTypeId { get; set; }
    public MemoryType? MemoryType { get; set; }
    public int PciSlots { get; set; }
    public int SataPorts { get; set; }
    public int UsbPorts { get; set; }
}

public class Gpu
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int GpuInterfaceId { get; set; }
    public GpuInterface? GpuInterface { get; set; }
    public double ChipFrequency { get; set; }
    public int VideoMemory { get; set; }
    public int MemoryBus { get; set; }
    public int RecommendPower { get; set; }
}

public class Ram
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int MemoryTypeId { get; set; }
    public MemoryType? MemoryType { get; set; }
    public int Capacity { get; set; }
    public int Count { get; set; }
    public double Ghz { get; set; }
    public string Timings { get; set; } = "";
}

public class ProcessorCooler
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int FanDimensionId { get; set; }
    public FanDimension? FanDimension { get; set; }
    public int HeatPipes { get; set; }
    public int MinSpeed { get; set; }
    public int MaxSpeed { get; set; }
    public double NoiseLevel { get; set; }
    public List<SocketProcessorCooler> SocketCompatibility { get; set; } = new();
}

public class SocketProcessorCooler
{
    public int Id { get; set; }
    public int SocketId { get; set; }
    public Socket? Socket { get; set; }
    public int ProcessorCoolerId { get; set; }
    public ProcessorCooler? ProcessorCooler { get; set; }
}

public class Case
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int SizeId { get; set; }
    public CaseSize? Size { get; set; }
    public int ExpansionSlots { get; set; }
    public int Fans { get; set; }
    public List<BoardFormFactorCase> FormFactorCompatibility { get; set; } = new();
}

public class BoardFormFactorCase
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public Case? Case { get; set; }
    public int FormFactorId { get; set; }
    public FormFactor? FormFactor { get; set; }
}

public class PowerSupply
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int Power { get; set; }
    public int FanDimensionId { get; set; }
    public FanDimension? FanDimension { get; set; }
    public int CertificationId { get; set; }
    public Certification? Certification { get; set; }
}

public class StorageDevice
{
    public int Id { get; set; }
    public BasePart? BasePart { get; set; }
    public int Capacity { get; set; }
    public int StorageDeviceInterfaceId { get; set; }
    public StorageDeviceInterface? StorageDeviceInterface { get; set; }
    public int StorageDeviceTypeId { get; set; }
    public StorageDeviceType? StorageDeviceType { get; set; }
}

public class Hdd
{
    public int Id { get; set; }
    public StorageDevice? StorageDevice { get; set; }
    public int RotationSpeed { get; set; }
}

public class Ssd
{
    public int Id { get; set; }
    public StorageDevice? StorageDevice { get; set; }
    public int Tbw { get; set; }
}

public class Assembly
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public List<PartAssembly> Parts { get; set; } = new();
}

public class PartAssembly
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public BasePart? Part { get; set; }
    public int AssemblyId { get; set; }
    public Assembly? Assembly { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<PartType> PartTypes => Set<PartType>();
    public DbSet<Socket> Sockets => Set<Socket>();
    public DbSet<FormFactor> FormFactors => Set<FormFactor>();
    public DbSet<MemoryType> MemoryTypes => Set<MemoryType>();
    public DbSet<CaseSize> CaseSizes => Set<CaseSize>();
    public DbSet<FanDimension> FanDimensions => Set<FanDimension>();
    public DbSet<GpuInterface> GpuInterfaces => Set<GpuInterface>();
    public DbSet<StorageDeviceInterface> StorageDeviceInterfaces => Set<StorageDeviceInterface>();
    public DbSet<StorageDeviceType> StorageDeviceTypes => Set<StorageDeviceType>();
    public DbSet<Certification> Certifications => Set<Certification>();
    public DbSet<BasePart> BaseParts => Set<BasePart>();
    public DbSet<Cpu> Cpus => Set<Cpu>();
    public DbSet<Motherboard> Motherboards => Set<Motherboard>();
    public DbSet<Gpu> Gpus => Set<Gpu>();
    public DbSet<Ram> Rams => Set<Ram>();
    public DbSet<ProcessorCooler> ProcessorCoolers => Set<ProcessorCooler>();
    public DbSet<SocketProcessorCooler> SocketProcessorCoolers => Set<SocketProcessorCooler>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<BoardFormFactorCase> BoardFormFactorCases => Set<BoardFormFactorCase>();
    public DbSet<PowerSupply> PowerSupplies => Set<PowerSupply>();
    public DbSet<StorageDevice> StorageDevices => Set<StorageDevice>();
    public DbSet<Hdd> Hdds => Set<Hdd>();
    public DbSet<Ssd> Ssds => Set<Ssd>();
    public DbSet<Assembly> Assemblies => Set<Assembly>();
    public DbSet<PartAssembly> PartAssemblies => Set<PartAssembly>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Cpu>().HasOne(x => x.BasePart).WithOne().HasForeignKey<Cpu>(x => x.Id);
        mb.Entity<Motherboard>().HasOne(x => x.BasePart).WithOne().HasForeignKey<Motherboard>(x => x.Id);
        mb.Entity<Gpu>().HasOne(x => x.BasePart).WithOne().HasForeignKey<Gpu>(x => x.Id);
        mb.Entity<Ram>().HasOne(x => x.BasePart).WithOne().HasForeignKey<Ram>(x => x.Id);
        mb.Entity<ProcessorCooler>().HasOne(x => x.BasePart).WithOne().HasForeignKey<ProcessorCooler>(x => x.Id);
        mb.Entity<Case>().HasOne(x => x.BasePart).WithOne().HasForeignKey<Case>(x => x.Id);
        mb.Entity<PowerSupply>().HasOne(x => x.BasePart).WithOne().HasForeignKey<PowerSupply>(x => x.Id);
        mb.Entity<StorageDevice>().HasOne(x => x.BasePart).WithOne().HasForeignKey<StorageDevice>(x => x.Id);
        mb.Entity<Hdd>().HasOne(x => x.StorageDevice).WithOne().HasForeignKey<Hdd>(x => x.Id);
        mb.Entity<Ssd>().HasOne(x => x.StorageDevice).WithOne().HasForeignKey<Ssd>(x => x.Id);

        mb.Entity<SocketProcessorCooler>()
            .HasOne(x => x.ProcessorCooler)
            .WithMany(x => x.SocketCompatibility)
            .HasForeignKey(x => x.ProcessorCoolerId);

        mb.Entity<BoardFormFactorCase>()
            .HasOne(x => x.Case)
            .WithMany(x => x.FormFactorCompatibility)
            .HasForeignKey(x => x.CaseId);

        mb.Entity<PartAssembly>()
            .HasOne(x => x.Assembly)
            .WithMany(x => x.Parts)
            .HasForeignKey(x => x.AssemblyId);

        SeedData(mb);
    }

    private static void SeedData(ModelBuilder mb)
    {
        mb.Entity<Manufacturer>().HasData(
            new Manufacturer { Id = 1,  Name = "AMD" },
            new Manufacturer { Id = 2,  Name = "Intel" },
            new Manufacturer { Id = 3,  Name = "NVIDIA" },
            new Manufacturer { Id = 4,  Name = "ASUS" },
            new Manufacturer { Id = 5,  Name = "MSI" },
            new Manufacturer { Id = 6,  Name = "Gigabyte" },
            new Manufacturer { Id = 7,  Name = "ASRock" },
            new Manufacturer { Id = 8,  Name = "Corsair" },
            new Manufacturer { Id = 9,  Name = "Kingston" },
            new Manufacturer { Id = 10, Name = "G.Skill" },
            new Manufacturer { Id = 11, Name = "Crucial" },
            new Manufacturer { Id = 12, Name = "Samsung" },
            new Manufacturer { Id = 13, Name = "Seagate" },
            new Manufacturer { Id = 14, Name = "Western Digital" },
            new Manufacturer { Id = 15, Name = "be quiet!" },
            new Manufacturer { Id = 16, Name = "Cooler Master" },
            new Manufacturer { Id = 17, Name = "NZXT" },
            new Manufacturer { Id = 18, Name = "Seasonic" },
            new Manufacturer { Id = 19, Name = "EVGA" },
            new Manufacturer { Id = 20, Name = "Noctua" },
            new Manufacturer { Id = 21, Name = "DeepCool" },
            new Manufacturer { Id = 22, Name = "Thermaltake" },
            new Manufacturer { Id = 25, Name = "Fractal Design" }
        );

        mb.Entity<PartType>().HasData(
            new PartType { Id = 1, Name = "CPU" },
            new PartType { Id = 2, Name = "GPU" },
            new PartType { Id = 3, Name = "RAM" },
            new PartType { Id = 4, Name = "Motherboard" },
            new PartType { Id = 5, Name = "Case" },
            new PartType { Id = 6, Name = "PowerSupply" },
            new PartType { Id = 7, Name = "ProcessorCooler" },
            new PartType { Id = 8, Name = "StorageDevice" }
        );

        mb.Entity<Socket>().HasData(
            new Socket { Id = 1, Name = "AM5" },
            new Socket { Id = 2, Name = "AM4" },
            new Socket { Id = 3, Name = "LGA1700" },
            new Socket { Id = 4, Name = "LGA1200" }
        );

        mb.Entity<FormFactor>().HasData(
            new FormFactor { Id = 1, Name = "ATX" },
            new FormFactor { Id = 2, Name = "Micro-ATX" },
            new FormFactor { Id = 3, Name = "Mini-ITX" }
        );

        mb.Entity<MemoryType>().HasData(
            new MemoryType { Id = 1, Name = "DDR5" },
            new MemoryType { Id = 2, Name = "DDR4" }
        );

        mb.Entity<CaseSize>().HasData(
            new CaseSize { Id = 1, Name = "Full Tower" },
            new CaseSize { Id = 2, Name = "Mid Tower" },
            new CaseSize { Id = 3, Name = "Mini Tower" }
        );

        mb.Entity<FanDimension>().HasData(
            new FanDimension { Id = 1, Name = "120 mm" },
            new FanDimension { Id = 2, Name = "135 mm" },
            new FanDimension { Id = 3, Name = "140 mm" }
        );

        mb.Entity<GpuInterface>().HasData(
            new GpuInterface { Id = 1, Name = "PCIe 4.0 x16" },
            new GpuInterface { Id = 2, Name = "PCIe 5.0 x16" }
        );

        mb.Entity<StorageDeviceInterface>().HasData(
            new StorageDeviceInterface { Id = 1, Name = "SATA III" },
            new StorageDeviceInterface { Id = 2, Name = "NVMe PCIe 4.0" },
            new StorageDeviceInterface { Id = 3, Name = "NVMe PCIe 5.0" }
        );

        mb.Entity<StorageDeviceType>().HasData(
            new StorageDeviceType { Id = 1, Name = "SSD" },
            new StorageDeviceType { Id = 2, Name = "HDD" }
        );

        mb.Entity<Certification>().HasData(
            new Certification { Id = 1, Name = "80 PLUS Bronze" },
            new Certification { Id = 2, Name = "80 PLUS Gold" },
            new Certification { Id = 3, Name = "80 PLUS Platinum" }
        );

        mb.Entity<BasePart>().HasData(
            new BasePart { Id = 1,  Name = "Ryzen 7 7800X3D OEM",                          ManufacturerId = 1,  PartTypeId = 1, Price = 33499 },
            new BasePart { Id = 2,  Name = "AMD Ryzen 5 7600",                              ManufacturerId = 1,  PartTypeId = 1, Price = 18999 },
            new BasePart { Id = 3,  Name = "Intel Core i5-14600K",                          ManufacturerId = 2,  PartTypeId = 1, Price = 28999 },
            new BasePart { Id = 4,  Name = "NVIDIA GeForce RTX 4070 SUPER 12GB",            ManufacturerId = 3,  PartTypeId = 2, Price = 69999 },
            new BasePart { Id = 5,  Name = "AMD Radeon RX 7800 XT 16GB",                    ManufacturerId = 1,  PartTypeId = 2, Price = 62999 },
            new BasePart { Id = 6,  Name = "NVIDIA GeForce RTX 4090 24GB",                  ManufacturerId = 3,  PartTypeId = 2, Price = 199999 },
            new BasePart { Id = 7,  Name = "Corsair Vengeance 32GB (2x16) DDR5-6000 CL30",  ManufacturerId = 8,  PartTypeId = 3, Price = 90000 },
            new BasePart { Id = 8,  Name = "Kingston Fury Beast 16GB (2x8) DDR4-3200 CL16", ManufacturerId = 9,  PartTypeId = 3, Price = 24900 },
            new BasePart { Id = 9,  Name = "ASUS TUF Gaming B650-PLUS (AM5, ATX)",          ManufacturerId = 4,  PartTypeId = 4, Price = 20999 },
            new BasePart { Id = 10, Name = "MSI PRO B760M-A WiFi (LGA1700, mATX)",          ManufacturerId = 5,  PartTypeId = 4, Price = 16999 },
            new BasePart { Id = 11, Name = "NZXT H5 Flow (Mid Tower)",                      ManufacturerId = 17, PartTypeId = 5, Price = 8999 },
            new BasePart { Id = 12, Name = "Fractal Design Meshify 2 (Full Tower)",          ManufacturerId = 25, PartTypeId = 5, Price = 14999 },
            new BasePart { Id = 13, Name = "Seasonic Focus GX-750 80+ Gold",                ManufacturerId = 18, PartTypeId = 6, Price = 12999 },
            new BasePart { Id = 14, Name = "be quiet! Straight Power 12 1000W 80+ Gold",    ManufacturerId = 15, PartTypeId = 6, Price = 21999 },
            new BasePart { Id = 15, Name = "Noctua NH-D15 chromax.black",                   ManufacturerId = 20, PartTypeId = 7, Price = 11999 },
            new BasePart { Id = 16, Name = "DeepCool AK400",                                ManufacturerId = 21, PartTypeId = 7, Price = 3499 },
            new BasePart { Id = 17, Name = "Samsung 990 PRO 1TB NVMe",                      ManufacturerId = 12, PartTypeId = 8, Price = 20999 },
            new BasePart { Id = 18, Name = "WD Black SN850X 2TB NVMe",                      ManufacturerId = 14, PartTypeId = 8, Price = 15999 },
            new BasePart { Id = 19, Name = "Seagate BarraCuda 4TB 5400RPM",                 ManufacturerId = 13, PartTypeId = 8, Price = 7499 },
            new BasePart { Id = 20, Name = "DeepCool CH160 PLUS",                           ManufacturerId = 21, PartTypeId = 5, Price = 5499 }
        );

        mb.Entity<Cpu>().HasData(
            new Cpu { Id = 1, SocketId = 1, NumberOfCores = 8,  BaseCoreFrequency = 4.2, MaxCoreFrequency = 5.0, CacheL3 = 96, ThermalPower = 120, HasIGpu = true },
            new Cpu { Id = 2, SocketId = 1, NumberOfCores = 6,  BaseCoreFrequency = 3.8, MaxCoreFrequency = 5.1, CacheL3 = 32, ThermalPower = 65,  HasIGpu = true },
            new Cpu { Id = 3, SocketId = 3, NumberOfCores = 14, BaseCoreFrequency = 3.5, MaxCoreFrequency = 5.3, CacheL3 = 24, ThermalPower = 125, HasIGpu = true }
        );

        mb.Entity<Motherboard>().HasData(
            new Motherboard { Id = 9,  SocketId = 1, FormFactorId = 1, MemorySlots = 4, MemoryTypeId = 1, PciSlots = 3, SataPorts = 4, UsbPorts = 8 },
            new Motherboard { Id = 10, SocketId = 3, FormFactorId = 2, MemorySlots = 4, MemoryTypeId = 1, PciSlots = 2, SataPorts = 4, UsbPorts = 8 }
        );

        mb.Entity<Gpu>().HasData(
            new Gpu { Id = 4, GpuInterfaceId = 1, ChipFrequency = 2475, VideoMemory = 12, MemoryBus = 192, RecommendPower = 750 },
            new Gpu { Id = 5, GpuInterfaceId = 1, ChipFrequency = 2124, VideoMemory = 16, MemoryBus = 256, RecommendPower = 750 },
            new Gpu { Id = 6, GpuInterfaceId = 1, ChipFrequency = 2235, VideoMemory = 24, MemoryBus = 384, RecommendPower = 850 }
        );

        mb.Entity<Ram>().HasData(
            new Ram { Id = 7, MemoryTypeId = 1, Capacity = 32, Count = 2, Ghz = 6000, Timings = "30-36-36-76" },
            new Ram { Id = 8, MemoryTypeId = 2, Capacity = 16, Count = 2, Ghz = 3200, Timings = "16-18-18-36" }
        );

        mb.Entity<ProcessorCooler>().HasData(
            new ProcessorCooler { Id = 15, FanDimensionId = 3, HeatPipes = 6, MinSpeed = 300,  MaxSpeed = 1500, NoiseLevel = 24.6 },
            new ProcessorCooler { Id = 16, FanDimensionId = 1, HeatPipes = 4, MinSpeed = 500,  MaxSpeed = 1850, NoiseLevel = 29.0 }
        );

        mb.Entity<SocketProcessorCooler>().HasData(
            new SocketProcessorCooler { Id = 1, SocketId = 1, ProcessorCoolerId = 15 },
            new SocketProcessorCooler { Id = 2, SocketId = 3, ProcessorCoolerId = 15 },
            new SocketProcessorCooler { Id = 3, SocketId = 1, ProcessorCoolerId = 16 },
            new SocketProcessorCooler { Id = 4, SocketId = 3, ProcessorCoolerId = 16 }
        );

        mb.Entity<Case>().HasData(
            new Case { Id = 11, SizeId = 2, ExpansionSlots = 7, Fans = 2 },
            new Case { Id = 12, SizeId = 1, ExpansionSlots = 8, Fans = 3 },
            new Case { Id = 20, SizeId = 3, ExpansionSlots = 4, Fans = 3 }
        );

        mb.Entity<BoardFormFactorCase>().HasData(
            new BoardFormFactorCase { Id = 1, CaseId = 11, FormFactorId = 1 },
            new BoardFormFactorCase { Id = 2, CaseId = 11, FormFactorId = 2 },
            new BoardFormFactorCase { Id = 3, CaseId = 11, FormFactorId = 3 },
            new BoardFormFactorCase { Id = 4, CaseId = 12, FormFactorId = 1 },
            new BoardFormFactorCase { Id = 5, CaseId = 12, FormFactorId = 2 },
            new BoardFormFactorCase { Id = 6, CaseId = 12, FormFactorId = 3 },
            new BoardFormFactorCase { Id = 7, CaseId = 20, FormFactorId = 2 },
            new BoardFormFactorCase { Id = 8, CaseId = 20, FormFactorId = 3 }
        );

        mb.Entity<PowerSupply>().HasData(
            new PowerSupply { Id = 13, Power = 750,  FanDimensionId = 2, CertificationId = 2 },
            new PowerSupply { Id = 14, Power = 1000, FanDimensionId = 2, CertificationId = 2 }
        );

        mb.Entity<StorageDevice>().HasData(
            new StorageDevice { Id = 17, Capacity = 1000, StorageDeviceInterfaceId = 2, StorageDeviceTypeId = 1 },
            new StorageDevice { Id = 18, Capacity = 2000, StorageDeviceInterfaceId = 2, StorageDeviceTypeId = 1 },
            new StorageDevice { Id = 19, Capacity = 4000, StorageDeviceInterfaceId = 1, StorageDeviceTypeId = 2 }
        );

        mb.Entity<Ssd>().HasData(
            new Ssd { Id = 17, Tbw = 600 },
            new Ssd { Id = 18, Tbw = 1200 }
        );

        mb.Entity<Hdd>().HasData(
            new Hdd { Id = 19, RotationSpeed = 5400 }
        );

        mb.Entity<Assembly>().HasData(
            new Assembly { Id = 1, Name = "Midrange Gaming 1440p (AM5 + RX 7800 XT)",         Author = "TestData" },
            new Assembly { Id = 2, Name = "Creator/Gaming 1440p (LGA1700 + RTX 4070 SUPER)",  Author = "TestData" },
            new Assembly { Id = 3, Name = "Enthusiast 4K (7800X3D + RTX 4090)",               Author = "TestData" },
            new Assembly { Id = 4, Name = "BuilderPC",                                         Author = "123" },
            new Assembly { Id = 5, Name = "Супер ультра мегу тупа жоская сборка хватит до 2030 года", Author = "Net2Fox" }
        );

        mb.Entity<PartAssembly>().HasData(
            new PartAssembly { Id = 1,  PartId = 2,  AssemblyId = 1 },
            new PartAssembly { Id = 2,  PartId = 5,  AssemblyId = 1 },
            new PartAssembly { Id = 3,  PartId = 9,  AssemblyId = 1 },
            new PartAssembly { Id = 4,  PartId = 7,  AssemblyId = 1 },
            new PartAssembly { Id = 5,  PartId = 11, AssemblyId = 1 },
            new PartAssembly { Id = 6,  PartId = 13, AssemblyId = 1 },
            new PartAssembly { Id = 7,  PartId = 16, AssemblyId = 1 },
            new PartAssembly { Id = 8,  PartId = 17, AssemblyId = 1 },
            new PartAssembly { Id = 9,  PartId = 19, AssemblyId = 1 },
            new PartAssembly { Id = 10, PartId = 3,  AssemblyId = 2 },
            new PartAssembly { Id = 11, PartId = 4,  AssemblyId = 2 },
            new PartAssembly { Id = 12, PartId = 10, AssemblyId = 2 },
            new PartAssembly { Id = 13, PartId = 7,  AssemblyId = 2 },
            new PartAssembly { Id = 14, PartId = 11, AssemblyId = 2 },
            new PartAssembly { Id = 15, PartId = 13, AssemblyId = 2 },
            new PartAssembly { Id = 16, PartId = 16, AssemblyId = 2 },
            new PartAssembly { Id = 17, PartId = 18, AssemblyId = 2 },
            new PartAssembly { Id = 18, PartId = 1,  AssemblyId = 3 },
            new PartAssembly { Id = 19, PartId = 6,  AssemblyId = 3 },
            new PartAssembly { Id = 20, PartId = 9,  AssemblyId = 3 },
            new PartAssembly { Id = 21, PartId = 7,  AssemblyId = 3 },
            new PartAssembly { Id = 22, PartId = 12, AssemblyId = 3 },
            new PartAssembly { Id = 23, PartId = 14, AssemblyId = 3 },
            new PartAssembly { Id = 24, PartId = 15, AssemblyId = 3 },
            new PartAssembly { Id = 25, PartId = 18, AssemblyId = 3 },
            new PartAssembly { Id = 26, PartId = 19, AssemblyId = 3 },
            new PartAssembly { Id = 27, PartId = 1,  AssemblyId = 4 },
            new PartAssembly { Id = 28, PartId = 9,  AssemblyId = 4 },
            new PartAssembly { Id = 29, PartId = 13, AssemblyId = 4 },
            new PartAssembly { Id = 30, PartId = 11, AssemblyId = 4 },
            new PartAssembly { Id = 31, PartId = 4,  AssemblyId = 4 },
            new PartAssembly { Id = 32, PartId = 15, AssemblyId = 4 },
            new PartAssembly { Id = 33, PartId = 7,  AssemblyId = 4 },
            new PartAssembly { Id = 34, PartId = 17, AssemblyId = 4 },
            new PartAssembly { Id = 35, PartId = 1,  AssemblyId = 5 },
            new PartAssembly { Id = 36, PartId = 9,  AssemblyId = 5 },
            new PartAssembly { Id = 37, PartId = 13, AssemblyId = 5 },
            new PartAssembly { Id = 38, PartId = 11, AssemblyId = 5 },
            new PartAssembly { Id = 39, PartId = 4,  AssemblyId = 5 },
            new PartAssembly { Id = 40, PartId = 15, AssemblyId = 5 },
            new PartAssembly { Id = 41, PartId = 7,  AssemblyId = 5 },
            new PartAssembly { Id = 42, PartId = 17, AssemblyId = 5 }
        );
    }
}
