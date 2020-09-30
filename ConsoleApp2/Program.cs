using System;

namespace ConsoleApp2
{
    delegate int DelGetMax(object o1, object o2);
    class Program
    {
        static void Main(string[] args)
        {
            object[] num = { 1, 2, 3, 4, 6, 5, 8, };
            object[] str = { "fsdfsfewrwew", "fdsfsdfsdf", "fsfdsfsf", "fdsfsd", "fsd" };
            object[] pers = { new Person() { Name = "张三", Age = 23 }, new Person() { Name = "李四", Age = 32 } };
            object max1 = GetMax(num, (Object o1, Object o2) => { return (int)o1 - (int)o2; });
            Console.WriteLine(max1);
            object max2 = GetMax(str, (object o1, object o2) => { return (o1.ToString().Length - o2.ToString().Length); });
            Console.WriteLine(max2);
            object max3 = GetMax(pers, (object o1, object o2) => { return ((Person)o1).Age - ((Person)o2).Age; });
            Console.WriteLine(((Person)max3).Name);
            Console.ReadKey();
        }
        static object GetMax(object[] obj,DelGetMax del)
        {
            object max = obj[0];
            for(int i=0;i<obj.Length;i++)
            {
                if(del(max,obj[i])<0)
                {
                    max = obj[i];
                }
            }
            return max;
        }
    }

    internal class Person
    {
        public Person()
        {
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }
}
