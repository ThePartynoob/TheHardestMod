using PlusStudioLevelFormat;

namespace TheHardestMod.Extensions
{
    public static class Extensions
    {
        public static BaldiRoomAsset[] LoadFolderRooms(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);
            var d = new List<BaldiRoomAsset>();
            foreach (var item in files)
            {
                BinaryReader binaryReader = new(File.OpenRead(item));
                var a = BaldiRoomAsset.Read(binaryReader);
                d.Add(a);
                binaryReader.Close();

            }
            return d.ToArray();
        }

        public static BaldiRoomAsset LoadRoom(string path)
        {
            
                BinaryReader binaryReader = new(File.OpenRead(path));
                var a = BaldiRoomAsset.Read(binaryReader);
                
                binaryReader.Close();

            
            return a;
        }

    }
}