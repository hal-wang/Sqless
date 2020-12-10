using GalaSoft.MvvmLight.Command;
using Sqless.Api;
using Sqless.Demo.Common;
using Sqless.Demo.Common.Models;
using Sqless.Request;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Sqless.Demo.Wpf.ViewModel
{
    public class OrdersViewModel
    {
        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();

        public async void Init()
        {
            var request = new SqlessSelectRequest()
            {
                AccessParams = new string[] { WpfGlobal.LoginUser.Uid, WpfGlobal.LoginUser.Password },
                Table = Tables.Order
            };
            request.LoadFromType(typeof(Order));

            try
            {
                var orders = await SqlessClient.Select<Order>(request);
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }
            }
            catch (SqlessRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ICommand _deleteCommand = null;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<Order>(async order =>
                    {
                        var request = new SqlessDeleteRequest()
                        {
                            AccessParams = new string[] { WpfGlobal.LoginUser.Uid, WpfGlobal.LoginUser.Password },
                            Table = Tables.Order,
                            Queries = new System.Collections.Generic.List<Query.SqlessQuery>()
                            {
                                new Query.SqlessQuery()
                                {
                                    Field=nameof(Order.Id),
                                    Type=Query.SqlessQueryType.Equal,
                                    Value=order.Id
                                }
                            }
                        };

                        try
                        {
                            await SqlessClient.Delete(request);
                            Orders.Remove(order);
                        }
                        catch (SqlessRequestException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                }
                return _deleteCommand;
            }
        }

        private ICommand _paymentCommand = null;
        public ICommand PaymentCommand
        {
            get
            {
                if (_paymentCommand == null)
                {
                    _paymentCommand = new RelayCommand<Order>(async order =>
                    {
                        var request = new SqlessEditRequest()
                        {
                            Table = Tables.Order,
                            AccessParams = new string[] { WpfGlobal.LoginUser.Uid, WpfGlobal.LoginUser.Password },
                            Fields = new System.Collections.Generic.List<SqlessEditField>()
                            {
                                new SqlessEditField()
                                {
                                    Field=nameof(Order.Status),
                                    Value=2,
                                    Type=System.Data.DbType.Int32
                                }
                            },
                            Queries = new System.Collections.Generic.List<Query.SqlessQuery>()
                            {
                                new Query.SqlessQuery()
                                {
                                    Field=nameof(Product.Id),
                                    Type=Query.SqlessQueryType.Equal,
                                    Value=order.Id
                                }
                            }
                        };

                        try
                        {
                            await SqlessClient.Update(request);
                            order.Status = 2;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                }
                return _paymentCommand;
            }
        }
    }
}
