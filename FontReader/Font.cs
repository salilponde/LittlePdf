using FontReader.Tables;

namespace FontReader
{
    public class Font
    {
        public OffsetTable OffsetTable { get; internal set; }
        public HeadTable HeadTable { get; internal set; }
        public MaximumProfileTableBase MaximumProfileTable { get; internal set; }
        public HorizontalHeaderTable HorizontalHeaderTable { get; internal set; }
        public HorizontalMetricsTable HorizontalMetricsTable { get; internal set; }
        public PostScriptTable PostScriptTable { get; internal set; }
        public Os2Table Os2Table { get; internal set; }
        public CharacterMapTable CharacterMapTable { get; internal set; }
    }
}
