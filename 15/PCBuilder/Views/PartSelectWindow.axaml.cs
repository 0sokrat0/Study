using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using PCBuilder.Models;
using PCBuilder.ViewModels;

namespace PCBuilder.Views;

public partial class PartSelectWindow : Window
{
    private Func<AppDbContext>? _dbFactory;
    private string _category = "";
    private List<PartListItem> _allItems = new();
    private List<string> _manufacturers = new();

    public PartSelectWindow()
    {
        InitializeComponent();
    }

    public PartSelectWindow(Func<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
        InitializeComponent();
    }

    public void SetCategory(string category)
    {
        _category = category;
        TitleLabel.Text = "Выбор: " + GetCategoryRu(category);
        Title = "Выбор: " + GetCategoryRu(category);
        LoadParts();
    }

    private void LoadParts()
    {
        if (_dbFactory == null) return;
        using var db = _dbFactory();
        _allItems = _category switch
        {
            "CPU" => LoadCpus(db),
            "Motherboard" => LoadMotherboards(db),
            "RAM" => LoadRams(db),
            "GPU" => LoadGpus(db),
            "Cooler" => LoadCoolers(db),
            "PSU" => LoadPsus(db),
            "Case" => LoadCases(db),
            "Storage" => LoadStorages(db),
            _ => new List<PartListItem>()
        };

        _manufacturers = new List<string> { "Все производители" };
        _manufacturers.AddRange(_allItems.Select(i => i.ManufacturerName).Distinct().OrderBy(x => x));
        ManufacturerFilter.ItemsSource = _manufacturers;
        ManufacturerFilter.SelectedIndex = 0;

        ApplyFilter();
    }

    private static List<PartListItem> LoadCpus(AppDbContext db)
    {
        return db.Cpus
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.Socket)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"Сокет: {x.Socket?.Name} | Ядра: {x.NumberOfCores} | {x.BaseCoreFrequency}/{x.MaxCoreFrequency} ГГц | TDP: {x.ThermalPower}Вт"
            }).ToList();
    }

    private static List<PartListItem> LoadMotherboards(AppDbContext db)
    {
        return db.Motherboards
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.Socket)
            .Include(x => x.FormFactor)
            .Include(x => x.MemoryType)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"Сокет: {x.Socket?.Name} | {x.FormFactor?.Name} | {x.MemoryType?.Name} x{x.MemorySlots} | PCIe: {x.PciSlots} | SATA: {x.SataPorts}"
            }).ToList();
    }

    private static List<PartListItem> LoadRams(AppDbContext db)
    {
        return db.Rams
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.MemoryType)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"{x.MemoryType?.Name} | {x.Capacity * x.Count}GB ({x.Count}x{x.Capacity}GB) | {x.Ghz} ГГц | CL: {x.Timings}"
            }).ToList();
    }

    private static List<PartListItem> LoadGpus(AppDbContext db)
    {
        return db.Gpus
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.GpuInterface)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"{x.GpuInterface?.Name} | VRAM: {x.VideoMemory}GB | Шина: {x.MemoryBus}bit | TDP: {x.RecommendPower}Вт"
            }).ToList();
    }

    private static List<PartListItem> LoadCoolers(AppDbContext db)
    {
        return db.ProcessorCoolers
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.FanDimension)
            .Include(x => x.SocketCompatibility).ThenInclude(s => s.Socket)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"Вентилятор: {x.FanDimension?.Name} | {x.MinSpeed}-{x.MaxSpeed} об/мин | {x.NoiseLevel} дБ | Сокеты: {string.Join(", ", x.SocketCompatibility.Select(s => s.Socket?.Name))}"
            }).ToList();
    }

    private static List<PartListItem> LoadPsus(AppDbContext db)
    {
        return db.PowerSupplies
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.FanDimension)
            .Include(x => x.Certification)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"Мощность: {x.Power}Вт | {x.Certification?.Name} | Вентилятор: {x.FanDimension?.Name}"
            }).ToList();
    }

    private static List<PartListItem> LoadCases(AppDbContext db)
    {
        return db.Cases
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.Size)
            .Include(x => x.FormFactorCompatibility).ThenInclude(f => f.FormFactor)
            .AsEnumerable()
            .Select(x => new PartListItem
            {
                Part = x.BasePart!,
                Name = x.BasePart!.Name,
                ManufacturerName = x.BasePart.Manufacturer?.Name ?? "",
                Price = x.BasePart.Price,
                Specs = $"{x.Size?.Name} | Форм-факторы: {string.Join(", ", x.FormFactorCompatibility.Select(f => f.FormFactor?.Name))} | Слоты: {x.ExpansionSlots} | Вентиляторы: {x.Fans}"
            }).ToList();
    }

    private static List<PartListItem> LoadStorages(AppDbContext db)
    {
        var items = new List<PartListItem>();
        var devices = db.StorageDevices
            .Include(x => x.BasePart).ThenInclude(x => x!.Manufacturer)
            .Include(x => x.StorageDeviceInterface)
            .Include(x => x.StorageDeviceType)
            .ToList();

        var hdds = db.Hdds.ToDictionary(h => h.Id, h => h.RotationSpeed);
        var ssds = db.Ssds.ToDictionary(s => s.Id, s => s.Tbw);

        foreach (var d in devices)
        {
            string extraSpec = "";
            if (hdds.TryGetValue(d.Id, out int rpm))
                extraSpec = $" | {rpm} об/мин";
            else if (ssds.TryGetValue(d.Id, out int tbw))
                extraSpec = $" | TBW: {tbw}";

            items.Add(new PartListItem
            {
                Part = d.BasePart!,
                Name = d.BasePart!.Name,
                ManufacturerName = d.BasePart.Manufacturer?.Name ?? "",
                Price = d.BasePart.Price,
                Specs = $"{d.StorageDeviceType?.Name} | {d.Capacity}GB | {d.StorageDeviceInterface?.Name}{extraSpec}"
            });
        }
        return items;
    }

    private void ApplyFilter()
    {
        var search = SearchBox.Text?.Trim().ToLowerInvariant() ?? "";
        var mfr = ManufacturerFilter.SelectedItem as string ?? "Все производители";

        var filtered = _allItems.Where(i =>
            (string.IsNullOrEmpty(search) || i.Name.ToLowerInvariant().Contains(search)) &&
            (mfr == "Все производители" || i.ManufacturerName == mfr)
        ).ToList();

        PartsList.ItemsSource = filtered;
        CountLabel.Text = $"Найдено: {filtered.Count}";
    }

    private void OnSearchChanged(object? sender, TextChangedEventArgs e) => ApplyFilter();
    private void OnManufacturerChanged(object? sender, SelectionChangedEventArgs e) => ApplyFilter();

    private void OnSelect(object? sender, RoutedEventArgs e)
    {
        if (PartsList.SelectedItem is PartListItem item)
            Close(item.Part);
    }

    private void OnDoubleTap(object? sender, TappedEventArgs e)
    {
        if (PartsList.SelectedItem is PartListItem item)
            Close(item.Part);
    }

    private void OnClose(object? sender, RoutedEventArgs e) => Close(null);

    private static string GetCategoryRu(string cat) => cat switch
    {
        "CPU" => "Процессор",
        "Motherboard" => "Материнская плата",
        "RAM" => "Оперативная память",
        "GPU" => "Видеокарта",
        "Cooler" => "Кулер процессора",
        "PSU" => "Блок питания",
        "Case" => "Корпус",
        "Storage" => "Накопитель",
        _ => cat
    };
}
