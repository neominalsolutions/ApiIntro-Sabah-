﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntro.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public short Stock { get; set; }

    }
}
