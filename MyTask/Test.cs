using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTask
{
    public class Test<T>
    {


        /// <summary>
        /// 将一组数据平均分成n组
        /// </summary>
        /// <param name="source">要分组的数据源</param>
        /// <param name="n">平均分成n组</param>
        /// <returns><T></returns>
        public static List<List<T>> AverageAssign(List<T> source, int n)
        {
            
            List<List<T>> result = new List<List<T>>();
            int remainder = source.Count() % n;  //(先计算出余数)
            int number = source.Count() / n;  //然后是商
            int offset = 0;//偏移量
            for (int i = 0; i < n; i++)
            {
                List<T> value = null;
                if (remainder > 0)
                { 
                    value = source.Take((i + 1) * number + offset + 1).Skip(i * number + offset).ToList();
                    remainder--;
                    offset++;
                }
                else
                { 
                    value = source.Take((i + 1) * number + offset).Skip(i * number + offset).ToList();
                }
                result.Add(value);
            }
            return result;
        }

    }
}
