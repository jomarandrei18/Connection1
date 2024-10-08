﻿using Connection1.Entities;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Class
{
    public class MenuValidation
    {
        private List<OrderList> _orderList;
        public MenuValidation(List<OrderList> orderList)
        {
            _orderList = orderList;

        }
        public int CheckNoLessZero(int quantity)
        {
            return quantity > 1 ? --quantity : 1;
        }
            
        public int CheckIfExistInList(string categName, string productName, decimal price, int Id, string abb, out bool con)
        {
            var list = _orderList.Find(r => r.ProductId == Id);
            int orderId = _orderList.Count == 0? 1 : _orderList[_orderList.Count- 1].Id + 1;
             con = false;

            if (list == null)
            {
                var _order = new OrderList
                {
                    Id = orderId,
                    CategName = categName,
                    Quantity = 1,
                    Price = price,
                    ProductId = Id,
                    ProductName = productName,
                    Abbreviation = abb
                };
                con = true;
                _orderList.Add(_order);
                
            }

            return list == null ? orderId : list.Id;
        }

        public string GetCategShortname(string categName)
        {
            string shortName = "";
            var categ = categName.Split(' ');
            foreach (var item in categ)
            {
                shortName = shortName + item[0];
            }
            return shortName;
        }

    }
}
