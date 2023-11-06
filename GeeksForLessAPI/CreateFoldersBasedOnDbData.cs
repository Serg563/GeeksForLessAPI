









//So when we get data from database inside out List 'items' then we send this data to frontend,
//but we can also create a fodler structure based on them like below 









//List<HierarchicalItemCopy> items = new List<HierarchicalItemCopy>
//    {
//        new HierarchicalItemCopy { Id = 1, Name = "CreatingDigitalImages", ParentId = 0, Deepth = 0 },
//        new HierarchicalItemCopy { Id = 2, Name = "Evidence", ParentId = 1, Deepth = 1 },
//        new HierarchicalItemCopy { Id = 3, Name = "GraphicProducts", ParentId = 1, Deepth = 1 },
//        new HierarchicalItemCopy { Id = 4, Name = "FinalProduct", ParentId = 3, Deepth = 2 },
//        new HierarchicalItemCopy { Id = 5, Name = "Process", ParentId = 3, Deepth = 2 },
//        new HierarchicalItemCopy { Id = 6, Name = "Resources", ParentId = 1, Deepth = 1 },
//        new HierarchicalItemCopy { Id = 7, Name = "PrimaryResources", ParentId = 6, Deepth = 2 },
//        new HierarchicalItemCopy { Id = 8, Name = "SecondaryResources", ParentId = 6, Deepth = 2 },
//    };
//List<FolderItem> folderItems = items.Select(i => new FolderItem
//{
//    Id = i.Id,
//    Category = i.Name,
//    ParentId = i.ParentId,
//    Deepth = i.Deepth
//}).ToList();

//string rootPath = @"D:\TestHierarchyCreation";
//CreateFolders(folderItems, rootPath, 0);
//Console.WriteLine("Folders created successfully.");
//static void CreateFolders(List<FolderItem> items, string rootPath, int parentId)
//{
//    foreach (var item in items)
//    {
//        if (item.ParentId == parentId)
//        {
//            string folderPath = Path.Combine(rootPath, item.Category);
//            Directory.CreateDirectory(folderPath);
//            CreateFolders(items, folderPath, item.Id);
//        }
//    }
//}

//class HierarchicalItemCopy
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public int ParentId { get; set; }
//    public int Deepth { get; set; }
//}

//public class FolderItem
//{
//    public int Id { get; set; }
//    public string Category { get; set; }
//    public int ParentId { get; set; }
//    public int Deepth { get; set; }

//    public List<FolderItem> Children { get; set; } = new List<FolderItem>();
//}
