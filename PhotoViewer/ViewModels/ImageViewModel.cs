using PhotoViewer.Mvvm;

namespace PhotoViewer.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value; 
                OnPropertyChanged(nameof(Path));
            }
        }

        private bool _hasGaussianBlurEffect;
        public bool HasGaussianBlurEffect
        {
            get { return _hasGaussianBlurEffect; }
            set
            {
                _hasGaussianBlurEffect = value; 
                OnPropertyChanged(nameof(HasGaussianBlurEffect));
            }
        }
    }
}