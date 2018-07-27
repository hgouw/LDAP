using System;
using System.DirectoryServices;

namespace LDAP
{
    class Program
    {
        const string LDAPPATHCORP = "LDAP://corp.regn.net:3268";
        const string LDAPPATHLEGAL = "LDAP://legal.regn.net:3268";
        const string LDAPPATH = "LDAP://OU=REASYD,OU=REA,OU=Users,OU=User Accounts,DC=corp,DC=regn,DC=net";
        const string LDAPPATHWLG = "LDAP://OU=LNGWLG,OU=REA,OU=Users,OU=User Accounts,DC=corp,DC=regn,DC=net";
        const string LDAPNAME = "Svc-corlngsydapp013";
        const string LDAPPWD = "hXEouSfLdpE7OdK12WDC";
        static void Main(string[] args)
        {
            string[] users =
            {
                "gouwh",
                "Herman Gouw",
                "chowdhv1",
                "Chowdhary, Vikarn",
                "Vikarn Chowdhary"
                //"aldersons",
                //"alexanderj001",
                //"ansariz",
                //"arkells",
                //"barfordm",
                //"bavaroa",
                //"bergquistj",
                //"bockmank",
                //"brow12",
                //"browne002",
                //"caona",
                //"chiak001",
                //"chial",
                //"chowdhv1",
                //"corishg",
                //"cosimor",
                //"costad001",
                //"cunninghama",
                //"cunninghama1",
                //"daniel3",
                //"dawkinga",
                //"delacruzg",
                //"fisher1",
                //"fossl",
                //"gardnerj001",
                //"ginnanev",
                //"gouwh",
                //"grainger",
                //"grimaj",
                //"hammanl",
                //"hardingp",
                //"hargreavesa",
                //"harrisc002",
                //"haveyr",
                //"jacobsd001",
                //"kapoord",
                //"kashyapt",
                //"kazis",
                //"klemtv",
                //"lee8",
                //"leeb",
                //"leiss",
                //"lingj",
                //"lohs",
                //"longv",
                //"lulichj001",
                //"lus001",
                //"maessenp1",
                //"malchers",
                //"martina001",
                //"mason1",
                //"mathurs",
                //"mccarthj001",
                //"mcdermottm",
                //"mcnamara1",
                //"muhammas",
                //"munafv",
                //"nawabn",
                //"nonis1",
                //"osaigbo",
                //"osmana",
                //"patrickt",
                //"paulians",
                //"pearce1",
                //"pearsonk",
                //"philippa maessen",
                //"polakn",
                //"ragnii",
                //"riosv",
                //"robertsj",
                //"robeyr",
                //"robinsos",
                //"rookek",
                //"rufm",
                //"sarah s",
                //"sidnells",
                //"simpson1",
                //"stackelbecka",
                //"sues",
                //"tadrosd",
                //"thomsenr",
                //"todj",
                //"vonnidaa",
                //"wallisp",
                //"watsonb",
                //"williamj",
                //"wongs",
                //"yousefr"
            };

            foreach (var user in users)
            {
                Console.WriteLine(GetUserDetails(user));
            }
            Console.ReadLine();
        }

        static string GetUserDetails(string user)
        {
            var searcher = GetSearcher(LDAPPATH, LDAPNAME, LDAPPWD, user);
            var result = searcher.FindOne();
            if (result == null)
            {
                searcher = GetSearcher(LDAPPATHCORP, user);
                result = searcher.FindOne();
                if (result == null)
                {
                    searcher = GetSearcher(LDAPPATHWLG, LDAPNAME, LDAPPWD, user);
                    result = searcher.FindOne();
                    if (result == null)
                    {
                        searcher = GetSearcher(LDAPPATHLEGAL, user);
                        result = searcher.FindOne();
                    }
                }
            }
            var details = "No details for " + user + Environment.NewLine;
            if (result != null)
            {
                details = "Path: " + result.Properties["adspath"][0].ToString() + Environment.NewLine;
                details += "Name: " + result.Properties["displayname"][0].ToString() + Environment.NewLine;
                details += "Dept: " + result.Properties["department"][0].ToString() + Environment.NewLine;
                details += "Title: " + result.Properties["title"][0].ToString() + Environment.NewLine;
                details += "Email: " + result.Properties["mail"][0].ToString() + Environment.NewLine;
            }
            return details;
        }

        static DirectorySearcher GetSearcher(string path, string user)
        {
            var entry = new DirectoryEntry(path);
            var props = entry.Properties;
            var searcher = new DirectorySearcher(entry)
            {
                PageSize = int.MaxValue,
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
            };
            searcher.PropertiesToLoad.Add("displayname");
            searcher.PropertiesToLoad.Add("first");
            searcher.PropertiesToLoad.Add("last");
            searcher.PropertiesToLoad.Add("department");
            searcher.PropertiesToLoad.Add("title");
            searcher.PropertiesToLoad.Add("mail");
            searcher.PropertiesToLoad.Add("telephonenumber");
            return searcher;
        }

        static DirectorySearcher GetSearcher(string path, string name, string password, string user)
        {
            var entry = new DirectoryEntry(path, name, password);
            var props = entry.Properties;
            var searcher = new DirectorySearcher(entry);
            searcher.PropertiesToLoad.Add("displayname");
            searcher.PropertiesToLoad.Add("first");
            searcher.PropertiesToLoad.Add("last");
            searcher.PropertiesToLoad.Add("department");
            searcher.PropertiesToLoad.Add("title");
            searcher.PropertiesToLoad.Add("mail");
            searcher.PropertiesToLoad.Add("telephonenumber");
            searcher.Filter = "(&(objectClass=user)(anr=" + user + "))";
            return searcher;
        }
    }
}