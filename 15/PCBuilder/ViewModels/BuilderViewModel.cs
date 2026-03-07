using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PCBuilder.Models;
using ReactiveUI;

namespace PCBuilder.ViewModels;

public class SelectedComponent : ReactiveObject
{
    public string Category { get; set; } = "";
    public string CategoryRu { get; set; } = "";
    public BasePart? Part { get; set; }
    public string DisplayName => Part?.Name ?? "Не выбрано";
    public string PriceDisplay => Part != null ? $"{Part.Price:N0} ₽" : "";
    public bool HasPart => Part != null;
    public string CategoryIcon => Category switch
    {
        "CPU" => "CPU",
        "Motherboard" => "MB",
        "RAM" => "RAM",
        "GPU" => "GPU",
        "Cooler" => "FAN",
        "PSU" => "PSU",
        "Case" => "BOX",
        "Storage" => "HDD",
        _ => "?"
    };
}

public class BuilderViewModel : ReactiveObject
{
    private readonly Func<AppDbContext> _dbFactory;

    public ObservableCollection<SelectedComponent> Components { get; } = new();

    private decimal _totalPrice;
    public decimal TotalPrice
    {
        get => _totalPrice;
        set => this.RaiseAndSetIfChanged(ref _totalPrice, value);
    }

    private string _compatibilityStatus = "";
    public string CompatibilityStatus
    {
        get => _compatibilityStatus;
        set => this.RaiseAndSetIfChanged(ref _compatibilityStatus, value);
    }

    private bool _isCompatible = true;
    public bool IsCompatible
    {
        get => _isCompatible;
        set => this.RaiseAndSetIfChanged(ref _isCompatible, value);
    }

    public string StatusIcon => IsCompatible ? "✓" : "⚠";

    public BuilderViewModel(Func<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;

        Components.Add(new SelectedComponent { Category = "CPU", CategoryRu = "Процессор" });
        Components.Add(new SelectedComponent { Category = "Motherboard", CategoryRu = "Материнская плата" });
        Components.Add(new SelectedComponent { Category = "RAM", CategoryRu = "Оперативная память" });
        Components.Add(new SelectedComponent { Category = "GPU", CategoryRu = "Видеокарта" });
        Components.Add(new SelectedComponent { Category = "Cooler", CategoryRu = "Кулер процессора" });
        Components.Add(new SelectedComponent { Category = "PSU", CategoryRu = "Блок питания" });
        Components.Add(new SelectedComponent { Category = "Case", CategoryRu = "Корпус" });
        Components.Add(new SelectedComponent { Category = "Storage", CategoryRu = "Накопитель" });
    }

    public void SetComponent(string category, BasePart? part)
    {
        var comp = Components.FirstOrDefault(c => c.Category == category);
        if (comp != null)
        {
            comp.Part = part;
            comp.RaisePropertyChanged(nameof(SelectedComponent.DisplayName));
            comp.RaisePropertyChanged(nameof(SelectedComponent.PriceDisplay));
            comp.RaisePropertyChanged(nameof(SelectedComponent.HasPart));
        }
        RecalculateTotal();
        CheckCompatibility();
    }

    public BasePart? GetComponent(string category)
    {
        return Components.FirstOrDefault(c => c.Category == category)?.Part;
    }

    private void RecalculateTotal()
    {
        TotalPrice = Components.Where(c => c.Part != null).Sum(c => c.Part!.Price);
    }

