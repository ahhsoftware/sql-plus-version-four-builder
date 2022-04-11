using SQLPLUS.Builder.ConfigurationModels;
using SQLPLUS.Builder.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLPLUS.Builder.TemplateModels
{
    public abstract class DBField
    {
        /// <summary>
        /// Index within the collection for this parameter of column.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// This is the native name assigned to the parameter or column.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// This is the property name that the name evaluates to.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Add any annotations to this collection.
        /// </summary>
        public List<string> Annotations { set; get; } = new List<string>();


        #region DataTypeMapping (Shared)

        protected readonly DataTypeMapping dataTypeMapping;
        public string Using => dataTypeMapping.Using;

        #endregion DataTypeMapping (Shared)

        #region Project Information

        protected readonly ProjectInformation project;




        #endregion

        protected readonly BuildDefinition build;

        

        public DBField(
            int? index,
            string name,
            string propertyName,
            ProjectInformation project,
            BuildDefinition build,
            DataTypeMapping mapping
            )
        {
            Index = index ?? throw new ArgumentNullException(nameof(index));
            
            if (name is null)
            {
                //TODO: Better error columns 
                throw new ArgumentNullException("A column was presented without a name");
            }

            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (build is null)
            {
                throw new ArgumentNullException(nameof(build));
            }

            if (mapping is null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }
            
            Name = name;
            PropertyName = propertyName;
            this.project = project;
            this.build = build;
            this.dataTypeMapping = mapping;
        }

        public virtual String PropertyType { get; }
       
        
    }
}
