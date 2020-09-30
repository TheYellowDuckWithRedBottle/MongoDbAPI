using System;

namespace ConsoleApp1
{
    public delegate int DelChange(string[] names);
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get the type of a specified class.
                Type myType1 = Type.GetType("System.String");
                Console.WriteLine("The full name is {0}.\n", myType1);
            }
            catch (TypeLoadException e)
            {
                Console.WriteLine("{0}: Unable to load type System.Int32", e.GetType().Name);
            }

            try
            {
                // Since NoneSuch does not exist in this assembly, GetType throws a TypeLoadException.
                Type myType2 = Type.GetType("NoneSuch", true);
                Console.WriteLine("The full name is {0}.", myType2.FullName);
            }
            catch (TypeLoadException e)
            {
                Console.WriteLine("{0}: Unable to load type NoneSuch", e.GetType().Name);
            }
        }
        static int ProToUpper(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i].ToUpper();
            }
            return 0;
        }
        static int ProToLower(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i].ToLower();
            }
            return 1;
        }
        static int ProAddQuo(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = "\"" + names[i] + "\"";
            }
            return 2;
        }

        static void Change(string[] names,DelChange del)
        {
            del(names);
        }
    }
}
