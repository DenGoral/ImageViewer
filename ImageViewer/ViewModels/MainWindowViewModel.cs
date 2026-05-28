using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using SixLabors.ImageSharp;

namespace ImageViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ImageItemViewModel> Images { get; } = new();
    
    private string _folderPath;
    
    public MainWindowViewModel()
    {
    }

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
            
            using (var stream = File.OpenRead(file))
            {
                var imageInfo = Image.Identify(stream);
                if (imageInfo != null)
                {
                    string resolution = $"{imageInfo.Width}x{imageInfo.Height}";

                    Images.Add(new ImageItemViewModel(file, resolution, info.CreationTime, $"{info.Length / 1048576}"));
                }
            }
        }
    }
}