    private void CheckCompatibility()
    {
        var issues = new List<string>();

        using var db = _dbFactory();

        var cpuPart = GetComponent("CPU");
        var mbPart = GetComponent("Motherboard");
        var ramPart = GetComponent("RAM");
        var coolerPart = GetComponent("Cooler");
        var casePart = GetComponent("Case");
        var gpuPart = GetComponent("GPU");
        var psuPart = GetComponent("PSU");

        Cpu? cpu = null;
        Motherboard? mb = null;
        Ram? ram = null;
        ProcessorCooler? cooler = null;
        Case? pcCase = null;
        Gpu? gpu = null;
        PowerSupply? psu = null;

        if (cpuPart != null)
            cpu = db.Cpus.Include(x => x.Socket).FirstOrDefault(x => x.Id == cpuPart.Id);
        if (mbPart != null)
            mb = db.Motherboards.Include(x => x.Socket).Include(x => x.FormFactor).Include(x => x.MemoryType)
                    .FirstOrDefault(x => x.Id == mbPart.Id);
        if (ramPart != null)
            ram = db.Rams.Include(x => x.MemoryType).FirstOrDefault(x => x.Id == ramPart.Id);
        if (coolerPart != null)
            cooler = db.ProcessorCoolers.Include(x => x.SocketCompatibility).ThenInclude(x => x.Socket)
                        .FirstOrDefault(x => x.Id == coolerPart.Id);
        if (casePart != null)
            pcCase = db.Cases.Include(x => x.FormFactorCompatibility).ThenInclude(x => x.FormFactor)
                        .FirstOrDefault(x => x.Id == casePart.Id);
        if (gpuPart != null)
            gpu = db.Gpus.FirstOrDefault(x => x.Id == gpuPart.Id);
        if (psuPart != null)
            psu = db.PowerSupplies.FirstOrDefault(x => x.Id == psuPart.Id);

        if (cpu != null && mb != null)
        {
            if (cpu.SocketId != mb.SocketId)
                issues.Add($"Сокет процессора ({cpu.Socket?.Name}) не совпадает с сокетом материнской платы ({mb.Socket?.Name})");
        }

        if (cpu != null && cooler != null)
        {
            bool supported = cooler.SocketCompatibility.Any(s => s.SocketId == cpu.SocketId);
            if (!supported)
                issues.Add($"Кулер не поддерживает сокет процессора ({cpu.Socket?.Name})");
        }

        if (mb != null && pcCase != null)
        {
            bool supported = pcCase.FormFactorCompatibility.Any(f => f.FormFactorId == mb.FormFactorId);
            if (!supported)
                issues.Add($"Корпус не поддерживает форм-фактор материнской платы ({mb.FormFactor?.Name})");
        }

        if (mb != null && ram != null)
        {
            if (mb.MemoryTypeId != ram.MemoryTypeId)
                issues.Add($"Тип памяти материнской платы ({mb.MemoryType?.Name}) не совпадает с типом ОЗУ ({ram.MemoryType?.Name})");
        }

        if (gpu != null && psu != null)
        {
            if (gpu.RecommendPower > psu.Power)
                issues.Add($"Мощность блока питания ({psu.Power}Вт) недостаточна для видеокарты (рекомендуется {gpu.RecommendPower}Вт)");
        }

        IsCompatible = issues.Count == 0;
        CompatibilityStatus = issues.Count == 0
            ? (Components.All(c => c.Part == null) ? "" : "Все выбранные комплектующие совместимы")
            : string.Join("\n", issues);
        this.RaisePropertyChanged(nameof(StatusIcon));
    }

    public async Task<bool> SaveAssembly(string name, string author)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        var parts = Components.Where(c => c.Part != null).Select(c => c.Part!).ToList();
        if (parts.Count == 0) return false;

        using var db = _dbFactory();
        var assembly = new Assembly { Name = name, Author = author };
        db.Assemblies.Add(assembly);
        await db.SaveChangesAsync();

        foreach (var p in parts)
            db.PartAssemblies.Add(new PartAssembly { AssemblyId = assembly.Id, PartId = p.Id });
        await db.SaveChangesAsync();
        return true;
    }

    public void ClearAll()
    {
        foreach (var c in Components)
        {
            c.Part = null;
            c.RaisePropertyChanged(nameof(SelectedComponent.DisplayName));
            c.RaisePropertyChanged(nameof(SelectedComponent.PriceDisplay));
            c.RaisePropertyChanged(nameof(SelectedComponent.HasPart));
        }
        RecalculateTotal();
        CheckCompatibility();
    }
}
