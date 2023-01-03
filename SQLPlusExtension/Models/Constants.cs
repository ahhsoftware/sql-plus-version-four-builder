using System.Collections.Generic;

namespace SQLPlusExtension.Models
{
    public class Constants
    {
        public const string WindowsAuthentication = "Windows Authentication";
        public const string SQLServerAuthentication = "SQL Server Authentication";

        /*"ImplementINotifyPropertyChanged",
    "ImplementIChangeTracking",
    "ImplementIRevertibleChangeTracking",
    "IncludeSynchronousMethods",
    "IncludeAsynchronousMethods",
    "IncludeModels",
    "IncludeServices",
    "IncludeDocumentation",
    "CleanPreviousBuilds"
        */


        public const string ImplementINotifyPropertyChanged = "Implement INotifyPropertyChanged";
        public const string ImplementIChangeTracking = "Implement IChangeTracking";
        public const string ImplementIRevertibleChangeTracking = "Implement IRevertibleChangeTracking";
        public const string IncludeSynchronousMethods = "Include Synchronous Methods";
        public const string IncludeAsynchronousMethods = "Include Asynchronous Methods";

        public static List<string> AuthenticationTypes = new List<string>
        {
            WindowsAuthentication, SQLServerAuthentication
        };

        public static HashSet<string> CSharpKeywords = new HashSet<string>
        {
            "abstract","as","base","bool","break","byte","case","catch","char","checked","class","const","continue","decimal","default","delegate","do","double","else","enum","event","explicit","extern","false","finally","fixed","float","for","foreach","goto","if","implicit","in","int","interface","internal","is","lock","long","namespace","new","null","object","operator","out","override","params","private","protected","public","readonly","record","ref","return","sbyte","sealed","short","sizeof","stackalloc","static","string","struct","switch","this","throw","true","try","typeof","uint","ulong","unchecked","unsafe","ushort","using","virtual","void","volatile","while"
        };

    }
}
