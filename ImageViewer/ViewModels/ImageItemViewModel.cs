using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageViewer.ViewModels;

public partial class ImageItemViewModel : ViewModelBase
{
    [ObservableProperty] private string _fileName;
    [ObservableProperty] private string _resolution;
    [ObservableProperty] private string _size;
    [ObservableProperty] private string _fileFormat;
    [ObservableProperty] private DateTime _dateModified;
    
    private Bitmap? _imageSource; 
    private Bitmap? _fullImageSource;
    public Bitmap? ImageSource
    {
        get // advanced "get" huh 
        {
            if (_imageSource == null && System.IO.File.Exists(_fullFilePath))
            {
                try
                {
                    using (var stream = System.IO.File.OpenRead(_fullFilePath))
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
                if (!System.IO.File.Exists(_fullFilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"[XAML BUG] FIle not found with path: {_fullFilePath}");
                    return null;
                }

                try
                {
                    using (var stream = System.IO.File.OpenRead(_fullFilePath))
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
    
    public ImageItemViewModel(string filePath, string res, DateTime date, string size, string fileFormat)
    {
        FileName = System.IO.Path.GetFileName(filePath);
        Resolution = res;
        Size = size;
        FileFormat = _fileFormat;
        DateModified = date;
        _fullFilePath = filePath;
    }
}