using System;
using System.Collections.Generic;
using System.Text;

namespace GaoJD.Club.BusinessEntity
{
    public class Product
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        public Users UserOwner { get; set; }

        public string CreateTime { get; set; }


    }
}
