namespace SQLPLUS.Builder.DataCollectors
{
    using SQLPLUS.Builder.TemplateModels;
    using System.Collections.Generic;

    public enum DataCollectorModes
    {
        Configuration,
        Build
    }

    public interface IDataCollector
    {
        bool TestConnection();

        List<Routine> CollectDBRoutines();

        List<Routine> CollectQueryRoutines();

        List<Routine> CollectDBRoutinesAndQueryRoutines();

        List<EnumCollection> CollectEnumCollections();

        List<StaticCollection> CollectStaticCollections();
        
    }
}