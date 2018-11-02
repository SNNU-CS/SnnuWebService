using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnnuWebService.Model
{
    public class CardItem
    {
        private string id;
        private DateTime date = DateTime.MinValue;
        private int frequency;
        private double origiAmount;
        private double transAmount;
        private double balance;
        private string location = string.Empty;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }

            set
            {
                frequency = value;
            }
        }

        public double OrigiAmount
        {
            get
            {
                return origiAmount;
            }

            set
            {
                origiAmount = value;
            }
        }

        public double TransAmount
        {
            get
            {
                return transAmount;
            }

            set
            {
                transAmount = value;
            }
        }

        public double Balance
        {
            get
            {
                return balance;
            }

            set
            {
                balance = value;
            }
        }

        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }
    }
}