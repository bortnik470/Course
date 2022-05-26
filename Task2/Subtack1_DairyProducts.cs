﻿using System;
namespace Course.Task2
{
    class DairyProducts : Product
    {
        private int expirationDate;

        public int ExpirationDate { get => expirationDate; set => expirationDate = value; }

        public DairyProducts()
        {
        }

        public DairyProducts(string name, float price, int weight, int expirationDate) : base(name, price, weight)
        {
            ExpirationDate = expirationDate;
        }

        public override void ChangePrice(int percent)
        {
            percent += expirationDate / 100;
            base.ChangePrice(percent);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DairyProducts)) return false;
            return (this.Name == ((DairyProducts)obj).Name && this.Price == ((DairyProducts)obj).Price && this.Weight == ((DairyProducts)obj).Weight
                 && this.ExpirationDate == ((DairyProducts)obj).ExpirationDate);
        }

        public override string ToString()
        {
            string result = "";
            result += base.ToString() + $"\tExpiration Date: {ExpirationDate}";
            return result;
        }
    }
}
