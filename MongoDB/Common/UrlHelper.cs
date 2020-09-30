using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace MongoDB.Common
{
    public class UrlHelper
    {
        public List<string> UrlList { get; set; }
        public string  Content { get; set; }
        public string rootName = "G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\";
       
        public UrlHelper(string conetent)
        {
            this.Content = Content;
            this.UrlList = new List<string>();
        }
        public List<string> ExtractUrl(string content)
        {
           
            List<string> UrlList = new List<string>();
            string pattern = @"uri.+?b3dm";
            string fullName = "";
            foreach (Match match in Regex.Matches(content,pattern))
            {
                fullName=match.Value;
                UrlList.Add(fullName);
            }
          
            return UrlList;
        }
        public void FixedUrl(List<string> urlList)
        {
            string temp = "";
            string fullName="";
            foreach (var item in urlList)
            {
                temp = item.Split(':')[1];
                if(temp.Contains('/'))
                {
                  temp= temp.Replace('/','\\');
                  temp = temp.Remove(0, 2);
                }
                fullName = rootName + temp;
                this.UrlList.Add(fullName);
            }
        }
    }
}
