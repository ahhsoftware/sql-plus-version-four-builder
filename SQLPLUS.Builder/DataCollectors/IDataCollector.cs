namespace SQLPLUS.Builder.DataCollectors
{
    using SQLPLUS.Builder.TemplateModels;
    using System.Collections.Generic;

    public interface IDataCollector
    {
        /// <summary>
        /// This method should collect all the database routines and all the query routines.
        /// </summary>
        /// <returns>List of Routines.</returns>
        List<Routine> CollectRoutines();

        /// <summary>
        /// Try to establish a connection to the database.
        /// </summary>
        /// <returns>True if connected, otherwise false.</returns>
        bool TestConnection();


        List<EnumCollection> CollectEnumCollections();

        List<StaticCollection> CollectStaticCollections();

    }
}
