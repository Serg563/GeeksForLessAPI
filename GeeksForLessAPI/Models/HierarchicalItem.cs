namespace GeeksForLessAPI.Models
{
    class HierarchicalItem
    {
        public int Id;
        public string Name;
        public int Deepth;

        public HierarchicalItem(int id, string name, int deepth)
        {
            this.Id = id;
            this.Name = name;
            this.Deepth = deepth;
        }
    }
}
