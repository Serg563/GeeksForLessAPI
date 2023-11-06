namespace GeeksForLessAPI.Models
{
    class HierarchicalItemCopy
    {
        public int Id;
        public string Name;
        public int ParentId;
        public int Deepth;

        public HierarchicalItemCopy(int id, string name, int parentId, int deepth)
        {
            this.Id = id;
            this.Name = name;
            this.ParentId = parentId;
            this.Deepth = deepth;
        }
    }
}
