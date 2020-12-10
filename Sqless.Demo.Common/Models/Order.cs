using GalaSoft.MvvmLight;

namespace Sqless.Demo.Common.Models
{
    public class Order : ObservableObject
    {
        public string Id { get; set; }
        public string Uid { get; set; }
        public string ProductId { get; set; }
        public long Time { get; set; }

        private int _status;
        public int Status
        {
            get => _status;
            set => Set(ref _status, value);
        }
    }
}
