using System;
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

        /*public override bool Equals(object obj)
        {
            FlatData other = (FlatData)obj;
            if (other == null)
                return false;

            return this.ImageSrc == other.ImageSrc && this.Link == other.Link && this.Title == other.Title;
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode() + this.ImageSrc.GetHashCode() + this.Link.GetHashCode();
        }*/
    }



        /*public static bool IsEqual<T>(this IEnumerable<T> data1, IEnumerable<T> data2)
        {
            bool result = true;
            using (IEnumerator<T> enum1 = data1.GetEnumerator(), enum2 = data2.GetEnumerator())
            {
                while (true)
                {
                    bool next1 = enum1.MoveNext();
                    bool next2 = enum2.MoveNext();
                    if (next1 == next2)
                    {
                        if (!next1) break;
                        var val1 = enum1.Current;
                        var val2 = enum2.Current;
                        if (val1 == null ? val2 != null : !val1.Equals(val2))
                        {
                            result = false;
                            break;
                        }
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }*/
    

    public class MyClassComparer : IEqualityComparer<FlatData>
    {
        public bool Equals(FlatData x, FlatData y)
        {
            return x.ImageSrc == y.ImageSrc && x.Link == y.Link && x.Title == y.Title;
            }

        public int GetHashCode(FlatData obj)
        {
            return obj.Title.GetHashCode()+obj.Link.GetHashCode()+obj.ImageSrc.GetHashCode();
        }
    }
}
