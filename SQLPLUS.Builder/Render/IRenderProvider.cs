using SQLPLUS.Builder.TemplateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLPLUS.Builder.Render
{
    public interface IRenderProvider
    {
        string ValidInput();

        string Helpers(List<string> usings, List<string> types, List<Parameter> parameters);

        string InputObject(Routine routine);

        string TransientErrors();

        string TransientErrorsExample();

        string UserDefinedType(Parameter parameter);

        string OutputObject(Routine routine);

        string ServiceBase(string nameSpace);

        string ServiceMethod(Routine routine);

        string Enumerations(EnumCollection enums);

        string Statics(StaticCollection statics);
    }
}
