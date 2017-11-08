using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using DiaShowWpf.Util;
using WpfUtility.Services;

namespace DiaShowWpf
{
    public class WindowViewModel : ObservableObject
    {
        private string _mainImagePath;

        public string MainImagePath
        {
            get { return _mainImagePath; }
            set
            {
                _mainImagePath = value;
                OnPropertyChanged();
            }
        }
        private string _previousImagePath;

        public string PreviousImagePath
        {
            get { return _previousImagePath; }
            set
            {
                _previousImagePath = value;
                OnPropertyChanged();
            }
        }
        private string _nextImagePath;

        public string NextImagePath
        {
            get { return _nextImagePath; }
            set
            {
                _nextImagePath = value;
                OnPropertyChanged();
            }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
                GetResultsFromPath();
            }
        }

        private double _interval;

        public double Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnPropertyChanged();
            }
        }

        private int _currentIndex = -1;
        private int _index = -1;

        private int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged();
                LoadImages();
            }
        }

        public ICommand ExecuteTimer => new DelegateCommand(SetUpTimer);
        private Timer _timer;

        public Timer Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShufflePictures => new DelegateCommand(() =>
        {
            _searchResults.Shuffle();
            Index = Index + 1;
        });

        public ICommand KeyDownCommand => new RelayCommand<string>(x =>
        {
            if (x.Equals("+1"))
                Index = Index + 1;
            if (x.Equals("-1"))
                Index = Index - 1;
            if (x.Equals("0"))
                _timer.Enabled = !_timer.Enabled;
        });

        public ICommand ExecuteSearch => new DelegateCommand(GetResultsFromPath);
        private ListyList<string> _searchResults;

        public ListyList<string> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public WindowViewModel()
        {
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            Interval = 10d;
            Timer = new Timer(TimeSpan.FromSeconds(Interval).TotalMilliseconds);
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Index = Index + 1;
        }

        private void SetUpTimer()
        {
                Timer.Interval = TimeSpan.FromSeconds(Interval).TotalMilliseconds;
        }

        public void GetResultsFromPath(){
            var files = new ListyList<string>();
            if (Directory.Exists(Path))
            {
                files.AddRange(Directory.EnumerateFiles(Path, "*.*", SearchOption.AllDirectories).ToList()
                    .Where(s => s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".jpg")
                                || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".bmp")).ToList());
            }
            SearchResults = files;
            Index = 0;
        }

        private void LoadImages()
        {
            if (_searchResults.Any())
            {
                var file = Index > _currentIndex ? _searchResults.Next() : _searchResults.Previous();
                _currentIndex = Index;
                MainImagePath = file;
                NextImagePath = _searchResults.Next(+1);
                PreviousImagePath = _searchResults.Next(-1);
            }
        }
    }
}
