using System;

namespace _1CIntegrationDB
{
    public class Good
    {
        public Int64 good_id { get; set; }
        public string good_key { get; set; }
        public string good { get; set; }
        public int group_id { get; set; }
        public int brand_id { get; set; }
        public string created_on { get; set; }
        public string changed_on { get; set; }
        public string img_path { get; set; }
        public int is_actual { get; set; }
    }
}
