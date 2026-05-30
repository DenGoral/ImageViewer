using System;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageViewer.ViewModels;

public partial class ImageItemViewModel : ViewModelBase
{
    [ObservableProperty] private string _fileName;
    [ObservableProperty] private string _resolution;
    [ObservableProperty] private string _size;
    [ObservableProperty] private DateTime _dateModified;
    
    private Bitmap? _imageSource; 
    private Bitmap? _fullImageSource;
    public Bitmap? ImageSource
    {
        get // advanced "get" huh 
        {
            if (_imageSource == null && File.Exists(_fullFilePath))
            {
                try
                {
                    using (var stream = File.OpenRead(_fullFilePath))
                    {
                        _imageSource = Bitmap.DecodeToHeight(stream, 120);
                    }
                }
                catch
                {
                    return null;
                }
            }
            return _imageSource;
        }
    }
    
    public Bitmap? FullImageSource
    {
        get
        {
            if (_fullImageSource == null)
            {
                if (!File.Exists(_fullFilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"[XAML BUG] FIle not found with path: {_fullFilePath}");
                    return null;
                }

                try
                {
                    using (var stream = File.OpenRead(_fullFilePath))
                    {
                        _fullImageSource = new Bitmap(stream);
                        System.Diagnostics.Debug.WriteLine($"[SUCCESS] Big image loaded: {FileName}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[EXCEPTION] Error downloading original: {ex.Message}");
                    return null;
                }
            }
            return _fullImageSource;
        }
    }
    
    private readonly string _fullFilePath;
    
    // will return empty string if _fullFilePath is null
    public string FolderPath => Path.GetDirectoryName(_fullFilePath) ?? string.Empty; 
    public string FileExtension => Path.GetExtension(_fullFilePath).Replace(".", " ").ToUpper(); 
    
    public ImageItemViewModel(string filePath, string res, DateTime date, string size)
    {
        FileName = Path.GetFileName(filePath);
        Resolution = res;
        Size = size;
        DateModified = date;
        _fullFilePath = filePath;
    }
}