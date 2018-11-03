using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnnuWebService.Model
{
    public class LibiaryItem
    {
        private string name;
        private string book;
        private string author;
        private DateTime deadline=DateTime.MinValue;
        private string branch;
        private string Location;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Book
        {
            get
            {
                return book;
            }

            set
            {
                book = value;
            }
        }

        public string Author
        {
            get
            {
                return author;
            }

            set
            {
                author = value;
            }
        }

        public DateTime Deadline
        {
            get
            {
                return deadline;
            }

            set
            {
                deadline = value;
            }
        }

        public string Branch
        {
            get
            {
                return branch;
            }

            set
            {
                branch = value;
            }
        }

        public string Location1
        {
            get
            {
                return Location;
            }

            set
            {
                Location = value;
            }
        }
    }
}