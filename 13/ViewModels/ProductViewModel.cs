using System;
using System.IO;
using System.Reflection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using _13.Models;

namespace _13.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; }

        public ProductViewModel(Product product)
        {
            Product = product;
        }

        public string Name => Product.Name ?? string.Empty;
        public decimal Price => Product.Price;
        public string ImageUrl
        {
            get
            {
                var value = Product.ImageUrl?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return string.Empty;
                }

                if (value.StartsWith("avares://", StringComparison.OrdinalIgnoreCase) ||
                    value.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }

                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "13";
                var normalized = value.TrimStart('/', '\\').Replace('\\', '/');
                if (normalized.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
                {
                    return $"avares://{assemblyName}/{normalized}";
                }

                return $"avares://{assemblyName}/Assets/{normalized}";
            }
        }

        public Bitmap? Image
        {
            get
            {
                if (_image != null)
                {
                    return _image;
                }

                var uriString = ImageUrl;
                if (string.IsNullOrWhiteSpace(uriString))
                {
                    return null;
                }

                try
                {
                    if (uriString.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
                    {
                        var localPath = new Uri(uriString).LocalPath;
                        using var stream = File.OpenRead(localPath);
                        _image = new Bitmap(stream);
                        return _image;
                    }

                    var uri = new Uri(uriString);
                    using var assetStream = AssetLoader.Open(uri);
                    _image = new Bitmap(assetStream);
                    return _image;
                }
                catch
                {
                    return null;
                }
            }
        }

        private Bitmap? _image;
    }
}
