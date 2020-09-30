using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.Common
{
    public class ClassHelper
    {

        public static object CreateInstance(Type t)
        {
            return Activator.CreateInstance(t);
        }
        public static object CreateInstance(string className, List<CustPropertyInfo> lcpi)
        {
            Type type = null;
            Type t = AddProperty(type, lcpi);
            return Activator.CreateInstance(t);
        }
        /// 返回创建的类的实例。
        public static object CreateInstance(List<CustPropertyInfo> lcpi)
        {
            return CreateInstance("DefaultClass", lcpi);
        }
        public static void SetPropertyValue(object classInstance, string propertyName, object propertSetValue)
        {
            classInstance.GetType().InvokeMember(propertyName, BindingFlags.SetProperty,
                                          null, classInstance, new object[] { Convert.ChangeType(propertSetValue, propertSetValue.GetType()) });
        }
        public static Type BuildType(string className)
        {

            AppDomain myDomain = Thread.GetDomain();
            AssemblyName myAsmName = new AssemblyName();
            myAsmName.Name = "MyDynamicAssembly";

            //创建一个永久程序集，设置为AssemblyBuilderAccess.RunAndSave。
            AssemblyBuilder myAsmBuilder = AssemblyBuilder.DefineDynamicAssembly(myAsmName,
                                                            AssemblyBuilderAccess.Run);

            //创建一个永久单模程序块。
            ModuleBuilder myModBuilder =
                myAsmBuilder.DefineDynamicModule(myAsmName.Name);
            //创建TypeBuilder。
            TypeBuilder myTypeBuilder = myModBuilder.DefineType(className,
                                                            TypeAttributes.Public);

            //创建类型。
            Type retval = myTypeBuilder.CreateType();

            //保存程序集，以便可以被Ildasm.exe解析，或被测试程序引用。
           // myAsmBuilder.Save(myAsmName.Name + ".dll");
            return retval;
        }

        public static object GetPropertyValue(object classInstance, string propertyName)
        {
            return classInstance.GetType().InvokeMember(propertyName, BindingFlags.GetProperty,
                                                          null, classInstance, new object[] { });
        }

  
        /// 

        /// 创建一个没有成员的类型的实例，类名为"DefaultClass"。
        /// 
        /// 返回创建的类型的实例。

        /// 

        /// 根据类名创建一个没有成员的类型的实例。
        /// 
        /// 将要创建的类型的实例的类名。
        /// 返回创建的类型的实例。
      
        public static Type AddProperty(Type classType, List<CustPropertyInfo> lcpi)
        {
            //合并先前的属性，以便一起在下一步进行处理。
            MergeProperty(classType, lcpi);
            //把属性加入到Type。
            return AddPropertyToType(classType, lcpi);
        }
        public static Type AddProperty(Type classType, CustPropertyInfo cpi)
        {
            List<CustPropertyInfo> lcpi = new List<CustPropertyInfo>();
            lcpi.Add(cpi);
            //合并先前的属性，以便一起在下一步进行处理。
            MergeProperty(classType, lcpi);
            //把属性加入到Type。
            return AddPropertyToType(classType, lcpi);
        }
        public static Type DeleteProperty(Type classType, string propertyName)
        {
            List<string> ls = new List<string>();
            ls.Add(propertyName);

            //合并先前的属性，以便一起在下一步进行处理。
            List<CustPropertyInfo> lcpi = SeparateProperty(classType, ls);
            //把属性加入到Type。
            return AddPropertyToType(classType, lcpi);
        }
        public static Type DeleteProperty(Type classType, List<string> ls)
        {
            //合并先前的属性，以便一起在下一步进行处理。
            List<CustPropertyInfo> lcpi = SeparateProperty(classType, ls);
            //把属性加入到Type。
            return AddPropertyToType(classType, lcpi);
        }
        private static void MergeProperty(Type t, List<CustPropertyInfo> lcpi)
        {
            CustPropertyInfo cpi;
            foreach (PropertyInfo pi in t.GetProperties())
            {
                cpi = new CustPropertyInfo(pi.PropertyType.FullName, pi.Name);
                lcpi.Add(cpi);
            }
        }
        private static List<CustPropertyInfo> SeparateProperty(Type t, List<string> ls)
        {
            List<CustPropertyInfo> ret = new List<CustPropertyInfo>();
            CustPropertyInfo cpi;
            foreach (PropertyInfo pi in t.GetProperties())
            {
                foreach (string s in ls)
                {
                    if (pi.Name != s)
                    {
                        cpi = new CustPropertyInfo(pi.PropertyType.FullName, pi.Name);
                        ret.Add(cpi);
                    }
                }
            }

            return ret;
        }
        private static void AddPropertyToTypeBuilder(TypeBuilder myTypeBuilder, List<CustPropertyInfo> lcpi)
        {
            FieldBuilder customerNameBldr;
            PropertyBuilder custNamePropBldr;
            MethodBuilder custNameGetPropMthdBldr;
            MethodBuilder custNameSetPropMthdBldr;
            MethodAttributes getSetAttr;
            ILGenerator custNameGetIL;
            ILGenerator custNameSetIL;

            // 属性Set和Get方法要一个专门的属性。这里设置为Public。
            getSetAttr =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig;

            // 添加属性到myTypeBuilder。
            foreach (CustPropertyInfo cpi in lcpi)
            {
                //定义字段。
                string FieldName = cpi.FieldName;
                Type type = Type.GetType("System.String");

                customerNameBldr = myTypeBuilder.DefineField(FieldName,
                                                                 type,
                                                                 FieldAttributes.Private);

                //定义属性。
                //最后一个参数为null，因为属性没有参数。
                custNamePropBldr = myTypeBuilder.DefineProperty(cpi.PropertyName,
                                                                 PropertyAttributes.HasDefault,
                                                                 Type.GetType(cpi.Type),
                                                                 null);


                //定义Get方法。
                custNameGetPropMthdBldr =
                    myTypeBuilder.DefineMethod(cpi.GetPropertyMethodName,
                                               getSetAttr,
                                               Type.GetType(cpi.Type),
                                               Type.EmptyTypes);

                custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();

                custNameGetIL.Emit(OpCodes.Ldarg_0);
                custNameGetIL.Emit(OpCodes.Ldfld, customerNameBldr);
                custNameGetIL.Emit(OpCodes.Ret);

                //定义Set方法。
                custNameSetPropMthdBldr =
                    myTypeBuilder.DefineMethod(cpi.SetPropertyMethodName,
                                               getSetAttr,
                                               null,
                                               new Type[] { Type.GetType("System.String") });

                custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

                custNameSetIL.Emit(OpCodes.Ldarg_0);
                custNameSetIL.Emit(OpCodes.Ldarg_1);
                custNameSetIL.Emit(OpCodes.Stfld, customerNameBldr);
                custNameSetIL.Emit(OpCodes.Ret);

                //把创建的两个方法(Get,Set)加入到PropertyBuilder中。
                custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
                custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);
            }
        }
        public static Type AddPropertyToType(Type classType, List<CustPropertyInfo> lcpi)
        {
            AppDomain myDomain = AppDomain.CurrentDomain;
           
            AssemblyName myAsmName = new AssemblyName("MongDB");

            //创建一个永久程序集，设置为AssemblyBuilderAccess.RunAndSave。
            AssemblyBuilder myAsmBuilder = AssemblyBuilder.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndCollect);
                //myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndCollect);


            ModuleBuilder myModBuilder = myAsmBuilder.DefineDynamicModule("type");
            TypeBuilder myTypeBuilder = myModBuilder.DefineType(classType.FullName,
                                                            TypeAttributes.Public);

            //把lcpi中定义的属性加入到TypeBuilder。将清空其它的成员。其功能有待扩展，使其不影响其它成员。
            AddPropertyToTypeBuilder(myTypeBuilder, lcpi);

            //创建类型。
            Type retval = myTypeBuilder.CreateType();

            //保存程序集，以便可以被Ildasm.exe解析，或被测试程序引用。
       
            return retval;
        }
        public class CustPropertyInfo
        {
            private string propertyName;
            private string type;

            /// 

            /// 空构造。
            /// 
            public CustPropertyInfo() { }

            /// 

            /// 根据属性类型名称，属性名称构造实例。
            /// 
            /// 属性类型名称。
            /// 属性名称。
            public CustPropertyInfo(string type, string propertyName)
            {
                this.type = type;
                this.propertyName = propertyName;
            }

            /// 

            /// 获取或设置属性类型名称。
            /// 
            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            /// 

            /// 获取或设置属性名称。
            /// 
            public string PropertyName
            {
                get { return propertyName; }
                set { propertyName = value; }
            }

            /// 

            /// 获取属性字段名称。
            /// 
            public string FieldName
            {
                get { return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1); }
            }

            /// 

            /// 获取属性在IL中的Set方法名。
            /// 
            public string SetPropertyMethodName
            {
                get { return "set_" + PropertyName; }
            }

            /// 

            ///  获取属性在IL中的Get方法名。
            /// 
            public string GetPropertyMethodName
            {
                get { return "get_" + PropertyName; }
            }
        }
    }
}
