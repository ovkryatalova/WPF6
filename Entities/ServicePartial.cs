using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF6.Entities
{
    public partial class Service
    {
        public string DiscountText
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return "";
                else
                    return $"* скидка {Discount*100}%";
            }
        }

        public string HasDiscountVisibility
        {
            get
            {
                if (Discount == 0 || Discount == null)
                    return "Collapsed";
                else
                    return "Visible";
            }
        }

        public double CostWithDiscount
        {
            get
            {
                if (Discount > 0)
                    return (double)Cost * (1.00 - Discount.Value);
                else
                    return (double)Cost;
            }
        }
        
        public string TotalCost
        {
            get
            {
                    return $"{CostWithDiscount} рублей за {DurationInSeconds / 60} минут";
            }
        }

        public string BackColor
        {
            get
            {
                if (Discount > 0)
                    return "#D1FFD1";
                else
                    return "White";
            }
        }

        public string AdminControlsVisibility
        { 
            get
            {
                // 1 - админ, 2 - пользователь.
                if (App.CurrentUser.RoleId == 1)
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
        }
    }
}
