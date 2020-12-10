using Demo.Sqless.Common;
using Demo.Sqless.Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Sqless.Api;
using Sqless.Query;
using Sqless.Request;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Demo.Sqless.Wpf.ViewModel
{
    class ProductsViewModel : ViewModelBase
    {
        public User User => WpfGlobal.LoginUser;

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public async void Init()
        {
            var request = new SqlessSelectRequest()
            {
                AccessParams = new string[] { User.Uid, User.Password },
                Table = Tables.Product
            };
            request.LoadFromType(typeof(Product));

            try
            {
                var products = await SqlessClient.Select<Product>(request);
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (SqlessRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ICommand _purchaseCommand = null;
        public ICommand PurchaseCommand
        {
            get
            {
                if (_purchaseCommand == null)
                {
                    _purchaseCommand = new RelayCommand<Product>(async (product) =>
                      {
                          var orderId = await CreateOrder(product);
                          if (string.IsNullOrEmpty(orderId))
                          {
                              return;
                          }

                          var mbr = MessageBox.Show("Pay now?", "Payment", MessageBoxButton.YesNo);
                          if (mbr != MessageBoxResult.Yes)
                          {
                              return;
                          }

                          await Payment(orderId);
                      });
                }
                return _purchaseCommand;
            }
        }

        private async Task<string> CreateOrder(Product product)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
                Status = 1,
                Time = DateTimeOffset.Now.ToUnixTimeSeconds(),
            };
            var request = new SqlessEditRequest()
            {
                Table = Tables.Order,
                AccessParams = new string[] { User.Uid, User.Password },
            };
            request.LoadFromObject(order);

            try
            {
                await SqlessClient.Insert(request);
                return order.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private async Task<bool> Payment(string orderId)
        {
            var request = new SqlessEditRequest()
            {
                Table = Tables.Order,
                AccessParams = new string[] { User.Uid, User.Password },
                Fields = new System.Collections.Generic.List<SqlessEditField>()
                {
                    new SqlessEditField()
                    {
                        Field=nameof(Order.Status),
                        Value=2,
                        Type=System.Data.DbType.Int32
                    }
                },
                Queries = new System.Collections.Generic.List<SqlessQuery>()
                {
                    new SqlessQuery()
                    {
                        Field=nameof(Product.Id),
                        Type=SqlessQueryType.Equal,
                        Value=orderId
                    }
                }
            };

            try
            {
                await SqlessClient.Update(request);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
