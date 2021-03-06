﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_App1.Service
{
    public class FlatData
    {
        public string ImageSrc { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }
    }


        public class MyClassComparer : IEqualityComparer<FlatData>
        {
            public bool Equals(FlatData x, FlatData y)
            {
                return x.ImageSrc == y.ImageSrc && x.Link == y.Link && x.Title == y.Title;
            }

            public int GetHashCode(FlatData obj)
            {
                return obj.Title.GetHashCode() + obj.Link.GetHashCode() + obj.ImageSrc.GetHashCode();
            }
        }
}
