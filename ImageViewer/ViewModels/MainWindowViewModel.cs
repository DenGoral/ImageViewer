using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ImageViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ImageItemViewModel> Images { get; } = new();
    
    [ObservableProperty] private ImageItemViewModel? _selectedImage;
    private string _folderPath;
    
    public MainWindowViewModel()
    { }

    public async Task OpenFolderAsync(Visual visual)
    {
        if (visual == null!) return;

        var topLevel = TopLevel.GetTopLevel(visual);
        if (topLevel == null) return;

        var result = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select a Folder",
            AllowMultiple = false
        });

        if (result.Count > 0)
        {
            _folderPath = result[0].Path.LocalPath;
        }
        
        // look through folderPath
        if (result.Count == 0) return;
        _folderPath = result[0].Path.LocalPath;

        Images.Clear(); // clear old images before load new
        
        foreach (string file in Directory.EnumerateFiles(_folderPath))
        {
            string ext = Path.GetExtension(file).ToLower();
            if (ext != ".jpg" && ext != ".png" && ext != ".jpeg") continue; 

            var info = new FileInfo(file);

            using (var stream = await Task.Run(() => File.OpenRead(file)))
            {
                var tempBitmap = new Avalonia.Media.Imaging.Bitmap(stream);
                string resolution = $"{tempBitmap.PixelSize.Width}x{tempBitmap.PixelSize.Height}";
        
                double sizeInMb = (double)info.Length / (1024 * 1024);
                string sizeStr = $"{Math.Round(sizeInMb, 2)} MB";

                Images.Add(new ImageItemViewModel(file, resolution, info.CreationTime, sizeStr));
            }
        }
    }
    
    // OPEN FILE (forgot 'bout it LOL)
    public async Task OpenFileAsync(Visual visual)
    {
        if (visual == null!) return;

        var topLevel = TopLevel.GetTopLevel(visual);
        if (topLevel == null) return;
        
        var result = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select an Image",
            AllowMultiple = false
        });

        if (result.Count == 0) return;

        string file = result[0].Path.LocalPath;

        if (Path.GetFileName(file).StartsWith("._")) return;

        string ext = Path.GetExtension(file).ToLower();
        if (ext != ".jpg" && ext != ".png" && ext != ".jpeg") return; 

        try
        {
            var info = new FileInfo(file);
        
            using (var stream = await Task.Run(() => File.OpenRead(file)))
            {
                var tempBitmap = new Avalonia.Media.Imaging.Bitmap(stream);
                string resolution = $"{tempBitmap.PixelSize.Width}x{tempBitmap.PixelSize.Height}";
            
                double sizeInMb = (double)info.Length / (1024 * 1024);
                string sizeStr = $"{Math.Round(sizeInMb, 2)} MB";

                var newItem = new ImageItemViewModel(file, resolution, info.CreationTime, sizeStr);

                Images.Add(newItem);
                SelectedImage = newItem;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка читання файлу {file}: {ex.Message}");
        }
    }
    
    
    // NEXT IMAGE METHOD 
    [RelayCommand]
    private void NextImage()
    {
        if (SelectedImage == null || Images.Count == 0) return;

        int currentIndex = Images.IndexOf(SelectedImage);

        if (currentIndex < Images.Count - 1)
        {
            SelectedImage = Images[currentIndex + 1];
        }
    }
    
    // Previous IMAGE METHOD 
    [RelayCommand]
    private void PreviousImage()
    {
        if (SelectedImage == null || Images.Count == 0) return;

        int currentIndex = Images.IndexOf(SelectedImage);

        if (currentIndex > 0)
        {
            SelectedImage = Images[currentIndex - 1];
        }
    }
}