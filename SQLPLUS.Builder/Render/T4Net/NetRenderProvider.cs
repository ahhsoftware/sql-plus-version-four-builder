using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.TemplateModels;
using System;
using System.Collections.Generic;

namespace SQLPLUS.Builder.Render.T4Net
{
    public class NetRenderProvider : IRenderProvider
    {
        private readonly ProjectInformation project;
        private readonly BuildDefinition build;

        public NetRenderProvider(ProjectInformation project, BuildDefinition build)
        {
            this.project = project;
            this.build = build;
        }
        public string InputObject(Routine routine)
        {
            var template = new InputObject()
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project },
                    { "build", build },
                    { "routine", routine },
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string OutputObject(Routine routine)
        {
            var template = new OutputObject()
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project },
                    { "build", build },
                    { "routine", routine },
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string ServiceBase(string nameSpace)
        {
            var template = new ServiceBase()
            {
                Session = new Dictionary<string, object>
                {
                    { "build", build },
                    { "project", project },
                    { "nameSpace", nameSpace }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string ServiceMethod(Routine routine)
        {
            var template = new ServiceMethod()
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project },
                    { "build", build },
                    { "routine", routine },
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string TransientErrors()
        {
            var template = new TransientErrors()
            {
                Session = new Dictionary<string, object>
                {
                    { "build", build },
                    { "project", project }
                }
            };
            template.Initialize();
            return template.TransformText();

        }
        public string TransientErrorsExample()
        {
            var template = new TransientErrorsExample
            {
                Session = new Dictionary<string, object>
                {
                    { "build", build },
                    { "project", project }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string UserDefinedType(Parameter parameter)
        {
            var template = new UserDefinedType
            {
                Session = new Dictionary<string, object>
                {
                    { "build", build },
                    { "parameter", parameter },
                    { "project", project }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string ValidInput()
        {
            var template = new ValidInput
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string Helpers(List<string> usings, List<string> types, List<Parameter> parameters)
        {
            var template = new T4Net.Helpers()
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project },
                    { "build", build },
                    { "types", types },
                    { "usings", usings },
                    { "parameters", parameters }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string Enumerations(EnumCollection enums)
        {
            var template = new Enumerations
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project },
                    { "enums", enums }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
        public string Statics(StaticCollection data)
        {
            var template = new StaticData
            {
                Session = new Dictionary<string, object>
                {
                    { "project", project },
                    { "data", data }
                }
            };
            template.Initialize();
            return template.TransformText();
        }
    }
}
