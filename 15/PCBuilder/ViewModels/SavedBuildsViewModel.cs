using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PCBuilder.Models;
using ReactiveUI;

namespace PCBuilder.ViewModels;

public class AssemblyViewModel : ReactiveObject
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public List<PartViewModel> Parts { get; set; } = new();
    public decimal TotalPrice => Parts.Sum(p => p.Price);
    public string TotalPriceDisplay => $"{TotalPrice:N0} ₽";
    public string PartsCount => $"{Parts.Count} комплектующих";
}

public class PartViewModel
{
    public string Name { get; set; } = "";
    public string PartType { get; set; } = "";
    public decimal Price { get; set; }
    public string PriceDisplay => $"{Price:N0} ₽";
    public string ManufacturerName { get; set; } = "";
    public string DisplayLine => $"{PartType}: {Name}";
}

public class SavedBuildsViewModel : ReactiveObject
{
    private readonly Func<AppDbContext> _dbFactory;

    public ObservableCollection<AssemblyViewModel> Assemblies { get; } = new();

    private AssemblyViewModel? _selectedAssembly;
    public AssemblyViewModel? SelectedAssembly
    {
        get => _selectedAssembly;
        set => this.RaiseAndSetIfChanged(ref _selectedAssembly, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public SavedBuildsViewModel(Func<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task LoadAsync()
    {
        IsLoading = true;
        Assemblies.Clear();
        SelectedAssembly = null;

        using var db = _dbFactory();
        var assemblies = await db.Assemblies
            .Include(a => a.Parts)
                .ThenInclude(p => p.Part)
                    .ThenInclude(p => p!.Manufacturer)
            .Include(a => a.Parts)
                .ThenInclude(p => p.Part)
                    .ThenInclude(p => p!.PartType)
            .OrderByDescending(a => a.Id)
            .ToListAsync();

        foreach (var a in assemblies)
        {
            var avm = new AssemblyViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Author = a.Author,
                Parts = a.Parts.Select(p => new PartViewModel
                {
                    Name = p.Part?.Name ?? "—",
                    PartType = p.Part?.PartType?.Name ?? "—",
                    Price = p.Part?.Price ?? 0,
                    ManufacturerName = p.Part?.Manufacturer?.Name ?? "—"
                }).ToList()
            };
            Assemblies.Add(avm);
        }

        IsLoading = false;
    }

    public async Task DeleteAssemblyAsync(int id)
    {
        using var db = _dbFactory();
        var parts = db.PartAssemblies.Where(p => p.AssemblyId == id);
        db.PartAssemblies.RemoveRange(parts);
        var asm = await db.Assemblies.FindAsync(id);
        if (asm != null) db.Assemblies.Remove(asm);
        await db.SaveChangesAsync();
        await LoadAsync();
    }
}
