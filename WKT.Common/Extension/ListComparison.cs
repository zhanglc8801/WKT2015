using System.Collections.Generic;

namespace WKT.Common.Extension
{
    public static class ListComparison
    {
        /// <summary>
        /// 判断两个集合是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns></returns>
        public static bool ReferenceEquals<T>(this List<T> listA, List<T> listB)
        {
            if (listA == null) return false;
            if (listB == null) return false;
            if (listA.Count != listB.Count) return false;

            bool ret = false;

            foreach (T objA in listA)
            {
                if (listB.Contains(objA))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 获取两个集合的差集
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns></returns>
        public static List<T> DifferenceSet<T>(this List<T> listA, List<T> listB)
        {
            List<T> list = new List<T>();
            List<T> listC = new List<T>();
            listC.AddRange(listA);

            if (listA != null && listB != null)
            {
                foreach (T objB in listB)
                {
                    if (listA.Contains(objB))
                    {
                        listC.Remove(objB);
                    }
                }

                list.AddRange(listC);
            }

            return list;
        }
    }
}
