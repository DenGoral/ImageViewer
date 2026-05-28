using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageViewer.ViewModels;

public partial class ImageItemViewModel : ViewModelBase
{
    [ObservableProperty] private string _fileName;
    [ObservableProperty] private string _resolution;
    [ObservableProperty] private DateTime _dateModified;
    [ObservableProperty] private Bitmap? _imageSource; 
    
    // only for tests!!!
    public ImageItemViewModel(string filePath, string res, DateTime date)
    {
        FileName = System.IO.Path.GetFileName(filePath);
        Resolution = res;
        DateModified = date;

        if (System.IO.File.Exists(filePath))
        {
            ImageSource = new Bitmap(filePath);
        }
    }
}