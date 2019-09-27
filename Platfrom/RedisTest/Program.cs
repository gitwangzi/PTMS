using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
namespace RedisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("按任意键开始Test");
            Console.Read();
            string key = "Test.TestKeyForSixT";
            RedisManager<TestClass> redis = new RedisManager<TestClass>(key);
            int mark = 0;
            for (int i = 0; i <= 7000; i++)
            {
                mark = i + 1;
                TestClass t = new TestClass() { ID = mark, Name = "Name_" + mark.ToString(), Time = DateTime.Now };
                Console.WriteLine(t.ToString());
                if (redis.Hash_Exist(mark.ToString()))
                {
                    Console.WriteLine("True");
                    redis.Hash_Set(mark.ToString(), t);
                }
                else
                {
                    redis.Hash_Add(mark.ToString(), t);
                }
                Console.WriteLine("OK");
            }
            Console.WriteLine("查询数据库中是否存在为1的值");
            Console.Read();
            if (redis.Hash_Exist("1"))
            {
                Console.WriteLine("存在");
                TestClass t = redis.Hash_Get("1");
                Console.WriteLine(t.ToString());
            }
            Console.Read();


        }
    }

    public class TestClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public override string ToString()
        {
            return string.Format("ID:{0},Name:{1},Time:{2}", ID, Name, Time);
        }
    }
}
